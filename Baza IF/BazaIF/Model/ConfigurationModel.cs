using BazaMvp.DataAccess;
using BazaMvp.DataAccess.Repos;
using BazaMvp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaIF.Model
{
    public class ConfigurationModel : IModel, INotifyPropertyChanged
    {
        private object _selectedCryteria;

        public bool IsVisa { get; set; }
        public IEnumerable<object> Cryteria { get; set; }
        public object SelectedCryteria
        {
            get
            {
                return _selectedCryteria;
            }
            set
            {
                _selectedCryteria = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedCryteria"));
            }
        }

        public ConfigurationModel()
        {
            FetchData();
        }

        public void FetchData()
        {
            using (var unitOfWork = UnitOfWork.Create())
            {
                var cryteria = unitOfWork.Repo<HelperRepo>().GetCryteria();

                if (IsVisa)
                    Cryteria = cryteria.OfType<CryteriaVisa>().ToList();
                else
                    Cryteria = cryteria.OfType<CryteriaMC>().ToList();

                if (Cryteria.Count() > 0)
                    SelectedCryteria = Cryteria.FirstOrDefault();
            }
        }

        public void UpdateCryteria(CryteriaBase cryteria)
        {
            using (var unitOfWork = UnitOfWork.Create())
            {
                unitOfWork.Repo<HelperRepo>().UpdateCryteria(cryteria);
                unitOfWork.Commit();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
