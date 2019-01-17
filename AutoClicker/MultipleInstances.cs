using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using AutoClicker.Controls;

namespace AutoClicker
{
    public partial class MultipleInstances : Form
    {
        public List<int> SelectedInstances { get; }

        public MultipleInstances(IEnumerable<Process> foundProcesses)
        {
            InitializeComponent();

            this.SelectedInstances = new List<int>();
            const int x = 25;
            var y = 20;
            var buttonX = this.grpInstances.Location.X + this.grpInstances.Width - 100;
            var processCount = 0;

            foreach (var process in foundProcesses)
            {
                var checkbox = new ValueCheckBox
                {
                    Text = process.MainWindowTitle,
                    Value = process.Id.ToString(),
                    Location = new Point(x, y),
                    AutoSize = true
                };

                var button = new Button
                {
                    Text = @"Focus",
                    Location = new Point(buttonX, y)
                };

                button.Click += (sender, e) => { Win32Api.SetForegroundWindow(process.MainWindowHandle); };

                this.grpInstances.Controls.Add(checkbox);
                this.grpInstances.Controls.Add(button);

                y += 25;
                processCount++;
                AdjustForm(processCount);
            }
        }

        private void Btn_ok_Click(object sender, EventArgs e)
        {
            var selectedInstances = this.grpInstances.AllControls<ValueCheckBox>().Where(b => b.Checked).ToList();

            if (!selectedInstances.Any())
            {
                MessageBox.Show(@"You must select at least one instance!", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var instance in selectedInstances)
                this.SelectedInstances.Add(int.Parse(instance.Value));

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void AdjustForm(int processCount)
        {
            if (processCount > 4)
            {
                this.grpInstances.Height += 25;
                this.Height += 25;
                this.btn_cancel.Location = new Point(this.btn_cancel.Location.X, this.btn_cancel.Location.Y + 25);
                this.btn_ok.Location = new Point(this.btn_ok.Location.X, this.btn_ok.Location.Y + 25);
            }
        }
    }
}
