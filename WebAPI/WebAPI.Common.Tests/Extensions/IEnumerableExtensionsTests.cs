using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Common.Exceptions;
using WebAPI.Common.Extensions;
using Xunit;

namespace WebAPI.Common.Tests.Extensions
{
    public class IEnumerableExtensionsTests
    {
        public IEnumerableExtensionsTests()
        {

        }

        [Fact]
        public void first_returns_element()
        {
            var col = new List<string>
            {
                "a",
                "b"
            };

            var result = col.First<string, NotFoundException>(c => c == "a", "not found");
        }

        [Fact]
        public void first_incorrect_throws_exception()
        {
            var col = new List<string>
            {
                "a",
                "b"
            };

            Assert.Throws<NotFoundException>(() => col.First<string, NotFoundException>(c => c == "c", "not found"));
        }
    }
}
