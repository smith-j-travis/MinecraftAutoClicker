using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace AutoClicker
{
    public partial class Main : Form
    {
        private Dictionary<Process, List<Clicker>> instanceClickers = new Dictionary<Process, List<Clicker>>();

        public Main()
        {
            InitializeComponent();
            biLeftMouse.Init("Left mouse button", Win32Api.WmLbuttonDown, Win32Api.WmLbuttonDown + 1);
            biRightMouse.Init("Right mouse button", Win32Api.WmRbuttonDown, Win32Api.WmRbuttonDown + 1);
        }

        private void Btn_action_Click(object sender, EventArgs e)
        {
            EnableElements(false);
            var mcProcesses = Process.GetProcessesByName("javaw").Where(b => b.MainWindowTitle.Contains("Minecraft")).ToList();
            var mainHandle = Handle;

            if (!mcProcesses.Any())
            {
                MessageBox.Show(@"Minecraft is not running!", @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EnableElements(true);
                return;
            }

            if (mcProcesses.Count > 1)
            {
                var instancesForm = new MultipleInstances(mcProcesses);

                if (instancesForm.ShowDialog() != DialogResult.OK)
                    return;

                mcProcesses = instancesForm.SelectedInstances.Select(Process.GetProcessById).ToList();
            }

            this.lblstart_time.Text = DateTime.Now.ToString("MMMM dd HH:mm tt");

            foreach (var mcProcess in mcProcesses)
            {
                SetControlPropertyThreadSafe(this.btn_start, "Enabled", false);
                SetControlPropertyThreadSafe(this.btn_stop, "Enabled", true);

                var minecraftHandle = mcProcess.MainWindowHandle;
                FocusToggle(minecraftHandle);

                SetControlPropertyThreadSafe(this.btn_start, "Text", @"Starting in: ");
                Thread.Sleep(500);

                for (var i = 5; i > 0; i--)
                {
                    SetControlPropertyThreadSafe(this.btn_start, "Text", i.ToString());
                    Thread.Sleep(500);
                }

                FocusToggle(mainHandle);
                SetControlPropertyThreadSafe(this.btn_start, "Text", @"Running...");
                Thread.Sleep(500);

                if(biLeftMouse.Needed)
                {
                    var clicker = biLeftMouse.StartClicking(minecraftHandle);
                    AddToInstanceClickers(mcProcess, clicker);
                }

                if(biRightMouse.Needed)
                {
                    var clicker = biRightMouse.StartClicking(minecraftHandle);
                    AddToInstanceClickers(mcProcess, clicker);
                }

                Thread.Sleep(200);
                FocusToggle(minecraftHandle);
                FocusToggle(mainHandle);
            }
        }

        private void AddToInstanceClickers(Process mcProcess, Clicker clicker)
        {
            if(instanceClickers.ContainsKey(mcProcess))
            {
                instanceClickers[mcProcess].Add(clicker);
            }
            else
            {
                instanceClickers.Add(mcProcess, new List<Clicker>() { clicker });
            }
        }

        private void Btn_stop_Click(object sender, EventArgs e)
        {
            this.btn_stop.Enabled = false;

            foreach(var clickers in instanceClickers.Values)
            {
                foreach(var clicker in clickers)
                {
                    clicker.Dispose();
                }
            }
            instanceClickers.Clear();

            btn_start.Text = "START";
            EnableElements(true);
        }

        private void EnableElements(bool enable)
        {
            btn_start.Enabled = enable;
            biLeftMouse.Enabled = enable;
            biRightMouse.Enabled = enable;
            btn_stop.Enabled = !enable;
        }

        private static void FocusToggle(IntPtr hwnd)
        {
            Thread.Sleep(200);
            Win32Api.SetForegroundWindow(hwnd);
        }

        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);
        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), control, propertyName, propertyValue);
            else
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new[] { propertyValue });
        }//end SetControlPropertyThreadSafe
    }
}
