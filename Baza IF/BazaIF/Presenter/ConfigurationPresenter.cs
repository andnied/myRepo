using BazaIF.Model;
using BazaIF.View;
using BazaIF.View.IView;
using BazaMvp.Model;
using BazaMvp.Presenter;
using BazaMvp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BazaIF.Presenter
{
    public class ConfigurationPresenter : PresenterBase
    {
        private readonly IConfigurationView _view;
        private readonly ConfigurationModel _model;

        public ConfigurationPresenter(IConfigurationView view)
            : base(view)
        {
            _view = view;
            _model = new ConfigurationModel();
            _model.PropertyChanged += _model_PropertyChanged;

            view.Load += view_Load;
            view.CheckedChanged += view_CheckedChanged;
            view.CellClick += view_CellClick;
            view.UpdateClick += view_UpdateClick;
        }

        void view_Load(object sender, EventArgs e)
        {
            var dgv = _view.DgvCryteria;
            dgv.DataSource = _model.Cryteria;

            if (dgv.Rows.Count > 0)
                view_CellClick(_view.DgvCryteria, new DataGridViewCellEventArgs(0, 0));
        }

        void view_CheckedChanged(object sender, EventArgs e)
        {
            _model.IsVisa = _view.IsVisa;
            _model.FetchData();
            view_Load(null, null);
        }

        void view_CellClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            _model.SelectedCryteria = ((DataGridView)sender).CurrentRow.DataBoundItem;
        }

        void _model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                var selectedItem = (CryteriaMC)_model.GetType().GetProperty(e.PropertyName).GetValue(sender);

                foreach (var field in _view.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(ConfigurationViewAttribute), false).Any()))
                {
                    var propertyName = (field.GetCustomAttributes(typeof(ConfigurationViewAttribute), false).FirstOrDefault() as ConfigurationViewAttribute).Name;
                    var value = selectedItem.GetType().GetProperty(propertyName).GetValue(selectedItem);

                    ((Form)_view).Invoke(new Action(() =>
                    {
                        ((TextBox)field.GetValue(_view)).Text = value.ToString();
                    }));
                }
            });
        }

        void view_UpdateClick(object sender, EventArgs e)
        {
            var selectedItem = (CryteriaMC)_model.SelectedCryteria;

            foreach (var field in _view.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(ConfigurationViewAttribute), false).Any()))
            {
                var attribute = field.GetCustomAttributes(typeof(ConfigurationViewAttribute), false).FirstOrDefault() as ConfigurationViewAttribute;
                var value = ((TextBox)field.GetValue(_view)).Text;
                selectedItem.GetType().GetProperty(attribute.Name).SetValue(selectedItem, Convert.ChangeType(value, attribute.PropertyType));
            }

            _model.UpdateCryteria(selectedItem);
            _view.DgvCryteria.Refresh();
        }
    }
}
