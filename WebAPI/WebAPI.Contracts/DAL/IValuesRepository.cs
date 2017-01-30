using JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.DAL.Models;

namespace WebAPI.Contracts.DAL
{
    public interface IValuesRepository : IRepositoryBase<Value>
    {
    }
}
