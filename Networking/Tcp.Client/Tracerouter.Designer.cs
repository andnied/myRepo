namespace Tcp.Client
{
    partial class Tracerouter
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
            this.txtIp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.nmrTtl = new System.Windows.Forms.NumericUpDown();
            this.nmrTimeout = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nmrTtl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(88, 12);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(100, 20);
            this.txtIp.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP Address";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 90);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(260, 95);
            this.listBox1.TabIndex = 2;
            // 
            // nmrTtl
            // 
            this.nmrTtl.Location = new System.Drawing.Point(88, 38);
            this.nmrTtl.Name = "nmrTtl";
            this.nmrTtl.Size = new System.Drawing.Size(100, 20);
            this.nmrTtl.TabIndex = 3;
            this.nmrTtl.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // nmrTimeout
            // 
            this.nmrTimeout.Location = new System.Drawing.Point(88, 64);
            this.nmrTimeout.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nmrTimeout.Name = "nmrTimeout";
            this.nmrTimeout.Size = new System.Drawing.Size(100, 20);
            this.nmrTimeout.TabIndex = 4;
            this.nmrTimeout.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Max TTL";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Timeout";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(197, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 20);
            this.button1.TabIndex = 7;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(197, 38);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 20);
            this.button2.TabIndex = 8;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Tracerouter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 196);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nmrTimeout);
            this.Controls.Add(this.nmrTtl);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtIp);
            this.Name = "Tracerouter";
            this.Text = "Tracerouter";
            ((System.ComponentModel.ISupportInitialize)(this.nmrTtl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrTimeout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.NumericUpDown nmrTtl;
        private System.Windows.Forms.NumericUpDown nmrTimeout;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}