using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AutoClicker
{
    public partial class Main : Form
    {
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd); 

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow); //ShowWindow needs an IntPtr

        private const int WmLbuttonDown = 0x201;
        private const int WmRbuttonDown = 0x0204;

        private bool _stop;

        public Main()
        {
            InitializeComponent();
        }
        

        private void btn_action_Click(object sender, EventArgs e)
        {
            var mcProcess = Process.GetProcessesByName("javaw").FirstOrDefault();
            var mainHandle = this.Handle;
            int delay;

            if (!int.TryParse(this.txtDelay.Text, out delay))
            {
                MessageBox.Show(@"The delay must be an integer!", @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (mcProcess == null || !mcProcess.MainWindowTitle.Contains("Minecraft"))
            {
                MessageBox.Show(@"Minecraft not running!", @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var buttonCode = rdio_RightClick.Checked ? WmRbuttonDown : WmLbuttonDown;

            this._stop = false;
            this.lblstart_time.Text = DateTime.Now.ToString("MMMM dd HH:mm tt");

            var thread = new BackgroundWorker();
            thread.DoWork += delegate { StartClick(mcProcess, mainHandle, (uint)buttonCode, delay, this.chkHold.Checked); };
            thread.RunWorkerAsync();

            Thread.Sleep(200);
            FocusToggle(mcProcess.MainWindowHandle);
            FocusToggle(this.Handle);
        }

        private void StartClick(Process mcProcess, IntPtr mainWindowHandle, uint buttonCode, int delay, bool miningMode)
        {
            SetControlPropertyThreadSafe(this.btn_start, "Enabled", false);
            SetControlPropertyThreadSafe(this.btn_stop, "Enabled", true);

            var handle = mcProcess.MainWindowHandle;
            FocusToggle(mcProcess.MainWindowHandle);

            SetControlPropertyThreadSafe(this.btn_start, "Text", @"Starting in: ");
            Thread.Sleep(500);

            for (var i = 5; i > 0; i--)
            {
                SetControlPropertyThreadSafe(this.btn_start, "Text", i.ToString());
                Thread.Sleep(500);
            }

            FocusToggle(mainWindowHandle);
            SetControlPropertyThreadSafe(this.btn_start, "Text", @"Running...");
            Thread.Sleep(500);

            var millisecondsPassed = -1;
            if (miningMode)
                PostMessage(handle, (uint)buttonCode, (IntPtr)0x0001, IntPtr.Zero); // send left button down

            while (!this._stop)
            {
                if (millisecondsPassed == -1 || millisecondsPassed >= delay)
                {
                    millisecondsPassed = 0;
                    if (!miningMode)
                    {
                        PostMessage(handle, buttonCode, IntPtr.Zero, IntPtr.Zero);
                        PostMessage(handle, buttonCode + 1, IntPtr.Zero, IntPtr.Zero);
                    }
                }

                // sleep only 25 ms and do the check above so if the user clicks
                // "STOP" with a like 60 second delay, they don't have to wait 60 seconds
                Thread.Sleep(5);
                millisecondsPassed += 5;
            }

            PostMessage(handle, buttonCode, IntPtr.Zero, IntPtr.Zero);
            PostMessage(handle, buttonCode + 1, IntPtr.Zero, IntPtr.Zero);

            SetControlPropertyThreadSafe(this.btn_start, "Text", @"START!");
            SetControlPropertyThreadSafe(this.btn_start, "Enabled", true);
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            this._stop = true;
            this.btn_stop.Enabled = false;
        }

        private static void FocusToggle(IntPtr hwnd)
        {
            Thread.Sleep(200);
            SetForegroundWindow(hwnd);
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
