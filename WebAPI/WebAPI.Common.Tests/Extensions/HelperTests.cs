using Shouldly;
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
            Helper.AreFieldsValid<Model>("Name").ShouldBe(true);
            Helper.AreFieldsValid<Model>("name").ShouldBe(true);
            Helper.AreFieldsValid<Model>("number,Name").ShouldBe(true);
        }

        [Fact]
        public void AreFieldsValid_returns_false_for_invalid_field()
        {
            Helper.AreFieldsValid<Model>("invalid").ShouldBe(false);
        }

        [Fact]
        public void CreateDynamicSelectExpression_selects_proper_field()
        {
            var col = new List<Model> { new Model { Name = "a", Number = 1 } };
            var expression = Helper.CreateDynamicSelectExpression<Model>("number");
            var func = expression.Compile();
            var result = col.Select(func);

            result.All(r => r.Number.Equals(1)).ShouldBe(true);
            result.All(r => r.Name == null).ShouldBe(true);
        }
    }
}
