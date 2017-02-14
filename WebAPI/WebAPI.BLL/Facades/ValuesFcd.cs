using JsonPatch;
using System.Collections.Generic;
using WebAPI.Common.Structures;
using WebAPI.Contracts.BLL;
using WebAPI.Contracts.DAL;
using WebAPI.Common.Dto.Read;
using WebAPI.Common.Dto.Update;
using WebAPI.Common.SearchParams;
using System;
using System.Threading.Tasks;
using WebAPI.Common.Dto.Write;
using WebAPI.Model.Models;
using WebAPI.Mapper;
using WebAPI.Common.Utils;

namespace WebAPI.BLL.Facades
{
    public class ValuesFcd : BaseFcd, IValuesFcd
    {
        private readonly IValuesRepository _repo;
        private readonly IValuesService _service;

        public ValuesFcd(IValuesRepository repo, IValuesService service, WebApiMapper mapper)
            : base(mapper)
        {
            _repo = repo;
            _service = service;
        }

        public async Task<ApiCollection> GetAll(BaseSearchParams searchParams)
        {
            if (searchParams.Fields != null && !(Helper.AreFieldsValid<Value>(searchParams.Fields)))
            {
                searchParams.Fields = null;
            }

            var items = await _repo.Get(searchParams);
            var result = _service.GetDtoCollection(items, searchParams);
            
            return result;
        }

        public async Task<ValueReadDto> Get(int id)
        {
            var item = await _repo.Get(id);
            var dto = _mapper.Map<ValueReadDto>(item);

            return dto;
        }

        public async Task<ValueReadDto> Create(ValueWriteDto model)
        {
            var entity = _mapper.Map<Value>(model);
            var added = await _repo.Add(entity);
            var addedDto = _mapper.Map<ValueReadDto>(added);

            return addedDto;
        }

        public async Task<ValueReadDto> Update(int id, ValueWriteDto model)
        {
            var mapped = _mapper.Map<Value>(model);
            mapped.Id = id;
            var updated = await _repo.Update(id, mapped);
            var updatedDto = _mapper.Map<ValueReadDto>(updated);

            return updatedDto;
        }

        public async Task<ValueReadDto> Update(int id, JsonPatchDocument<ValueUpdateDto> model)
        {
            var item = await _repo.Get(id);
            var dto = _mapper.Map<ValueUpdateDto>(item);

            model.ApplyUpdatesTo(dto);
            item = _mapper.Map(dto, item);

            var updated = await _repo.Update(id, item);
            var updatedDto = _mapper.Map<ValueReadDto>(updated);

            return updatedDto;
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }
    }
}
