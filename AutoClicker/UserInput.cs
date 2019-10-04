using System;
using System.Windows.Forms;

namespace AutoClicker
{
    public partial class UserInput : UserControl
    {
        public UserInput()
        {
            InitializeComponent();
            numDelay.Minimum = 1;
            numDelay.Maximum = int.MaxValue;
        }

        public string ButtonName
        {
            get { return cbButtonEnable.Text; }
            set { cbButtonEnable.Text = value; }
        }

        public uint Delay => cbHold.Checked ? 0 : (uint)numDelay.Value;

        private void CbButtonEnable_Click(object sender, EventArgs e)
        {
            if (cbButtonEnable.Checked)
            {
                cbHold.Enabled = true;
                HoldCheckBoxClicked();
            }
            else
            {
                cbHold.Enabled = false;
                numDelay.Enabled = false;
            }
        }

        private void CbHold_Click(object sender, EventArgs e)
        {
            HoldCheckBoxClicked();
        }

        private void HoldCheckBoxClicked()
        {
            if (cbHold.Checked)
            {
                numDelay.Enabled = false;
            }
            else
            {
                numDelay.Enabled = true;
            }
        }
    }
}
