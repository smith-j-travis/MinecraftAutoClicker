using System;
using System.Windows.Forms;

namespace AutoClicker
{
    public partial class ButtonInputs : UserControl
    {
        private readonly uint buttonDownCode;
        private readonly uint buttonUpCode;

        public bool Needed => cbButtonEnable.Checked;

        public ButtonInputs(string buttonName, uint buttonDownCode, uint buttonUpCode)
        {
            InitializeComponent();

            this.buttonDownCode = buttonDownCode;
            this.buttonUpCode = buttonUpCode;
            cbButtonEnable.Text = buttonName;
            numDelay.Maximum = int.MaxValue;
            numDelay.Value = 200;
        }

        internal Clicker StartClicking(IntPtr minecraftHandle)
        {
            var delay = (int)numDelay.Value;
            var clicker = new Clicker(buttonDownCode, buttonUpCode, minecraftHandle);

            clicker.Start(delay, cbHold.Checked);

            return clicker;
        }

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
            numDelay.Enabled = !cbHold.Checked;
        }
    }
}
