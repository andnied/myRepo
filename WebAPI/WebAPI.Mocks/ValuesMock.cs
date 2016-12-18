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
using WebAPI.Model.SearchParams;
using WebAPI.Common.Structures;

namespace WebAPI.Mocks
{
    public class ValuesMock
    {
        #region Values

        private static IList<Value> values = new List<Value>
        {
            new Value
            {
                Id = 1,
                Name = "Jeden",
                Description = "test"
            },
            new Value
            {
                Id = 2,
                Name = "Dwa"
            },
            new Value
            {
                Id = 3,
                Name = "Trzy"
            },
            new Value
            {
                Id = 4,
                Name = "Trzy"
            }
        };

        #endregion

        public static IValuesRepository GetValueRepositoryMock()
        {
            var mock = new Mock<IValuesRepository>();

            mock.Setup(m => m.Get(It.IsAny<BaseSearchParams>())).Returns<BaseSearchParams>(s =>
            {
                var items = values.AsQueryable();
                var count = values.Count;
                items = items.DynamicSort(s.Sort);
                items = items.Page(s.Page.Value, s.Items.Value);

                var apiCollection = new ApiCollection<Value>(items.ToList()) { TotalCount = count };

                return apiCollection;
            });

            mock.Setup(m => m.Get(It.IsAny<int>())).Returns<int>(i => values.FirstOrDefault(v => v.Id == i));

            mock.Setup(m => m.Update(It.IsAny<int>(), It.IsAny<Value>())).Returns<int, Value>((i, v) =>
            {
                var index = values.IndexOf(v);
                values[index] = v;

                return v;
            });

            return mock.Object;
        }
    }
}
