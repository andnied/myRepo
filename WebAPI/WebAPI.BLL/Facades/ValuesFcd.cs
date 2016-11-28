using JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Contracts.BLL;
using WebAPI.Contracts.DAL;
using WebAPI.DAL.Models;
using WebAPI.Model.Dto.Read;
using WebAPI.Model.Dto.Update;
using WebAPI.Model.SearchParams;

namespace WebAPI.BLL.Facades
{
    public class ValuesFcd : BaseFcd, IValuesFcd
    {
        private readonly IValuesRepository _repo;

        public ValuesFcd(IValuesRepository repo)
            : base()
        {
            _repo = repo;
        }

        public IEnumerable<ValueReadDto> GetAll(BaseSearchParams searchParams)
        {
            var items = _repo.Get(searchParams);

            return _mapper.Map<IEnumerable<ValueReadDto>>(items);
        }

        public ValueReadDto Get(int id)
        {
            var item = _repo.Get(id);

            return _mapper.Map<ValueReadDto>(item);
        }

        public ValueReadDto Update(int id, JsonPatchDocument<ValueUpdateDto> model)
        {
            var item = _repo.Get(id);
            var dto = _mapper.Map<ValueUpdateDto>(item);
            model.ApplyUpdatesTo(dto);
            item = _mapper.Map(dto, item);
            var updated = _repo.Update(id, item);

            return _mapper.Map<ValueReadDto>(updated);
        }
    }
}
