using System;
using System.Windows.Forms;
using System.Reflection;

namespace ComplaintConfigSetup
{
    public partial class ConfigForm : Form
    {
        private readonly Assembly _complaintToolCommon;

        public ConfigForm()
        {
            InitializeComponent();

            try
            {
                _complaintToolCommon = Assembly.Load(new AssemblyName("ComplaintTool.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1bbab36f94aeb06e, processorArchitecture = AMD64"));
            }
            catch
            {
                MessageBox.Show("Could not load the assembly.");
                this.Close();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                var registryConfig = _complaintToolCommon.GetType("ComplaintTool.Common.Config.RegistryConfig");
                var conf = Activator.CreateInstance(registryConfig);
                registryConfig.GetProperty("ServerName").SetValue(conf, serverNameTextBox.Text);
                registryConfig.GetProperty("DatabaseName").SetValue(conf, databaseNameTextBox.Text);
                registryConfig.GetProperty("IntegratedSecurity").SetValue(conf, IntegratedSecurityCheckBox.Checked);

                if (!IntegratedSecurityCheckBox.Checked)
                {
                    registryConfig.GetProperty("UserID").SetValue(conf, userNameTextBox.Text);
                    var encryption = _complaintToolCommon.GetType("ComplaintTool.Common.Utils.Encryption");
                    var pass = encryption.GetMethod("Encrypt", BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).Invoke(encryption, new object[] { passwordTextBox.Text });
                    registryConfig.GetProperty("Password").SetValue(conf, pass);
                }

                var complaintConfig = _complaintToolCommon.GetType("ComplaintTool.Common.Config.ComplaintConfig");
                complaintConfig.GetMethod("SetConfig", BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).Invoke(complaintConfig, new object[] { conf, Type.Missing });

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void IntegratedSecurityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            label3.Visible = label4.Visible = userNameTextBox.Visible = 
                passwordTextBox.Visible = !IntegratedSecurityCheckBox.Checked;

            if (!IntegratedSecurityCheckBox.Checked)
            {
                userNameTextBox.Text = passwordTextBox.Text = "";
            }
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            try
            {
                var complaintConfig = _complaintToolCommon.GetType("ComplaintTool.Common.Config.ComplaintConfig");
                var complaintConfigInstance = complaintConfig.GetProperty("Instance").GetValue(null);
                var conf = complaintConfig.GetProperty("Conf").GetValue(complaintConfigInstance);
                if (conf != null)
                {
                    serverNameTextBox.Text = _complaintToolCommon.GetType("ComplaintTool.Common.Config.RegistryConfig").GetProperty("ServerName").GetValue(conf) as string;
                    databaseNameTextBox.Text = _complaintToolCommon.GetType("ComplaintTool.Common.Config.RegistryConfig").GetProperty("DatabaseName").GetValue(conf) as string;
                    IntegratedSecurityCheckBox.Checked = (bool)_complaintToolCommon.GetType("ComplaintTool.Common.Config.RegistryConfig").GetProperty("IntegratedSecurity").GetValue(conf);
                    userNameTextBox.Text = _complaintToolCommon.GetType("ComplaintTool.Common.Config.RegistryConfig").GetProperty("UserID").GetValue(conf) as string;

                    var encryption = _complaintToolCommon.GetType("ComplaintTool.Common.Utils.Encryption");
                    var pass = encryption.GetMethod("Decrypt", BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).Invoke(encryption, new object[] { _complaintToolCommon.GetType("ComplaintTool.Common.Config.RegistryConfig").GetProperty("Password").GetValue(conf) as string });

                    passwordTextBox.Text = pass as string;
                }
            }
            catch { }
        }
    }
}
