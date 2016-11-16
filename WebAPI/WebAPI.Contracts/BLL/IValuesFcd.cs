using JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model.Dto.Read;
using WebAPI.Model.Dto.Update;

namespace WebAPI.Contracts.BLL
{
    public interface IValuesFcd
    {
        IEnumerable<ValueReadDto> GetAll(string sort);
        ValueReadDto Update(int id, JsonPatchDocument<ValueUpdateDto> model);
    }
}
