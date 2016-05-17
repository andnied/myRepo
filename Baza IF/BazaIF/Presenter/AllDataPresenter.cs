using BazaIF.Model;
using BazaIF.View;
using BazaIF.View.IView;
using BazaMvp;
using BazaMvp.Presenter;
using BazaMvp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BazaMvp.DataAccess;
using BazaMvp.DataAccess.Repos;
using System.Windows.Forms;
using BazaMvp.Model;
using System.Collections;
using BazaMvp.Utils;
using BazaIF.Logic;

namespace BazaIF.Presenter
{
    public class AllDataPresenter : PresenterBase
    {
        private readonly IAllDataView _view;
        private AllDataModel _model;

        #region Public

        public AllDataPresenter(IAllDataView view)
            : base(view)
        {
            _view = view;
            _model = new AllDataModel();

            LoadAllFilesAsync(DateTime.Today.AddDays(-100), DateTime.Now);
            
            _view.Load += view_Load;
            _view.FormClosing += view_FormClosing;
            _view.CellClick += _view_CellClick;
            _view.Refresh_Click += _view_Refresh_Click;
            _view.LoadFile += _view_LoadFile;
            _view.OpenForm += _view_OpenForm;
        }

        void _view_OpenForm(object sender, OpenFormEventArgs e)
        {
            var type = e.FormToOpen;
            View.DisplayView(type);
        }

        void _view_LoadFile(object sender, EventArgs e)
        {
            try
            {
                string filePath = null;

                using (var dialog = new OpenFileDialog())
                {
                    dialog.Filter = "Text files|*.txt";

                    if (dialog.ShowDialog() == DialogResult.OK)
                        filePath = dialog.FileName;
                    else
                        return;
                }

                using (var parserBase = ParserFactory.GetParser(filePath))
                {
                    var parser = (IParser)parserBase;
                    var records = new List<InputBase>();

                    foreach (var msg in parser.GetMessages())
                    {
                        var header = parser.RetrieveHeader(msg);
                        var parentRecord = parser.InitializeParent(header);
                        records.AddRange(parser.GenerateRecords(parentRecord, msg));
                    }

                    if (records == null)
                    {
                        MessageBox.Show("Error");
                        return;
                    }

                    if (records.Count() == 0)
                    {
                        MessageBox.Show("Nie znaleziono rekordow do importu.");
                        return;
                    }

                    parser.AddRecords(records);
                    MessageBox.Show("Plik zostal wczytany poprawnie.");
                    _view_Refresh_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                //TODO: log
                MessageBox.Show(ex.Message, "Blad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void view_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            
        }

        void view_Load(object sender, EventArgs e)
        {

        }

        void _view_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var gv = (DataGridView)sender;
            var id = Convert.ToInt64(gv.CurrentRow.Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.OwningColumn.HeaderText == "Id").Value);

            if (gv.Columns["colDelete"].Index == e.ColumnIndex)
            {
                if (MessageBox.Show("Czy na pewno chcesz skasowac plik i rekordy?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DeleteFile(id);
                    _view_Refresh_Click(null, null);
                }
            }
            else
                LoadRecords(id);
        }

        void _view_Refresh_Click(object sender, RefreshByDateEventArgs e)
        {
            LoadAllFilesAsync(_view.DateFrom, _view.DateTo.AddDays(1));
        }

        #endregion

        #region Private

        private async void LoadAllFilesAsync(DateTime from, DateTime to)
        {
            try
            {
                if (_model.FetchingTask != null && _model.FetchingTask.Status == TaskStatus.Running)
                    await _model.FetchingTask;

                _model.FetchData();
                await _model.FetchingTask;

                ((System.Windows.Forms.Form)_view).Invoke(new Action(() =>
                {
                    _view.AllFilesGV.DataSource = _model.AllFiles
                        .Select(f => new { f.Id, f.FileName, f.UploadDate, f.UserName })
                        .Where(f => f.UploadDate >= from && f.UploadDate <= to).ToList();
                    if (_view.AllFilesGV.Columns["colDelete"] == null)
                        _view.AllFilesGV.Columns.Add(new DataGridViewButtonColumn()
                        {
                            Name = "colDelete",
                            Text = "usun",
                            HeaderText = string.Empty,
                            UseColumnTextForButtonValue = true
                        });
                }));
            }
            catch { }
        }

        private void LoadRecords(long id)
        {
            var file = _model.AllFiles.FirstOrDefault(f => f.Id == id);
            IEnumerable<InputBase> res = null;
            var propertyWithRecords = file.GetType().GetProperties()
                .Where(p => !(p.GetGetMethod().IsVirtual) && p.PropertyType.GetInterface("IEnumerable") != null && p.PropertyType.GetInterfaces().Count() == 1)
                .FirstOrDefault(p => (res = (p.GetValue(file) as IEnumerable).Cast<InputBase>()).Any());
            
            ((System.Windows.Forms.Form)_view).Invoke(new Action(() =>
            {
                var baseColumnsToBeDisplayed = new string[] { "BusinessDate", "Fid" };

                _view.RecordsGV.DataSource = res.CastDynamically(propertyWithRecords.PropertyType.GetGenericArguments()[0]).ToList();

                typeof(InputBase).GetProperties().ToList().ForEach(p =>
                    {
                        if (_view.RecordsGV.Columns[p.Name] != null && !(baseColumnsToBeDisplayed.Contains(p.Name)))
                            _view.RecordsGV.Columns[p.Name].Visible = false;
                    }
                );
            }));
        }

        private void DeleteFile(long id)
        {
            var msg = string.Empty;

            if (_model.DeleteFile(id))
                msg = "Plik zostal skasowany.";
            else
                msg = "Nie mozna usunac pliku. Szczegoly bledu mozna znalezc w logach.";

            _view.RecordsGV.DataSource = null;
            MessageBox.Show(msg);
        }

        #endregion
    }
}
