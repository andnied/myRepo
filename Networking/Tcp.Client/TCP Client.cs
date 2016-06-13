using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tcp.Client
{
    public partial class Form1 : Form
    {
        private TcpClient _client;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (((Button)sender).Text == "Connect")
                {
                    _client = new TcpClient(txtIp.Text, int.Parse(txtPort.Text));
                    listBox1.Items.Add(string.Format("Connected to {0}:{1}", txtIp.Text, txtPort.Text));
                    btnConnect.Text = "Disconnect";
                }
                else
                {
                    _client.Close();
                    listBox1.Items.Add("Disconnected.");
                    btnConnect.Text = "Connect";
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add(ex.Message);
            }
        }

        private void txtPing_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ping = new Ping())
                {
                    ping.PingCompleted += Ping_PingCompleted;
                    ping.SendAsync(txtIp.Text, 120, new byte[] { 0 }, new PingOptions(2, true), null);
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add(ex.Message);
            }
        }

        private void Ping_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null)
            {
                listBox1.Items.Add(string.Format("Error{0}", ": " + e.Error.Message ?? string.Empty));
                return;
            }

            var response = e.Reply;
            if (response.Status == IPStatus.Success)
            {
                listBox1.Items.Add(string.Format("Response from {0}, with {1} bytes, within {2} ms.", response.Address, response.Buffer.Length, response.RoundtripTime));
            }
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            new Tracerouter().ShowDialog(this);
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
