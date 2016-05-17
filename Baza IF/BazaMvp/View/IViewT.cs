using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.View
{
    public interface IView<TModel> : IView
    {
        TModel Model { get; set; }
    }
}
