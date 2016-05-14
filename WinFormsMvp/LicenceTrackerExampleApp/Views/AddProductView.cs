﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WinFormsMvp;
using WinFormsMvp.Binder;
using WinFormsMvp.Forms;

namespace LicenceTracker.Views
{
    public partial class AddProductView : MvpForm, IAddProductView
    {
        public AddProductView()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SoftwareTypesComboBox.DataSource = new BindingSource(SoftwareTypes, null);
        }

        public event EventHandler CloseFormClicked;

        public event EventHandler AddProductClicked;



        private void AddProductButton_Click(object sender, EventArgs e)
        {            
            Description = DescriptionTextBox.Text.Trim();            
            Name = NameTextBox.Text.Trim();
            TypeId = (int)SoftwareTypesComboBox.SelectedValue;
            

            AddProductClicked(this, EventArgs.Empty);

            MessageBox.Show("The new product has been added successfully.");
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public Dictionary<int, string> SoftwareTypes { get; set; }

        public void Exit(IPresenter presenter)
        {
            Close();
            PresenterBinder.Factory.Release(presenter);
        }

        public int TypeId { get; set; }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            CloseFormClicked(this, EventArgs.Empty);
        }        
    }
}
