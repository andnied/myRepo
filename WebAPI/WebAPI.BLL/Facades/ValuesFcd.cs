using JsonPatch;
using System.Collections.Generic;
using WebAPI.Common.Structures;
using WebAPI.Contracts.BLL;
using WebAPI.Contracts.DAL;
using WebAPI.Model.Dto.Read;
using WebAPI.Model.Dto.Update;
using WebAPI.Model.SearchParams;
using System;
using WebAPI.Mapper;
using System.Threading.Tasks;

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

        public async Task<ApiCollection<ValueReadDto>> GetAll(BaseSearchParams searchParams)
        {
            var items = _repo.Get(searchParams);

            return await items;
        }

        public async Task<ValueReadDto> Get(int id)
        {
            var item = _repo.Get(id);

            return await item;
        }

        public async Task<ValueReadDto> Update(int id, JsonPatchDocument<ValueUpdateDto> model)
        {
            var item = await _repo.Get(id);
            var dto = _mapper.Map<ValueUpdateDto>(item);
            model.ApplyUpdatesTo(dto);
            item = _mapper.Map(dto, item);
            var updated = _repo.Update(id, item);

            return await updated;
        }

        public async Task<ValueReadDto> Update(int id, ValueReadDto model)
        {
            throw new NotImplementedException();
        }
    }
}
