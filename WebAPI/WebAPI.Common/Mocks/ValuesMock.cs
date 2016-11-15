using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Contracts.DAL;
using Moq;
using WebAPI.DAL.Models;
using WebAPI.Common.Extensions;
using JsonPatch;

namespace WebAPI.Common.Mocks
{
    public class ValuesMock
    {
        #region Values

        private static IEnumerable<Value> values = new List<Value>
        {
            new Value
            {
                Id = 1,
                Name = "Jeden"
            },
            new Value
            {
                Id = 2,
                Name = "Dwa"
            },
            new Value
            {
                Id = 3,
                Name = "Dwa"
            }
        };

        #endregion

        public static IValuesRepository GetValueRepositoryMock()
        {
            var mock = new Mock<IValuesRepository>();
            mock.Setup(m => m.Get(It.IsAny<string>())).Returns<string>(s => values.AsQueryable().DynamicSort(s).ToList());
            
            return mock.Object;
        }
    }
}
