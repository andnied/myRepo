using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{
    public interface IDataRepositoryFactory
    {
        T GetRepo<T>() where T : IDataRepository;
    }
}
