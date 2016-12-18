using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.DAL.Models;
using WebAPI.Model.Dto.Read;
using WebAPI.Model.Dto.Update;

namespace WebAPI.Model.Mapper
{
    public sealed class WebApiMapper
    {
        private static WebApiMapper Instance;
        private readonly IMapper _mapper;

        public static WebApiMapper GetMapper()
        {
            return Instance = (Instance ?? new WebApiMapper());
        }

        private WebApiMapper()
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<Value, ValueReadDto>()
                    .ForMember(dto => dto.Id, s => s.MapFrom(model => model.Id))
                    .ForMember(dto => dto.Name, s => s.MapFrom(model => model.Name))
                    .ForMember(dto => dto.Description, s => s.MapFrom(model => model.Description));

                c.CreateMap<Value, ValueUpdateDto>()
                    .ForMember(dto => dto.Name, s => s.MapFrom(model => model.Name));

                c.CreateMap<ValueUpdateDto, Value>()
                    .ForMember(dto => dto.Name, s => s.MapFrom(model => model.Name));
            });

            _mapper = config.CreateMapper();
        }

        public TDest Map<TDest>(object source) 
            where TDest : class
        {
            return _mapper.Map<TDest>(source);
        }

        public TDest Map<TSource, TDest>(TSource source, TDest dest) 
            where TSource : class 
            where TDest : class
        {
            return _mapper.Map(source, dest);
        }
    }
}
