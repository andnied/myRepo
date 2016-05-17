using BazaMvp.Model;
using BazaMvp.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BazaMvp.View
{
    public class ViewMidBase : ViewBase<IModel>
    {
        protected ViewMidBase()
        {
            try
            {
                var type = Assembly.GetCallingAssembly().GetTypes().FirstOrDefault(x => x.Name.Equals(this.GetType().Name.Replace("View", "Presenter")));

                _presenter = (IPresenter)Activator.CreateInstance(type, new object[] { this });
            }
            catch { }
        }
    }
}
