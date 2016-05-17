using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BazaIF.View.IView
{
    public interface IConfigurationView : BazaMvp.View.IView
    {
        DataGridView DgvCryteria { get; }
        bool IsVisa { get; }
        TextBox TxtName { get; }
        TextBox TxtIrd { get; }
        TextBox TxtProduct { get; }
        TextBox TxtRegion { get; }
        TextBox TxtDescription { get; }
        TextBox TxtFee { get; }
        TextBox TxtDeviationIF { get; }
        TextBox TxtDeviationIFMin { get; }
        TextBox TxtIFNumber { get; }
        TextBox TxtIFBase { get; }
        TextBox TxtDeviationReg1 { get; }
        TextBox TxtDeviationReg2 { get; }
        TextBox TxtDeviationReg3 { get; }
        CheckBox IsActive { get; }

        event EventHandler Load;
        event EventHandler CheckedChanged;
        event DataGridViewCellEventHandler CellClick;
        event EventHandler UpdateClick;
    }
}
