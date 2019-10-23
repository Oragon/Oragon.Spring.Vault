using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Oragon.Spring.Vault.Tests
{
    [Trait("category", "unit")]
    public class SpringHelperTests
    {
        [Fact]
        public void NullOnType()
        {
            var result = SpringHelper.GetFilePath("a", null);
            Assert.Equal("a", result);
        }

        [Fact]
        public void Path()
        {
            var result = SpringHelper.GetFilePath("../../../../../Oragon.Spring.Vault.Tests.dll", typeof(SpringHelperTests));
            Assert.EndsWith("Oragon.Spring.Vault.Tests.dll", result);
        }

        [Fact]
        public void GetContainer()
        {
            var container = SpringHelper.GetContainer("config-01.xml");
        }
    }
}
