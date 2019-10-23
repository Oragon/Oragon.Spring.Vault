using Oragon.Configuration;
using System;
using System.Threading.Tasks;
using VaultSharp;

namespace Oragon.Spring.Vault
{
    /// <summary>
    /// Implements IConfigurationResolver using Hashicorp Vault KeyValues
    /// </summary>
    public class KeyValueResolver : ConfigurationResolverBase
    {

        private KeyValueManager manager { get; }

        /// <summary>
        /// Create new KeyValueResolver
        /// </summary>
        /// <param name="client">VaultClient configured</param>
        public KeyValueResolver(KeyValueManager manager)
        {
            this.manager = manager;
        }


        /// <summary>
        /// Key to use as Configuration
        /// </summary>
        public string Key { get; set; }


        /// <summary>
        /// Connect on Hashicorp Vault and get specific key 
        /// </summary>
        /// <returns>Key string</returns>
        public override async Task<string> GetConfigurationAsync()
        {
            var dic = await this.manager.GetKeyValuePairsAsync();
            if (dic != null && dic.ContainsKey(this.Key))
            {
                return (string)dic[this.Key];
            }
            return null;
        }
    }
}
