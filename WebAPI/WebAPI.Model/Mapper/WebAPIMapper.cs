using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.DAL.Models;
using WebAPI.Model.Dto.Read;

namespace WebAPI.Model.Mapper
{
    public sealed class WebAPIMapper
    {
        private static WebAPIMapper Instance;
        private readonly IMapper _mapper;

        public static WebAPIMapper GetMapper()
        {
            return Instance = (Instance ?? new WebAPIMapper());
        }

        private WebAPIMapper()
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<Value, ValueReadDto>()
                    .ForMember(dto => dto.Id, s => s.MapFrom(model => model.Id))
                    .ForMember(dto => dto.Name, s => s.MapFrom(model => model.Name));
            });

            _mapper = config.CreateMapper();
        }

        public TDest Map<TDest>(object source) where TDest : class
        {
            return _mapper.Map<TDest>(source);
        }
    }
}
