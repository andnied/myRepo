using JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Common.Structures;
using WebAPI.Common.Dto.Read;
using WebAPI.Common.Dto.Update;
using WebAPI.Common.Dto.Write;
using WebAPI.Common.SearchParams;

namespace WebAPI.Contracts.BLL
{
    public interface IValuesFcd
    {
        Task<ApiCollection> GetAll(BaseSearchParams searchParams);
        Task<ValueReadDto> Get(int id);
        Task<ValueReadDto> Update(int id, JsonPatchDocument<ValueUpdateDto> model);
        Task<ValueReadDto> Update(int id, ValueWriteDto model);
        Task<ValueReadDto> Create(ValueWriteDto model);
        Task Delete(int id);
    }
}
