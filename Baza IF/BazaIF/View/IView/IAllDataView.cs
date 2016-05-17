using BazaMvp.Model;
using BazaMvp.Utils;
using BazaMvp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BazaIF.View.IView
{
    public interface IAllDataView : BazaMvp.View.IView
    {
        DataGridView RecordsGV { get; }
        DataGridView AllFilesGV { get; }
        DateTime DateFrom { get; }
        DateTime DateTo { get; }

        event EventHandler Load;
        event FormClosingEventHandler FormClosing;
        event DataGridViewCellEventHandler CellClick;
        event EventHandler<RefreshByDateEventArgs> Refresh_Click;
        event EventHandler<OpenFormEventArgs> OpenForm;
        event EventHandler LoadFile;
    }
}
