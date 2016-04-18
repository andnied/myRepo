using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MvpDemo.Presenter;

namespace MvpDemo.View
{
    public partial class MainForm : Form, IEmployeesView
    {
        public event EventHandler LoadView;
        private EmployeesPresenter _presenter;

        public MainForm()
        {
            InitializeComponent();
            _presenter = new EmployeesPresenter(this);            
        }

        public void BindData()
        {
            dataGridView1.DataSource = _presenter.LoadEmployees();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (LoadView != null)
                LoadView(this, new EventArgs());
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
