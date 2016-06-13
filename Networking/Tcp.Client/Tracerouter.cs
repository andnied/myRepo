using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tcp.Client
{
    public partial class Tracerouter : Form
    {
        private Ping _ping = null;
        private string _ip = null;
        private int _ttl = 0;
        private int _timeout = 0;
        private int _i = 1;

        public Tracerouter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _ping = new Ping();
            _ip = txtIp.Text;
            _ttl = (int)nmrTtl.Value;
            _timeout = (int)nmrTimeout.Value;
            
            _ping.PingCompleted += Ping_PingCompleted;
            _ping.SendAsync(_ip, _timeout, new byte[] { 0 }, new PingOptions(_i, true));
            listBox1.Items.Add("Trace started...");
            button1.Enabled = false;
        }

        private void Ping_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                listBox1.Items.Add("Error: " + e.Error.Message);
                return;
            }

            if (e.Cancelled)
                listBox1.Items.Add("Request cancelled.");
            else
            {
                if (e.Reply.Status == IPStatus.TtlExpired)
                    listBox1.Items.Add(string.Format("Trace: {0}, Host: {1}", _i, e.Reply.Address));
                if (e.Reply.Status == IPStatus.TimedOut)
                {
                    listBox1.Items.Add("Timeout");
                    button1.Enabled = true;
                    _ping.Dispose();
                    return;
                }
                if (e.Reply.Status == IPStatus.Success)
                {
                    listBox1.Items.Add(string.Format("Trace finished: {0}, Host: {1}", _i, e.Reply.Address));
                    button1.Enabled = true;
                    _ping.Dispose();
                    return;
                }

                if (_i++ < _ttl)
                    _ping.SendAsync(_ip, _timeout, new byte[] { 0 }, new PingOptions(_i, true));
            }
        }
    }
}
