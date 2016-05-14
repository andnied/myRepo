﻿
using System;
using System.Windows.Forms;
using WinFormsMvp;
using WinFormsMvp.Binder;

namespace LicenceTracker.Views
{
    public partial class AddPersonView : AddPersonViewSlice, IAddPersonView
    {
        public AddPersonView()
        {
            InitializeComponent();            
        }

        private void CloseFormButton_Click(object sender, System.EventArgs e)
        {
            CloseFormClicked(this, EventArgs.Empty);
        }


        public event System.EventHandler CloseFormClicked;

        public event System.EventHandler AddPersonClicked;

        public void Exit(IPresenter presenter)
        {
            Close();
            PresenterBinder.Factory.Release(presenter);
        }

        private void AddPersonButton_Click(object sender, EventArgs e)
        {
            Model.NewPerson.FirstName = FirstNameTextBox.Text.Trim();
            Model.NewPerson.LastName = LastNameTextBox.Text.Trim();

            AddPersonClicked(this, EventArgs.Empty);

            MessageBox.Show("The new person has been added successfully.");
        }
    }
}
