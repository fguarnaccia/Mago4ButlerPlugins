using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Microarea.Mago4Butler
{
    internal class ConsoleWrapper : IDisposable
    {
        Process process;
        bool attached;
        public ConsoleWrapper()
        {
            var ptr = SafeNativeMethods.GetForegroundWindow();

            int processId;
            SafeNativeMethods.GetWindowThreadProcessId(ptr, out processId);

            this.process = Process.GetProcessById(processId);

            if (String.Compare(process.ProcessName, "cmd", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                SafeNativeMethods.AttachConsole(process.Id);
                this.attached = true;
            }
            else
            {
                SafeNativeMethods.AllocConsole();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool managed)
        {
            if (managed)
            {
                if (this.attached)
                {
                    var hWnd = this.process.MainWindowHandle;
                    SafeNativeMethods.PostMessage(hWnd, SafeNativeMethods.WM_KEYDOWN, SafeNativeMethods.VK_RETURN, 0);

                }
            }
        }
    }
}