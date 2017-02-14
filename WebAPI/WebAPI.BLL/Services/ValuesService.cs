using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Common.Exceptions;
using WebAPI.Common.Structures;
using WebAPI.Contracts.BLL;
using WebAPI.Model.Models;
using WebAPI.Mapper;
using WebAPI.Common.Dto.Read;
using WebAPI.Common.SearchParams;

namespace WebAPI.BLL.Services
{
    public class ValuesService : IValuesService
    {
        private readonly WebApiMapper _mapper;

        public ValuesService(WebApiMapper mapper)
        {
            _mapper = mapper;
        }

        public ApiCollection GetDtoCollection(ApiCollection entityCollection, BaseSearchParams searchParams)
        {
            if (searchParams.Fields == null)
            {
                var items = _mapper.Map<IEnumerable<ValueReadDto>>(entityCollection.Items);

                return new ApiCollection(items) { TotalCount = entityCollection.TotalCount };
            }
            else
            {
                var newItems = new List<object>();

                foreach (var item in entityCollection.Items)
                {
                    newItems.Add(_mapper.DynamicMap(item, searchParams.Fields));
                }

                var result = new ApiCollection(newItems);
                result.TotalCount = entityCollection.TotalCount;

                return result;
            }
        }
    }
}
