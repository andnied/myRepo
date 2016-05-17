using BazaMvp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.Presenter
{
    public abstract class PresenterBase : IPresenter
    {
        private readonly IView _view;

        public PresenterBase(IView view)
        {
            _view = view;
        }

        public IView View { get { return _view; } }
    }
}
