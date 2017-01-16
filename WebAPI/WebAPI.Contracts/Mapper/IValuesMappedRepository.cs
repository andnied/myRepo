using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Common.Structures;
using WebAPI.Contracts.DAL;
using WebAPI.DAL.Models;
using WebAPI.Model.Dto.Read;

namespace WebAPI.Contracts.Mapper
{
    public interface IValuesMappedRepository : IRepositoryBase<ValueReadDto, ApiCollection<ValueReadDto>>
    {
    }
}
