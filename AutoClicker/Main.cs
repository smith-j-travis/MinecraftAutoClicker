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

        private const uint WmKeydown = 0x100;
        private const uint WmKeyup = 0x0101;

        private const int VkEsc = 0x1B;
        private const int WmLbuttonDown = 0x201;
        private const int WmRbuttonDown = 0x0204;

        private const int click = 0x00000001;

        private bool _stop;

        public Main()
        {
            InitializeComponent();
        }
        

        private void btn_fish_Click(object sender, EventArgs e)
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
            thread.DoWork += delegate { Fish(mcProcess, mainHandle, (uint)buttonCode, delay); };
            thread.RunWorkerCompleted += delegate { StopFish(mcProcess); };
            thread.RunWorkerAsync();

            Thread.Sleep(200);
            FocusToggle(mcProcess.MainWindowHandle);
            FocusToggle(this.Handle);
        }

        private void Fish(Process mcProcess, IntPtr mainWindowHandle, uint buttonCode, int delay)
        {
            SetControlPropertyThreadSafe(this.btn_start, "Text", @"Running...");
            SetControlPropertyThreadSafe(this.btn_start, "Enabled", false);
            SetControlPropertyThreadSafe(this.btn_stop, "Enabled", true);

            var handle = mcProcess.MainWindowHandle;
            PostMessage(handle, WmKeydown, (IntPtr)VkEsc, IntPtr.Zero);
            PostMessage(handle, WmKeyup, (IntPtr)VkEsc, IntPtr.Zero);

            FocusToggle(mcProcess.MainWindowHandle);
            FocusToggle(mainWindowHandle);

            var millisecondsPassed = -1;
            while (!this._stop)
            {
                if (millisecondsPassed == -1 || millisecondsPassed > delay)
                {
                    millisecondsPassed = 0;

                    PostMessage(handle, buttonCode, (IntPtr)click, IntPtr.Zero);
                    PostMessage(handle, buttonCode + 1, (IntPtr)click, IntPtr.Zero);
                }

                // sleep only 25 ms and do the check above so if the user clicks
                // "STOP" with a like 60 second delay, they don't have to wait 60 seconds
                Thread.Sleep(25);
                millisecondsPassed += 25;
            }

            SetControlPropertyThreadSafe(this.btn_start, "Text", @"START!");
            SetControlPropertyThreadSafe(this.btn_start, "Enabled", true);
        }

        private static void StopFish(Process mcProcess)
        {
            ShowWindow(mcProcess.MainWindowHandle, 1); // 1 = show normal: http://www.pinvoke.net/default.aspx/user32.showwindow
            FocusToggle(mcProcess.MainWindowHandle);

            // bring up menu
            PostMessage(mcProcess.MainWindowHandle, WmKeydown, (IntPtr)VkEsc, IntPtr.Zero);
            PostMessage(mcProcess.MainWindowHandle, WmKeyup, (IntPtr)VkEsc, IntPtr.Zero);
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
