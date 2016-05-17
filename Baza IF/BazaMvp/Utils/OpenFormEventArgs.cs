using BazaMvp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.Utils
{
    public class OpenFormEventArgs : EventArgs
    {
        public Type FormToOpen { get; set; }

        public OpenFormEventArgs(Type form)
        {
            FormToOpen = form;
        }
    }

    public class RefreshByDateEventArgs : EventArgs
    {
        public string DateType { get; set; }
    }
}
