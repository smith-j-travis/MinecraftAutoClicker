using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoClicker
{
    internal class Clicker : IDisposable
    {
        private readonly uint buttonDownCode;
        private readonly uint buttonUpCode;
        private ICollection<IntPtr> minecraftHandles = null;
        private readonly Timer timer;

        private bool hold = false;

        public Clicker(uint buttonDownCode, uint buttonUpCode)
        {
            this.buttonDownCode = buttonDownCode;
            this.buttonUpCode = buttonUpCode;

            timer = new Timer();
            timer.Tick += Timer_Tick;
        }
        
        private void Timer_Tick(object sender, EventArgs e)
        {
            ToAllHandle((IntPtr handle) =>
            {
                Win32Api.PostMessage(handle, buttonDownCode, IntPtr.Zero, IntPtr.Zero);
                Win32Api.PostMessage(handle, buttonUpCode, IntPtr.Zero, IntPtr.Zero);
            });
        }

        public void Start(int delay, ICollection<IntPtr> minecraftHandles)
        {
            Stop();

            if (!(minecraftHandles != null && minecraftHandles.Count != 0))
            {
                return;
            }
            this.minecraftHandles = minecraftHandles;

            hold = (delay == 0);

            if (hold)
            {
                ToAllHandle((IntPtr handle) => Win32Api.PostMessage(handle, buttonDownCode, (IntPtr)0x0001, IntPtr.Zero));
            }
            else
            {
                timer.Interval = delay;
                timer.Start();
            }
        }

        public void Stop()
        {
            if (hold)
            {
                ToAllHandle((IntPtr handle) => Win32Api.PostMessage(handle, buttonUpCode, IntPtr.Zero, IntPtr.Zero));
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

        private void ToAllHandle(Action<IntPtr> todo)
        {
            foreach(var handle in minecraftHandles)
            {
                todo(handle);
            }
        }
    }
}
