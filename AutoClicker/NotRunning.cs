using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace AutoClicker
{
    public partial class NotRunning : Form
    {
        public NotRunning()
        {
            InitializeComponent();
        }

        public string ProcessTitle { get; private set; }

        private void NotRunning_Load(object sender, EventArgs e)
        {
            var processes = Process.GetProcesses().Where(b => b.ProcessName.StartsWith("java")).OrderBy(b => b.MainWindowTitle).Select(b => b.MainWindowTitle).ToArray();
            cmbProcess.Items.AddRange(processes);

            // if there is only 1 item, may as well select it for the, as it is probably the window they want
            if (cmbProcess.Items.Count == 1)
                cmbProcess.SelectedItem = cmbProcess.Items[0];
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            if(cmbProcess.SelectedItem != null)
                this.ProcessTitle = cmbProcess.SelectedItem.ToString();

            this.Close();
        }
    }
}
