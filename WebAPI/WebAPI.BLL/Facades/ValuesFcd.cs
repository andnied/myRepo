using JsonPatch;
using System.Collections.Generic;
using WebAPI.Common.Structures;
using WebAPI.Contracts.BLL;
using WebAPI.Contracts.DAL;
using WebAPI.Model.Dto.Read;
using WebAPI.Model.Dto.Update;
using WebAPI.Model.SearchParams;
using System;
using System.Threading.Tasks;
using WebAPI.Model.Dto.Write;
using WebAPI.DAL.Models;

namespace WebAPI.BLL.Facades
{
    public class ValuesFcd : BaseFcd, IValuesFcd
    {
        private readonly IValuesRepository _repo;
        private BaseSearchParams _searchParams;

        public ValuesFcd(IValuesRepository repo)
            : base()
        {
            _repo = repo;
        }

        public async Task<object> GetAll(BaseSearchParams searchParams)
        {
            _searchParams = searchParams;
            var items = await _repo.Get(searchParams);
            var result = GetDtoCollection(items);
            
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

        private ApiCollection<object> GetDtoCollection(ApiCollection<Value> entityCollection)
        {
            if (_searchParams.Fields == null)
            {
                var items = _mapper.Map<IEnumerable<ValueReadDto>>(entityCollection.Items);

                return new ApiCollection<object>(items) { TotalCount = entityCollection.TotalCount };
            }
            else
            {
                var newItems = new List<object>();

                foreach (var item in entityCollection.Items)
                {
                    newItems.Add(_mapper.DynamicMap(item, _searchParams.Fields));
                }

                var result = new ApiCollection<object>(newItems);
                result.TotalCount = entityCollection.TotalCount;

                return result;
            }            
        }
    }
}
