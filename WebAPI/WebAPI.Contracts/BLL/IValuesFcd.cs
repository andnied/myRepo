using JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Common.Structures;
using WebAPI.Model.Dto.Read;
using WebAPI.Model.Dto.Update;
using WebAPI.Model.SearchParams;

namespace WebAPI.Contracts.BLL
{
    public interface IValuesFcd
    {
        ApiCollection<ValueReadDto> GetAll(BaseSearchParams searchParams);
        ValueReadDto Get(int id);
        ValueReadDto Update(int id, JsonPatchDocument<ValueUpdateDto> model);
    }
}
