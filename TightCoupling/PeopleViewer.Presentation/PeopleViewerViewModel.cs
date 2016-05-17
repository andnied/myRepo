using PeopleViewer.SharedObjects;
using PersonRepository.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace PeopleViewer.Presentation
{
    public class PeopleViewerViewModel : INotifyPropertyChanged
    {
        protected IPersonRepository Repository;

        private IEnumerable<Person> _people;
        public IEnumerable<Person> People
        {
            get { return _people; }
            set
            {
                if (_people == value)
                    return;
                _people = value;
                RaisePropertyChanged("People");
            }
        }

        public PeopleViewerViewModel(IPersonRepository repository)
        {
            Repository = repository;
        }

        #region Commands

        #region RefreshCommand Standard Stuff

        private RefreshCommand _refreshPeopleCommand = new RefreshCommand();
        public RefreshCommand RefreshPeopleCommand
        {
            get
            {
                if (_refreshPeopleCommand.ViewModel == null)
                    _refreshPeopleCommand.ViewModel = this;
                return _refreshPeopleCommand;
            }
        }

        public class RefreshCommand : ICommand
        {
            public PeopleViewerViewModel ViewModel { get; set; }
            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return true;
            }
        #endregion RefreshCommand Standard Stuff

            public void Execute(object parameter)
            {
                ViewModel.People = ViewModel.Repository.GetPeople();
            }
        }

        #region ClearCommand Standard Stuff

        private ClearCommand _clearPeopleCommand = new ClearCommand();
        public ClearCommand ClearPeopleCommand
        {
            get
            {
                if (_clearPeopleCommand.ViewModel == null)
                    _clearPeopleCommand.ViewModel = this;
                return _clearPeopleCommand;
            }
        }

        public class ClearCommand : ICommand
        {
            public PeopleViewerViewModel ViewModel { get; set; }
            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return true;
            }

        #endregion

            public void Execute(object parameter)
            {
                ViewModel.People = new List<Person>();
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
