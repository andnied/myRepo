using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Mapper.Mappers
{
    public abstract class MapperBase
    {
        protected readonly WebApiMapper _mapper;

        protected MapperBase()
        {
            _mapper = WebApiMapper.GetMapper();
        }
    }
}
