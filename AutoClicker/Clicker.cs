using System;
using System.Windows.Forms;

namespace AutoClicker
{
    internal class Clicker : IDisposable
    {
        private readonly uint buttonDownCode;
        private readonly uint buttonUpCode;
        private readonly IntPtr minecraftHandle;
        private readonly Timer timer;

        private bool hold = false;
        private bool disposed;

        public Clicker(uint buttonDownCode, uint buttonUpCode, IntPtr minecraftHandle)
        {
            this.buttonDownCode = buttonDownCode;
            this.buttonUpCode = buttonUpCode;
            this.minecraftHandle = minecraftHandle;

            timer = new Timer();
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // keep sending hold every tick as well in case they do something to interrupt the input
            if(hold)
            {
                Hold();
            } else
            {
                Click();
            }
        }

        public void Start(int delay, bool hold)
        {
            this.hold = hold;
            Stop();

            if (hold)
            {
                Hold();
                timer.Interval = delay;
                timer.Start();

            }
            else
            {
                Click();
                timer.Interval = delay;
                timer.Start();
            }
        }

        public void Stop()
        {
            if (!hold)
                timer.Stop();

            Click();
        }

        private void Hold()
        {
            Win32Api.PostMessage(minecraftHandle, buttonDownCode, IntPtr.Zero, IntPtr.Zero);
        }

        private void Click()
        {
            Win32Api.PostMessage(minecraftHandle, buttonDownCode, IntPtr.Zero, IntPtr.Zero);
            Win32Api.PostMessage(minecraftHandle, buttonUpCode, IntPtr.Zero, IntPtr.Zero);
        }

        #region Dispose
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Stop();
                timer.Dispose();
            }

            disposed = true;
        }

        ~Clicker()
        {
            Dispose(false);
        }
        #endregion
    }
}
