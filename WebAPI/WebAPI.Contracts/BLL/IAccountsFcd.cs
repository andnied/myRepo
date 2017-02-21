using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Common.Dto.Write;

namespace WebAPI.Contracts.BLL
{
    public interface IAccountsFcd
    {
        Task<string> Create(ApplicationUserWriteDto model);
    }
}
