using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Contracts.DAL;
using Moq;
using WebAPI.DAL.Models;
using WebAPI.Common.Extensions;
using WebAPI.Model.Dto.Update;

namespace WebAPI.Common.Mocks
{
    public class ValuesMock
    {
        #region Values

        private static IList<Value> values = new List<Value>
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
            mock.Setup(m => m.Get(It.IsAny<int>())).Returns<int>(i => values.ElementAt(i));
            mock.Setup(m => m.Update(It.IsAny<int>(), It.IsAny<Value>())).Returns<int, Value>((i, v) =>
            {
                var oldId = values[i].Id;
                values[i] = v;
                v.Id = oldId;

                return v;
            });

            return mock.Object;
        }
    }
}
