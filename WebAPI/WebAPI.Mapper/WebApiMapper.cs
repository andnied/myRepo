using AutoMapper;
using System;
using WebAPI.Common.Structures;
using WebAPI.DAL.Models;
using WebAPI.Model.Dto.Read;
using WebAPI.Model.Dto.Update;
using WebAPI.Model.Dto.Write;

namespace WebAPI.Mapper
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
            var action = new Action<IMapperConfigurationExpression>(c =>
            {
                c.CreateMap<Child, ChildReadDto>()
                    .ForMember(dto => dto.Id, s => s.MapFrom(model => model.Id))
                    .ForMember(dto => dto.ChildName, s => s.MapFrom(model => model.ChildName));

                c.CreateMap<Value, ValueReadDto>()
                    .ForMember(dto => dto.Id, s => s.MapFrom(model => model.Id))
                    .ForMember(dto => dto.Name, s => s.MapFrom(model => model.Name))
                    .ForMember(dto => dto.Description, s => s.MapFrom(model => model.Description))
                    .ForMember(dto => dto.Children, s => s.MapFrom(model => model.Children));

                c.CreateMap<Value, ValueUpdateDto>()
                    .ForMember(dto => dto.Name, s => s.MapFrom(model => model.Name));

                c.CreateMap<ValueUpdateDto, Value>()
                    .ForMember(dto => dto.Name, s => s.MapFrom(model => model.Name));

                c.CreateMap<ApiCollection<Value>, ApiCollection<ValueReadDto>>();

                c.CreateMap<ValueWriteDto, Value>();
            });

            var config = new MapperConfiguration(action);

            _mapper = config.CreateMapper();
            AutoMapper.Mapper.Initialize(action);
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
