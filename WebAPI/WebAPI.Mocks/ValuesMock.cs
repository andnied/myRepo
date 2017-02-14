using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Contracts.DAL;
using Moq;
using WebAPI.Model.Models;
using WebAPI.Common.Extensions;
using WebAPI.Common.SearchParams;
using WebAPI.Common.Structures;
using WebAPI.Common.Exceptions;

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

        #region Children

        private static IList<Child> Children = new List<Child>
        {
            new Child { Id = 1, ChildName = "Child name 1"},
            new Child { Id = 2, ChildName = "Child name 2"}
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

                      if (s.Fields != null)
                          items = items.DynamicSelect(s.Fields);

                      items = items.DynamicSort(s.Sort);
                      items = items.Page(s.Page.Value, s.Items.Value);

                      var apiCollection = new ApiCollection(items.ToList()) { TotalCount = count };

                      return apiCollection;
                  });

                return task;
            });

            mock.Setup(m => m.Get(It.IsAny<int>())).Returns<int>(i => Task.Factory.StartNew(() =>
            {
                var value = values.First<Value, NotFoundException>(v => v.Id == i, string.Format("Value with id {0} not found.", i));

                return value;
            }));

            mock.Setup(m => m.Update(It.IsAny<int>(), It.IsAny<Value>())).Returns<int, Value>((i, v) =>
            {
                var task = Task.Factory.StartNew(() =>
                {
                    var index = values.IndexOf(v);

                    if (index == -1)
                    {
                        var toBeUpdated = values.First<Value, NotFoundException>(va => va.Id == i, string.Format("Value with id {0} not found.", i));

                        index = values.IndexOf(toBeUpdated);
                    }

                    values[index] = v;

                    return v;
                });

                return task;
            });

            mock.Setup(m => m.Add(It.IsAny<Value>())).Returns<Value>(v =>
            {
                var task = Task.Factory.StartNew(() =>
                {
                    values.Add(v);
                    v.Id = values.Count;

                    return v;
                });

                return task;
            });

            mock.Setup(m => m.Delete(It.IsAny<int>())).Returns<int>((i) =>
            {
                var task = Task.Factory.StartNew(() =>
                {
                    var value = values.First<Value, NotFoundException>(v => v.Id == i, string.Format("Value with id {0} not found.", i));

                    values.Remove(value);
                });

                return task;
            });

            return mock.Object;
        }
    }
}
