using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Oragon.Spring.Vault.Tests.Unit
{
    [Trait("category", "unit")]
    public class KeyValueResolverTests
    {
        [Fact]    
        public void DefaultSync()
        {
            var manager = KeyValueManagerTests.BuildCase1();
            manager.TTL = TimeSpan.FromSeconds(2);
            manager.Path = "path";
            var resolver = new KeyValueResolver(manager)
            {
                Key = "A"
            };

            Assert.Equal("A", resolver.Key);

            var result = resolver.GetConfiguration();
            Assert.Equal("default-value", result);
        }

        [Fact]
        public async Task DefaultAsync()
        {
            var manager = KeyValueManagerTests.BuildCase1();
            manager.TTL = TimeSpan.FromSeconds(2);
            manager.Path = "path";
            var resolver = new KeyValueResolver(manager)
            {
                Key = "A"
            };

            Assert.Equal("A", resolver.Key);

            var result = await resolver.GetConfigurationAsync();
            Assert.Equal("default-value", result);
        }

        [Fact]
        public async Task WrongPath()
        {
            var manager = KeyValueManagerTests.BuildCase1();
            manager.TTL = TimeSpan.FromSeconds(2);
            manager.Path = "path_a";
            var resolver = new KeyValueResolver(manager)
            {
                Key = "A"
            };

            var result = await resolver.GetConfigurationAsync();
            Assert.Null(result);
        }

        [Fact]
        public async Task WrongKeyAsync()
        {
            var manager = KeyValueManagerTests.BuildCase1();
            manager.TTL = TimeSpan.FromSeconds(2);
            manager.Path = "path";
            var resolver = new KeyValueResolver(manager)
            {
                Key = "B"
            };

            var result = await resolver.GetConfigurationAsync();
            Assert.Null(result);
        }

        [Fact]
        public void WrongKeySync()
        {
            var manager = KeyValueManagerTests.BuildCase1();
            manager.TTL = TimeSpan.FromSeconds(2);
            manager.Path = "path";
            var resolver = new KeyValueResolver(manager)
            {
                Key = "B"
            };

            var result = resolver.GetConfiguration();
            Assert.Null(result);
        }

        [Fact]
        public void WrongPathaAndKey()
        {
            var manager = KeyValueManagerTests.BuildCase1();
            manager.TTL = TimeSpan.FromSeconds(2);
            manager.Path = "xxx";
            var resolver = new KeyValueResolver(manager)
            {
                Key = "xxx"
            };

            var result = resolver.GetConfiguration();
            Assert.Null(result);
        }
    }
}
