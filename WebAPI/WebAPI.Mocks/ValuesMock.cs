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
using WebAPI.Model.Dto.Read;
using WebAPI.DAL;

namespace WebAPI.Mocks
{
    public class ValuesMock
    {
        #region Values

        private static IList<ValueReadDto> values = new List<ValueReadDto>
        {
            new ValueReadDto
            {
                Id = 1,
                Name = "Jeden",
                Description = "test"
            },
            new ValueReadDto
            {
                Id = 2,
                Name = "Dwa"
            },
            new ValueReadDto
            {
                Id = 3,
                Name = "Trzy"
            },
            new ValueReadDto
            {
                Id = 4,
                Name = "Trzy"
            }
        };

        #endregion

        #region Children

        private static IList<ChildReadDto> Children = new List<ChildReadDto>
        {
            new ChildReadDto { Id = 1, ChildName = "Child name 1"},
            new ChildReadDto { Id = 2, ChildName = "Child name 2"}
        };

        #endregion

        #region Constructor

        static ValuesMock()
        {
            values[0].Children = Children;
        }

        #endregion

        public static IValuesRepository GetValueRepositoryMock()
        {
            var mock = new Mock<IValuesRepository>();

            mock.Setup(m => m.Get(It.IsAny<BaseSearchParams>())).Returns<BaseSearchParams>(s =>
            {
                var task = Task.Factory.StartNew(() =>
                  {
                      var items = values.AsQueryable();
                      var count = values.Count;

                      items = items.DynamicSort(s.Sort);
                      items = items.Page(s.Page.Value, s.Items.Value);

                      var apiCollection = new ApiCollection<ValueReadDto>(items.ToList()) { TotalCount = count };

                      return apiCollection;
                  });

                return task;
            });

            mock.Setup(m => m.Get(It.IsAny<int>())).Returns<int>(i => Task.Factory.StartNew(() => values.AsQueryable().FirstOrDefault(v => v.Id == i)));

            mock.Setup(m => m.Update(It.IsAny<int>(), It.IsAny<ValueReadDto>())).Returns<int, ValueReadDto>((i, v) =>
            {
                var task = Task.Factory.StartNew(() =>
                {
                    var index = values.IndexOf(v);
                    values[index] = v;

                    return v;
                });

                return task;
            });

            return mock.Object;
        }
    }
}
