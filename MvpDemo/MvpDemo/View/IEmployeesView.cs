using Model;
using MvpDemo.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvpDemo.View
{
    public interface IEmployeesView
    {
        event EventHandler LoadView;
        void BindData();
    }
}
