using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model.Mapper;

namespace WebAPI.BLL
{
    public abstract class BaseFcd
    {
        protected readonly WebAPIMapper _mapper;

        protected BaseFcd()
        {
            _mapper = WebAPIMapper.GetMapper();
        }
    }
}
