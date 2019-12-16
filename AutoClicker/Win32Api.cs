using System;
using System.Runtime.InteropServices;

namespace AutoClicker
{
    public class Win32Api
    {
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow); //ShowWindow needs an IntPtr

        public static uint WmRbuttonDown => 0x0204;
        public static uint WmLbuttonDown => 0x201;
    }
}
