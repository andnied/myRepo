namespace Tcp.Client
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.txtPing = new System.Windows.Forms.Button();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(12, 12);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(135, 20);
            this.txtIp.TabIndex = 0;
            this.txtIp.Text = "127.0.0.1";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(153, 12);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(48, 20);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "8080";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(207, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 20);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 38);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(351, 108);
            this.listBox1.TabIndex = 3;
            // 
            // txtPing
            // 
            this.txtPing.Location = new System.Drawing.Point(288, 12);
            this.txtPing.Name = "txtPing";
            this.txtPing.Size = new System.Drawing.Size(75, 20);
            this.txtPing.TabIndex = 4;
            this.txtPing.Text = "Ping";
            this.txtPing.UseVisualStyleBackColor = true;
            this.txtPing.Click += new System.EventHandler(this.txtPing_Click);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3});
            this.menuItem1.Text = "Options";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Traceroute";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "Exit";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 156);
            this.Controls.Add(this.txtPing);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIp);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "TCP Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button txtPing;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
    }
}

