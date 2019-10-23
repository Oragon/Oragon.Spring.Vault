using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VaultSharp;

namespace Oragon.Spring.Vault
{

    /// <summary>
    /// Manage and Cache Requests for KeyValue
    /// </summary>
    public class KeyValueManager
    {
        private IVaultClient client { get; }

        private Dictionary<String, Object> cachedValues;

        private DateTime? lastExecution;

        private object syncLock = new object();

        /// <summary>
        /// Time to live
        /// </summary>
        /// <remarks>Express the time until retry get this value</remarks>
        public TimeSpan TTL { get; set; }

        /// <summary>
        /// Secret path on Hashicorp Vault
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Create new KeyValueManager
        /// </summary>
        /// <param name="client">VaultClient configured</param>
        public KeyValueManager(IVaultClient client)
        {
            this.client = client;
            this.lastExecution = null;
        }

        /// <summary>
        /// Extensible point to override the way to call Vault
        /// </summary>
        /// <returns>Dictionary of string (keys) and object (values)</returns>
        protected virtual async Task<Dictionary<string, object>> GetKeyValuePairsInternalAsync() => (await this.client.V1.Secrets.KeyValue.V2.ReadSecretAsync(this.Path))?.Data?.Data;


        /// <summary>
        /// Get Dictionary from cache or live
        /// </summary>
        /// <param name="ignoreCache">Force ignore cache and get data from </param>
        /// <returns></returns>
        public virtual async Task<Dictionary<String, Object>> GetKeyValuePairsAsync(bool ignoreCache = false)
        {
            DateTime now = DateTime.UtcNow;
            if (ignoreCache || this.lastExecution == null || this.lastExecution.Value.Add(this.TTL) < now)
            {
                var secretDictionary = await this.GetKeyValuePairsInternalAsync();
                if (secretDictionary != null)
                    lock (syncLock)
                    {
                        this.lastExecution = now;
                        this.cachedValues = secretDictionary;
                    }
            }
            return this.cachedValues;
        }
    }
}
