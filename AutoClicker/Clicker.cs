using System;
using System.Windows.Forms;

namespace AutoClicker
{
    internal class Clicker : IDisposable
    {
        private readonly uint buttonPressCode;
        private readonly uint buttonReleaseCode;
        private readonly IntPtr handle;
        private readonly Timer timer;

        private bool hold = false;

        public Clicker(uint buttonPressCode, uint buttonReleaseCode, IntPtr handle)
        {
            this.buttonPressCode = buttonPressCode;
            this.buttonReleaseCode = buttonReleaseCode;
            this.handle = handle;

            timer = new Timer();
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Win32Api.PostMessage(handle, buttonPressCode, IntPtr.Zero, IntPtr.Zero);
            Win32Api.PostMessage(handle, buttonReleaseCode, IntPtr.Zero, IntPtr.Zero);
        }

        public void Start(int delay)
        {
            timer.Stop();

            if (delay == 0)
            {
                hold = true;
                Win32Api.PostMessage(handle, buttonPressCode, IntPtr.Zero, IntPtr.Zero);
            }
            else
            {
                hold = false;
                timer.Interval = delay;
                timer.Start();
            }
        }

        public void Stop()
        {
            if (hold)
            {
                Win32Api.PostMessage(handle, buttonReleaseCode, IntPtr.Zero, IntPtr.Zero);
            }
            else
            {
                timer.Stop();
            }
        }
        
        public void Dispose()
        {
            timer.Stop();
            timer.Dispose();
        }
    }
}
