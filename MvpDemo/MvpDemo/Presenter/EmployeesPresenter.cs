using Model;
using MvpDemo.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvpDemo.Presenter
{
    public class EmployeesPresenter
    {
        private readonly IEmployeesView _view;
        private IModel<Employees> _employeesModel;

        public EmployeesPresenter(IEmployeesView view)
        {
            _view = view;
            _employeesModel = new EmployeesVM();
            InitEvents();
        }

        public IEnumerable<Employees> LoadEmployees()
        {
            return _employeesModel.LoadAll();
        }
        
        private void InitEvents()
        {
            _view.LoadView += _view_Load;
        }

        private void _view_Load(object sender, EventArgs e)
        {
            _view.BindData();
        }
    }
}
