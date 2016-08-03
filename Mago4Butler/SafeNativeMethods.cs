using System;
using System.Runtime.InteropServices;

namespace Microarea.Mago4Butler
{
    internal static class SafeNativeMethods
    {
        internal const int VK_RETURN = 0x0D;
        internal const int WM_KEYDOWN = 0x100;
        internal const int EM_SETCUEBANNER = 0x1501;

        internal const int LVM_FIRST = 0x1000;
        internal const int LVM_GETHEADER = LVM_FIRST + 31;

        internal const int HDM_FIRST = 0x1200;
        internal const int HDM_GETITEM = HDM_FIRST + 11;
        internal const int HDM_SETITEM = HDM_FIRST + 12;

        [StructLayout(LayoutKind.Sequential)]
        internal struct HDITEM
        {
            public Mask mask;
            public int cxy;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszText;
            public IntPtr hbm;
            public int cchTextMax;
            public Format fmt;
            public IntPtr lParam;
            // _WIN32_IE >= 0x0300 
            public int iImage;
            public int iOrder;
            // _WIN32_IE >= 0x0500
            public uint type;
            public IntPtr pvFilter;
            // _WIN32_WINNT >= 0x0600
            public uint state;

            [Flags]
            public enum Mask
            {
                Format = 0x4,       // HDI_FORMAT
            };

            [Flags]
            public enum Format
            {
                SortDown = 0x200,   // HDF_SORTDOWN
                SortUp = 0x400,     // HDF_SORTUP
            };
        };

        [DllImport("user32.dll", EntryPoint = "PostMessageA")]
        internal static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, ref HDITEM lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, string lp);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AllocConsole();

        [DllImport("kernel32", SetLastError = true)]
        internal static extern bool AttachConsole(int dwProcessId);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
    }
}