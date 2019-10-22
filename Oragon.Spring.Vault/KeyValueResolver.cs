using Oragon.Configuration;
using System;
using System.Threading.Tasks;
using VaultSharp;

namespace Oragon.Spring.Vault
{
    public class KeyValueResolver : ConfigurationResolverBase
    {

        public KeyValueResolver(VaultClient client)
        {
            this.Client = client;
        }

        public IVaultClient Client { get; }

        public string Path { get; set; }

        public string Key { get; set; }

        public override async Task<string> GetConfigurationAsync()
        {
            var secret = await this.Client.V1.Secrets.KeyValue.V2.ReadSecretAsync(this.Path);
            if (secret.Data.Data.ContainsKey(this.Key))
            {
                return (string)secret.Data.Data[this.Key] ?? "";
            }
            return null;
        }
    }
}
