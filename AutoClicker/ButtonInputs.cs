using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoClicker
{
    public partial class ButtonInputs : UserControl
    {
        private uint buttonDownCode;
        private uint buttonUpCode;
        private bool initialized = false;

        public bool Needed => cbButtonEnable.Checked;

        public ButtonInputs()
        {
            InitializeComponent();
            numDelay.Maximum = int.MaxValue;
        }

        public void Init(string buttonName, uint buttonDownCode, uint buttonUpCode)
        {
            //if(initialized)
            //{
            //    throw new Exception($"{nameof(ButtonInputs)}.{cbButtonEnable.Text} is already initialized!");
            //}

            this.buttonDownCode = buttonDownCode;
            this.buttonUpCode = buttonUpCode;
            cbButtonEnable.Text = buttonName;
            initialized = true;
        }

        internal Clicker StartClicking(IntPtr minecraftHandle)
        {
            if (!initialized)
            {
                throw new Exception($"{nameof(ButtonInputs)}.{cbButtonEnable.Text} is not initialized!");
            }
            int delay = cbHold.Checked ? 0 : (int)numDelay.Value;

            var clicker = new Clicker(buttonDownCode, buttonUpCode, minecraftHandle);
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
