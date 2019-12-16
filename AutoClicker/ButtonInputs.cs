using System;
using System.Windows.Forms;

namespace AutoClicker
{
    public partial class ButtonInputs : UserControl
    {
        private readonly uint _buttonDownCode;
        private readonly uint _buttonUpCode;

        public bool Needed => cbButtonEnable.Checked;

        public ButtonInputs(string buttonName, uint buttonDownCode, uint buttonUpCode)
        {
            InitializeComponent();

            this._buttonDownCode = buttonDownCode;
            this._buttonUpCode = buttonUpCode;
            cbButtonEnable.Text = buttonName;
            numDelay.Maximum = int.MaxValue;
            numDelay.Value = 200;
        }

        internal Clicker StartClicking(IntPtr minecraftHandle)
        {
            var delay = cbHold.Checked ? 0 : (int)numDelay.Value;
            var clicker = new Clicker(this._buttonDownCode, this._buttonUpCode, minecraftHandle);

            clicker.Start(delay);

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
