using JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Common.Structures;
using WebAPI.Model.Dto.Read;
using WebAPI.Model.Dto.Update;
using WebAPI.Model.Dto.Write;
using WebAPI.Model.SearchParams;

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
