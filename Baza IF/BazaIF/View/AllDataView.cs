using BazaIF.View.IView;
using BazaMvp.Utils;
using BazaMvp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BazaIF.View
{
    public class AllDataView : ViewMidBase, IAllDataView
    {
        #region Components

        private DataGridView dgvRecords;
        private MonthCalendar monthCalendar1;
        private MonthCalendar monthCalendar2;
        private GroupBox grpDate;
        private RadioButton rdbBusinessDate;
        private RadioButton rdbUploadDate;
        private Button btnRefresh;
        private Button btnLoad;
        private Button btnConfigure;
        private Button btnReport;
        private DataGridView dgvAllFiles;

        private void InitializeComponent()
        {
            this.dgvAllFiles = new System.Windows.Forms.DataGridView();
            this.dgvRecords = new System.Windows.Forms.DataGridView();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.monthCalendar2 = new System.Windows.Forms.MonthCalendar();
            this.grpDate = new System.Windows.Forms.GroupBox();
            this.rdbBusinessDate = new System.Windows.Forms.RadioButton();
            this.rdbUploadDate = new System.Windows.Forms.RadioButton();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnConfigure = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).BeginInit();
            this.grpDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvAllFiles
            // 
            this.dgvAllFiles.AllowUserToAddRows = false;
            this.dgvAllFiles.AllowUserToDeleteRows = false;
            this.dgvAllFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAllFiles.Location = new System.Drawing.Point(13, 18);
            this.dgvAllFiles.Name = "dgvAllFiles";
            this.dgvAllFiles.ReadOnly = true;
            this.dgvAllFiles.Size = new System.Drawing.Size(633, 156);
            this.dgvAllFiles.TabIndex = 1;
            this.dgvAllFiles.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAllFiles_CellClick);
            // 
            // dgvRecords
            // 
            this.dgvRecords.AllowUserToAddRows = false;
            this.dgvRecords.AllowUserToDeleteRows = false;
            this.dgvRecords.AllowUserToOrderColumns = true;
            this.dgvRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecords.Location = new System.Drawing.Point(13, 186);
            this.dgvRecords.Name = "dgvRecords";
            this.dgvRecords.ReadOnly = true;
            this.dgvRecords.Size = new System.Drawing.Size(1229, 150);
            this.dgvRecords.TabIndex = 2;
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(910, 12);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 3;
            // 
            // monthCalendar2
            // 
            this.monthCalendar2.Location = new System.Drawing.Point(1085, 12);
            this.monthCalendar2.Name = "monthCalendar2";
            this.monthCalendar2.TabIndex = 4;
            // 
            // grpDate
            // 
            this.grpDate.Controls.Add(this.rdbBusinessDate);
            this.grpDate.Controls.Add(this.rdbUploadDate);
            this.grpDate.Location = new System.Drawing.Point(733, 76);
            this.grpDate.Name = "grpDate";
            this.grpDate.Size = new System.Drawing.Size(165, 69);
            this.grpDate.TabIndex = 5;
            this.grpDate.TabStop = false;
            this.grpDate.Text = "Data";
            // 
            // rdbBusinessDate
            // 
            this.rdbBusinessDate.AutoSize = true;
            this.rdbBusinessDate.Location = new System.Drawing.Point(7, 43);
            this.rdbBusinessDate.Name = "rdbBusinessDate";
            this.rdbBusinessDate.Size = new System.Drawing.Size(101, 17);
            this.rdbBusinessDate.TabIndex = 1;
            this.rdbBusinessDate.Text = "Data biznesowa";
            this.rdbBusinessDate.UseVisualStyleBackColor = true;
            // 
            // rdbUploadDate
            // 
            this.rdbUploadDate.AutoSize = true;
            this.rdbUploadDate.Checked = true;
            this.rdbUploadDate.Location = new System.Drawing.Point(7, 20);
            this.rdbUploadDate.Name = "rdbUploadDate";
            this.rdbUploadDate.Size = new System.Drawing.Size(98, 17);
            this.rdbUploadDate.TabIndex = 0;
            this.rdbUploadDate.TabStop = true;
            this.rdbUploadDate.Text = "Data wczytania";
            this.rdbUploadDate.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(733, 151);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(165, 23);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Odswiez";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(652, 18);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 52);
            this.btnLoad.TabIndex = 7;
            this.btnLoad.Text = "Wczytaj plik";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnConfigure
            // 
            this.btnConfigure.Location = new System.Drawing.Point(733, 18);
            this.btnConfigure.Name = "btnConfigure";
            this.btnConfigure.Size = new System.Drawing.Size(75, 52);
            this.btnConfigure.TabIndex = 9;
            this.btnConfigure.Text = "Konfiguracja";
            this.btnConfigure.UseVisualStyleBackColor = true;
            this.btnConfigure.Click += new System.EventHandler(this.btnConfigure_Click);
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(814, 18);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(84, 52);
            this.btnReport.TabIndex = 10;
            this.btnReport.Text = "Wygeneruj raport";
            this.btnReport.UseVisualStyleBackColor = true;
            // 
            // AllDataView
            // 
            this.ClientSize = new System.Drawing.Size(1253, 359);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnConfigure);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.grpDate);
            this.Controls.Add(this.monthCalendar2);
            this.Controls.Add(this.monthCalendar1);
            this.Controls.Add(this.dgvRecords);
            this.Controls.Add(this.dgvAllFiles);
            this.Name = "AllDataView";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).EndInit();
            this.grpDate.ResumeLayout(false);
            this.grpDate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public DataGridView RecordsGV { get { return dgvRecords; } }
        public DataGridView AllFilesGV { get { return dgvAllFiles; } }
        public DateTime DateFrom { get { return monthCalendar1.SelectionStart; } }
        public DateTime DateTo { get { return monthCalendar2.SelectionStart; } }

        public event DataGridViewCellEventHandler CellClick;
        public event EventHandler<RefreshByDateEventArgs> Refresh_Click;
        public event EventHandler<OpenFormEventArgs> OpenForm;
        public event EventHandler LoadFile;

        public AllDataView()
        {
            InitializeComponent();
        }

        private void dgvAllFiles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CellClick != null)
                CellClick(sender, e);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (Refresh_Click != null)
                Refresh_Click(sender, new RefreshByDateEventArgs { DateType = "" });
        }

        private void btnConfigure_Click(object sender, EventArgs e)
        {
            OpenForm(this, new OpenFormEventArgs(typeof(ConfigurationView)));
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadFile(this, e);
        }
    }
}
