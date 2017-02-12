using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Common.Utils;
using Xunit;

namespace WebAPI.Common.Tests.Extensions
{
    public class HelperTests
    {
        private class Model
        {
            public int Number { get; set; }
            public string Name { get; set; }
        }

        [Fact]
        public void AreFieldsValid_returns_true_for_valid_field()
        {
            Assert.True(Helper.AreFieldsValid<Model>("Name"));
            Assert.True(Helper.AreFieldsValid<Model>("name"));
            Assert.True(Helper.AreFieldsValid<Model>("number,Name"));
        }

        [Fact]
        public void AreFieldsValid_returns_false_for_invalid_field()
        {
            Assert.False(Helper.AreFieldsValid<Model>("invalid"));
        }
    }
}
