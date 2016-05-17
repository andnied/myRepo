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
    public class ConfigurationView : ViewMidBase, IConfigurationView
    {
        #region Fields

        private System.Windows.Forms.DataGridView dgvCriteria;
        private System.Windows.Forms.RadioButton rdbMC;
        private System.Windows.Forms.RadioButton rdbVisa;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIrd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRegion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFee;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDeviationIF;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDeviationIFMin;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtDeviationReg2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtDeviationReg1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtIFBase;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtIFNumber;
        private System.ComponentModel.IContainer components;
        private CheckBox chkIsActive;
        private Button btnUpdate;
        private Label label9;
        private TextBox txtDeviationReg3;
        private Button btnDelete;
        private Button btnNew;
        private System.Windows.Forms.GroupBox grpOrganization;
    
        private void InitializeComponent()
        {
            this.dgvCriteria = new System.Windows.Forms.DataGridView();
            this.rdbMC = new System.Windows.Forms.RadioButton();
            this.rdbVisa = new System.Windows.Forms.RadioButton();
            this.grpOrganization = new System.Windows.Forms.GroupBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIrd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRegion = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFee = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDeviationIF = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDeviationIFMin = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtDeviationReg2 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtDeviationReg1 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtIFBase = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtIFNumber = new System.Windows.Forms.TextBox();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDeviationReg3 = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCriteria)).BeginInit();
            this.grpOrganization.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvCriteria
            // 
            this.dgvCriteria.AllowUserToAddRows = false;
            this.dgvCriteria.AllowUserToDeleteRows = false;
            this.dgvCriteria.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCriteria.Location = new System.Drawing.Point(12, 220);
            this.dgvCriteria.Name = "dgvCriteria";
            this.dgvCriteria.ReadOnly = true;
            this.dgvCriteria.Size = new System.Drawing.Size(1103, 174);
            this.dgvCriteria.TabIndex = 0;
            this.dgvCriteria.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCriteria_CellClick);
            // 
            // rdbMC
            // 
            this.rdbMC.AutoSize = true;
            this.rdbMC.Checked = true;
            this.rdbMC.Location = new System.Drawing.Point(6, 19);
            this.rdbMC.Name = "rdbMC";
            this.rdbMC.Size = new System.Drawing.Size(41, 17);
            this.rdbMC.TabIndex = 1;
            this.rdbMC.TabStop = true;
            this.rdbMC.Text = "MC";
            this.rdbMC.UseVisualStyleBackColor = true;
            // 
            // rdbVisa
            // 
            this.rdbVisa.AutoSize = true;
            this.rdbVisa.Location = new System.Drawing.Point(53, 19);
            this.rdbVisa.Name = "rdbVisa";
            this.rdbVisa.Size = new System.Drawing.Size(49, 17);
            this.rdbVisa.TabIndex = 2;
            this.rdbVisa.Text = "VISA";
            this.rdbVisa.UseVisualStyleBackColor = true;
            this.rdbVisa.CheckedChanged += new System.EventHandler(this.rdbVisa_CheckedChanged);
            // 
            // grpOrganization
            // 
            this.grpOrganization.Controls.Add(this.rdbMC);
            this.grpOrganization.Controls.Add(this.rdbVisa);
            this.grpOrganization.Location = new System.Drawing.Point(12, 12);
            this.grpOrganization.Name = "grpOrganization";
            this.grpOrganization.Size = new System.Drawing.Size(105, 42);
            this.grpOrganization.TabIndex = 3;
            this.grpOrganization.TabStop = false;
            this.grpOrganization.Text = "Organizacja";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(185, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(139, 20);
            this.txtName.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(139, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Nazwa";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(153, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "IRD";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtIrd
            // 
            this.txtIrd.Location = new System.Drawing.Point(185, 38);
            this.txtIrd.Name = "txtIrd";
            this.txtIrd.Size = new System.Drawing.Size(139, 20);
            this.txtIrd.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(135, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Produkt";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(185, 64);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(139, 20);
            this.txtProduct.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(138, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Region";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtRegion
            // 
            this.txtRegion.Location = new System.Drawing.Point(185, 90);
            this.txtRegion.Name = "txtRegion";
            this.txtRegion.Size = new System.Drawing.Size(139, 20);
            this.txtRegion.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(151, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Opis";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(185, 116);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(139, 20);
            this.txtDescription.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(414, 142);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Oplata";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtFee
            // 
            this.txtFee.Location = new System.Drawing.Point(458, 142);
            this.txtFee.Name = "txtFee";
            this.txtFee.Size = new System.Drawing.Size(73, 20);
            this.txtFee.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(352, 168);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Odchylenie IF (max)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtDeviationIF
            // 
            this.txtDeviationIF.Location = new System.Drawing.Point(458, 168);
            this.txtDeviationIF.Name = "txtDeviationIF";
            this.txtDeviationIF.Size = new System.Drawing.Size(73, 20);
            this.txtDeviationIF.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(368, 194);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Wartosc IF (min)";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtDeviationIFMin
            // 
            this.txtDeviationIFMin.Location = new System.Drawing.Point(458, 194);
            this.txtDeviationIFMin.Name = "txtDeviationIFMin";
            this.txtDeviationIFMin.Size = new System.Drawing.Size(73, 20);
            this.txtDeviationIFMin.TabIndex = 18;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(335, 91);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(117, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "Odchylenie - Regiony 2";
            // 
            // txtDeviationReg2
            // 
            this.txtDeviationReg2.Location = new System.Drawing.Point(458, 88);
            this.txtDeviationReg2.Name = "txtDeviationReg2";
            this.txtDeviationReg2.Size = new System.Drawing.Size(73, 20);
            this.txtDeviationReg2.TabIndex = 26;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(344, 65);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(108, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "Odchylenie - Regiony";
            // 
            // txtDeviationReg1
            // 
            this.txtDeviationReg1.Location = new System.Drawing.Point(458, 62);
            this.txtDeviationReg1.Name = "txtDeviationReg1";
            this.txtDeviationReg1.Size = new System.Drawing.Size(73, 20);
            this.txtDeviationReg1.TabIndex = 24;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(396, 39);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "Bazowy IF";
            // 
            // txtIFBase
            // 
            this.txtIFBase.Location = new System.Drawing.Point(458, 36);
            this.txtIFBase.Name = "txtIFBase";
            this.txtIFBase.Size = new System.Drawing.Size(73, 20);
            this.txtIFBase.TabIndex = 22;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(402, 13);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(50, 13);
            this.label14.TabIndex = 21;
            this.label14.Text = "Numer IF";
            // 
            // txtIFNumber
            // 
            this.txtIFNumber.Location = new System.Drawing.Point(458, 10);
            this.txtIFNumber.Name = "txtIFNumber";
            this.txtIFNumber.Size = new System.Drawing.Size(73, 20);
            this.txtIFNumber.TabIndex = 20;
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Location = new System.Drawing.Point(185, 142);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(66, 17);
            this.chkIsActive.TabIndex = 32;
            this.chkIsActive.Text = "Aktywny";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(185, 165);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(139, 23);
            this.btnUpdate.TabIndex = 33;
            this.btnUpdate.Text = "Zapisz";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(335, 117);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(117, 13);
            this.label9.TabIndex = 35;
            this.label9.Text = "Odchylenie - Regiony 3";
            // 
            // txtDeviationReg3
            // 
            this.txtDeviationReg3.Location = new System.Drawing.Point(458, 114);
            this.txtDeviationReg3.Name = "txtDeviationReg3";
            this.txtDeviationReg3.Size = new System.Drawing.Size(73, 20);
            this.txtDeviationReg3.TabIndex = 34;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(185, 189);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(139, 23);
            this.btnDelete.TabIndex = 36;
            this.btnDelete.Text = "Usun";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(12, 57);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(105, 47);
            this.btnNew.TabIndex = 37;
            this.btnNew.Text = "Nowa karta";
            this.btnNew.UseVisualStyleBackColor = true;
            // 
            // ConfigurationView
            // 
            this.ClientSize = new System.Drawing.Size(1127, 406);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtDeviationReg3);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.chkIsActive);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtDeviationReg2);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtDeviationReg1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtIFBase);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtIFNumber);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtDeviationIFMin);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtDeviationIF);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtFee);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtRegion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtProduct);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIrd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.grpOrganization);
            this.Controls.Add(this.dgvCriteria);
            this.Name = "ConfigurationView";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCriteria)).EndInit();
            this.grpOrganization.ResumeLayout(false);
            this.grpOrganization.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DataGridView DgvCryteria { get { return dgvCriteria; } }
        public bool IsVisa { get { return rdbVisa.Checked; } }

        [ConfigurationViewAttribute("Name", typeof(string))]
        public TextBox TxtName { get { return txtName; } }
        [ConfigurationViewAttribute("Ird", typeof(string))]
        public TextBox TxtIrd { get { return txtIrd; } }
        [ConfigurationViewAttribute("Product", typeof(string))]
        public TextBox TxtProduct { get { return txtProduct; } }
        [ConfigurationViewAttribute("Region", typeof(string))]
        public TextBox TxtRegion{ get { return txtRegion; } }
        [ConfigurationViewAttribute("Description", typeof(string))]
        public TextBox TxtDescription { get { return txtDescription; } }
        [ConfigurationViewAttribute("Fee", typeof(decimal))]
        public TextBox TxtFee { get { return txtFee; } }
        [ConfigurationViewAttribute("DeviationIF", typeof(decimal))]
        public TextBox TxtDeviationIF { get { return txtDeviationIF; } }
        [ConfigurationViewAttribute("DeviationIFMin", typeof(decimal))]
        public TextBox TxtDeviationIFMin { get { return txtDeviationIFMin; } }
        [ConfigurationViewAttribute("IFNumber", typeof(decimal))]
        public TextBox TxtIFNumber { get { return txtIFNumber; } }
        [ConfigurationViewAttribute("IFBase", typeof(decimal))]
        public TextBox TxtIFBase { get { return txtIFBase; } }
        [ConfigurationViewAttribute("DeviationReg1", typeof(decimal))]
        public TextBox TxtDeviationReg1 { get { return txtDeviationReg1; } }
        [ConfigurationViewAttribute("DeviationReg2", typeof(decimal))]
        public TextBox TxtDeviationReg2 { get { return txtDeviationReg2; } }
        [ConfigurationViewAttribute("DeviationReg3", typeof(decimal))]
        public TextBox TxtDeviationReg3 { get { return txtDeviationReg3; } }
        public CheckBox IsActive { get { return chkIsActive; } }

        public event EventHandler CheckedChanged;
        public event DataGridViewCellEventHandler CellClick;
        public event EventHandler UpdateClick;

        public ConfigurationView()
        {
            InitializeComponent();            
        }

        private void rdbVisa_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckedChanged != null)
                CheckedChanged(sender, e);
        }

        private void dgvCriteria_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CellClick != null)
                CellClick(sender, e);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (UpdateClick != null)
                UpdateClick(sender, e);
        }
    }
}
