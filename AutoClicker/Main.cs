using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace AutoClicker
{
    public partial class Main : Form
    {
        private readonly Dictionary<Process, List<Clicker>> instanceClickers = new Dictionary<Process, List<Clicker>>();
        private static readonly List<string> WindowTitles = new List<string>
        {
            "Minecraft",
            "RLCraft"
        };

        public Main()
        {
            InitializeComponent();
        }

        private void Btn_action_Click(object sender, EventArgs e)
        {
            try
            {
                EnableElements(false);
                var mcProcesses = Process.GetProcesses().Where(b => b.ProcessName.StartsWith("java") && WindowTitles.Any(title => b.MainWindowTitle.Contains(title))).ToList();
                var mainHandle = Handle;

                if (!mcProcesses.Any())
                {
                    // if we first don't find any windows matching an expected name, give the user the ability to override
                    var notRunning = new NotRunning();
                    if (notRunning.ShowDialog() != DialogResult.OK)
                    {
                        EnableElements(true);
                        return;
                    }

                    if(!string.IsNullOrEmpty(notRunning.ProcessTitle))
                        mcProcesses = Process.GetProcesses().Where(b => b.MainWindowTitle == notRunning.ProcessTitle).ToList();
                }

                if (!mcProcesses.Any())
                {
                    MessageBox.Show(@"Unable to find Minecraft process!", @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    EnableElements(true);
                    return;
                }

                if (mcProcesses.Count > 1)
                {
                    using (var instancesForm = new MultipleInstances(mcProcesses))
                    {
                        if (instancesForm.ShowDialog() != DialogResult.OK)
                        {
                            EnableElements(true);
                            return;
                        }   

                        mcProcesses = instancesForm.SelectedInstances.Select(Process.GetProcessById).ToList();
                    }
                }

                lblStartTime.Text = DateTime.Now.ToString("MMMM dd HH:mm tt");
                lblStarted.Visible = true;
                lblStartTime.Visible = true;

                foreach (var mcProcess in mcProcesses)
                {
                    SetControlPropertyThreadSafe(btn_start, "Enabled", false);
                    SetControlPropertyThreadSafe(btn_stop, "Enabled", true);

                    var minecraftHandle = mcProcess.MainWindowHandle;
                    FocusToggle(minecraftHandle);

                    SetControlPropertyThreadSafe(btn_start, "Text", @"Starting in: ");
                    Thread.Sleep(500);

                    for (var i = 5; i > 0; i--)
                    {
                        SetControlPropertyThreadSafe(btn_start, "Text", i.ToString());
                        Thread.Sleep(500);
                    }

                    FocusToggle(mainHandle);
                    SetControlPropertyThreadSafe(btn_start, "Text", @"Running...");
                    Thread.Sleep(500);

                    //Right click needs to be ahead of left click for concrete mining
                    if (biRightMouse.Needed)
                    {
                        var clicker = biRightMouse.StartClicking(minecraftHandle);
                        AddToInstanceClickers(mcProcess, clicker);
                    }

                    /*
                     * This sleep is needed, because if you want to mine concrete, then Minecraft starts to hold left click first
                     * and it won't place the block in your second hand for some reason...
                     */
                    Thread.Sleep(100);

                    if (biLeftMouse.Needed)
                    {
                        var clicker = biLeftMouse.StartClicking(minecraftHandle);
                        AddToInstanceClickers(mcProcess, clicker);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Stop();
            }
        }

        private void AddToInstanceClickers(Process mcProcess, Clicker clicker)
        {
            if (instanceClickers.ContainsKey(mcProcess))
                instanceClickers[mcProcess].Add(clicker);
            else
                instanceClickers.Add(mcProcess, new List<Clicker> { clicker });
        }

        private void Btn_stop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            btn_stop.Enabled = false;

            foreach (var clickers in instanceClickers.Values)
            {
                foreach (var clicker in clickers)
                {
                    clicker?.Dispose();
                }
            }

            instanceClickers.Clear();

            lblStarted.Visible = false;
            lblStartTime.Visible = false;

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
