using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Oragon.Spring.Vault.Tests
{
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
    }
}
