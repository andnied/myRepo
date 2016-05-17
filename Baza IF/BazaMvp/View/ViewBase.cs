using BazaMvp.Presenter;
using BazaMvp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace BazaMvp.View
{
    public abstract class ViewBase<TModel> : Form, IView<TModel> where TModel : IModel
    {
        protected IPresenter _presenter;

        public TModel Model { get; set; }
        
        public void DisplayView(Type type)
        {
            (Activator.CreateInstance(type) as Form).ShowDialog();
        }
    }
}
