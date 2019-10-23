using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VaultSharp;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.KeyValue.V2;
using Xunit;

namespace Oragon.Spring.Vault.Tests.Unit
{
    [Trait("category", "unit")]
    public class KeyValueManagerTests
    {
        public static KeyValueManager BuildCase1()
        {
            var IVaultClientMock = new Mock<IVaultClient>();

            Func<Secret<SecretData>> buildDic = () => new Secret<SecretData>()
            {
                Data = new SecretData()
                {
                    Data = new Dictionary<string, object> {
                        { "A", "default-value" },
                    }
                }
            };

            IVaultClientMock.Setup(it => it.V1.Secrets.KeyValue.V2.ReadSecretAsync(It.IsIn("path"), null, "kv", null))
            .ReturnsAsync(buildDic);


            var manager = new KeyValueManager(IVaultClientMock.Object);
            return manager;
        }

        [Fact]
        public async Task CacheTest()
        {
            var manager = BuildCase1();
            manager.TTL = TimeSpan.FromSeconds(2);
            manager.Path = "path";

            var newDic = await manager.GetKeyValuePairsAsync(false);
            Assert.Equal("default-value", ((string)newDic["A"]));

            newDic["A"] = "OVERRIDE";

            newDic = await manager.GetKeyValuePairsAsync(false);
            Assert.Equal("OVERRIDE", ((string)newDic["A"]));

        }

        [Fact]
        public async Task CheckTTL()
        {
            var manager = BuildCase1();
            manager.TTL = TimeSpan.FromMilliseconds(200);
            manager.Path = "path";

            var newDic = await manager.GetKeyValuePairsAsync(false);
            Assert.Equal("default-value", ((string)newDic["A"]));

            newDic["A"] = "OVERRIDE";

            newDic = await manager.GetKeyValuePairsAsync(false);
            Assert.Equal("OVERRIDE", ((string)newDic["A"]));

            System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(200));

            newDic = await manager.GetKeyValuePairsAsync(false);
            Assert.Equal("default-value", ((string)newDic["A"]));
        }

        [Fact]
        public async Task IgnoreCacheTest()
        {
            var manager = BuildCase1();
            manager.TTL = TimeSpan.FromSeconds(2);
            manager.Path = "path";

            var newDic = await manager.GetKeyValuePairsAsync(false);
            Assert.Equal("default-value", ((string)newDic["A"]));

            newDic["A"] = "OVERRIDE";

            newDic = await manager.GetKeyValuePairsAsync(false);
            Assert.Equal("OVERRIDE", ((string)newDic["A"]));

            newDic = await manager.GetKeyValuePairsAsync(true);
            Assert.Equal("default-value", ((string)newDic["A"]));
        }

        
    }
}
