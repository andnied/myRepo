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

namespace WebAPI.BLL.Facades
{
    public class ValuesFcd : BaseFcd, IValuesFcd
    {
        private readonly IValuesRepository _repo;
        private readonly WebApiMapper _mapper = WebApiMapper.GetMapper();

        public ValuesFcd(IValuesRepository repo)
            : base()
        {
            _repo = repo;
        }

        public ApiCollection<ValueReadDto> GetAll(BaseSearchParams searchParams)
        {
            var items = _repo.Get(searchParams);

            return items;
        }

        public ValueReadDto Get(int id)
        {
            var item = _repo.Get(id);

            return item;
        }

        public ValueReadDto Update(int id, JsonPatchDocument<ValueUpdateDto> model)
        {
            var item = _repo.Get(id);
            var dto = _mapper.Map<ValueUpdateDto>(item);
            model.ApplyUpdatesTo(dto);
            item = _mapper.Map(dto, item);
            var updated = _repo.Update(id, item);

            return updated;
        }

        public ValueReadDto Update(int id, ValueReadDto model)
        {
            var updated = _repo.Update(id, model);

            return updated;
        }
    }
}
