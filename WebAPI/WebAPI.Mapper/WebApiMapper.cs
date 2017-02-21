using AutoMapper;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using WebAPI.Common.Structures;
using WebAPI.Model.Models;
using WebAPI.Common.Dto.Read;
using WebAPI.Common.Dto.Update;
using WebAPI.Common.Dto.Write;
using WebAPI.Model.Models.Identity;

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

                c.CreateMap<ValueWriteDto, Value>();

                c.CreateMap<ApplicationUserWriteDto, ApplicationUser>();
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

        public object DynamicMap<TSource>(TSource source, string fields)
            where TSource : class
        {
            var result = new ExpandoObject();
            var fieldCollection = fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var field in fieldCollection)
            {
                var value = source.GetType()
                    .GetProperty(field.Trim(), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                    .GetValue(source);

                (result as IDictionary<string, object>).Add(field, value);
            }

            return result;
        }
    }
}
