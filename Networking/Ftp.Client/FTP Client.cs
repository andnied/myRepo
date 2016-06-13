using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ftp.Client
{
    public partial class FTP_Client : Form
    {
        private FolderBrowserDialog _folderDialog = null;

        public FTP_Client()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _folderDialog = new FolderBrowserDialog();
            
            if (_folderDialog.ShowDialog() == DialogResult.OK)
                textBox1.Text = _folderDialog.SelectedPath;
        }

        private void FTP_Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_folderDialog != null)
                try
                {
                    _folderDialog.Dispose();
                }
                catch { }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtUserName.Enabled = !checkBox1.Checked;
            txtPassword.Enabled = !checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var client = new FtpClient(txtServer.Text, txtUserName.Text, txtPassword.Text);
                var dirs = client.GetDirectories();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
