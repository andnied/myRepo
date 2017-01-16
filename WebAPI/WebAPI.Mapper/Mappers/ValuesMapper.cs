using AutoMapper.QueryableExtensions;
using System;
using System.Linq;
using WebAPI.Common.Structures;
using WebAPI.Contracts.DAL;
using WebAPI.Contracts.Mapper;
using WebAPI.Model.Dto.Read;
using WebAPI.Model.SearchParams;

namespace WebAPI.Mapper.Mappers
{
    public class ValuesMapper : MapperBase, IValuesMappedRepository
    {
        private readonly IValuesRepository _repo;

        public ValuesMapper(IValuesRepository repo)
        {
            _repo = repo;
        }

        public ValueReadDto Add(ValueReadDto entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ValueReadDto Get(int id)
        {
            var item = _repo.Get(id);
            return _mapper.Map<ValueReadDto>(item);
        }

        public ApiCollection<ValueReadDto> Get(BaseSearchParams searchParams)
        {
            var queried = _repo.Get(searchParams);
            var items = queried.ProjectTo<ValueReadDto>().ToList();
            var count = items.Count;

            return new ApiCollection<ValueReadDto>(items) { TotalCount = count };
        }

        public ValueReadDto Update(int id, ValueReadDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
