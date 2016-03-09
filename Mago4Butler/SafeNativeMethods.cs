using System;
using System.Runtime.InteropServices;

namespace Microarea.Mago4Butler
{
    internal static class SafeNativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AllocConsole();

        [DllImport("kernel32", SetLastError = true)]
        internal static extern bool AttachConsole(int dwProcessId);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        internal static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        internal const int VK_RETURN = 0x0D;
        internal const int WM_KEYDOWN = 0x100;
        internal const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, string lp);
    }
}