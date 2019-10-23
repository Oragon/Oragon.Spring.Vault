using System;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;
using Xunit;

namespace Oragon.Spring.Vault.Tests
{

    //[Trait("category", "integrated")]
    //public class KeyValueTests
    //{
    //    [Fact]
    //    public void HappyPath()
    //    {
    //        var container = SpringHelper.GetContainer("config-01.xml");
    //        var keyValueResolver = container.GetObject<KeyValueResolver>();
    //        var config = keyValueResolver.GetConfiguration();
    //        Assert.Equal("value", config);

    //        config = keyValueResolver.GetConfiguration();
    //        Assert.Equal("value", config);
    //    }

    //    [Fact]
    //    public void UnknowKeyProducesNull()
    //    {
    //        var container = SpringHelper.GetContainer("config-01.xml");
    //        var keyValueResolver = container.GetObject<KeyValueResolver>();
    //        keyValueResolver.Key = "null";
    //        var config = keyValueResolver.GetConfiguration();
    //        Assert.Null(config);
    //    }
    //}
}
