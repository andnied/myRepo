using BazaMvp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.Presenter
{
    public interface IPresenter
    {
    }

    public interface IPresenter<T> : IPresenter
    {
        T View { get; }
    }
}
