using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{
    public interface IServiceFactory
    {
        T GetService<T>() where T : IServiceContract;
    }
}
