using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoClicker
{
    public partial class ButtonInputs : UserControl
    {
        private bool initialized = false;
        private Clicker clicker = null;

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

            cbButtonEnable.Text = buttonName;
            clicker = new Clicker(buttonDownCode, buttonUpCode);
            initialized = true;
        }

        public void Start(ICollection<IntPtr> minecraftHandles)
        {
            if (!initialized)
            {
                throw new Exception($"{nameof(ButtonInputs)}.{cbButtonEnable.Text} is not initialized!");
            }

            int delay = cbHold.Checked ? 0 : (int)numDelay.Value;
            clicker.Start(delay, minecraftHandles);
        }

        public void Stop()
        {
            clicker.Stop();
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

        public new void Dispose()
        {
            clicker?.Dispose();
            base.Dispose();
        }
    }
}
