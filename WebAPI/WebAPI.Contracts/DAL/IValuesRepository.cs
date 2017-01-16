using JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.DAL.Models;
using WebAPI.Model.Dto.Update;

namespace WebAPI.Contracts.DAL
{
    public interface IValuesRepository : IRepositoryBase<Value, IQueryable<Value>>
    {
    }
}
