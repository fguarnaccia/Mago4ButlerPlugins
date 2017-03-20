using Microarea.Mago4Butler.BL;
using Microarea.Mago4Butler.Model;
using Microarea.Mago4Butler.Properties;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Console = Colorful.Console;

namespace Microarea.Mago4Butler
{
    static class Program
    {
        internal const string LogFileName = "Mago4BulterLog.txt";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            Parameter p = null;
            if (args != null && args.Length > 0)
            {
                p = new Parameter() { Name = "args", Value = args };
            }
            var forrest = IoCContainer.Instance.Get<IForrest>(p);

            return forrest.Run();
        }

        static Program()
        {
            Task.Factory.StartNew(
                ()
                =>
                {
                    var settings = IoCContainer.Instance.Get<ISettings>();

                    RefreshLogConfiguration(settings);
                    EnsurePaths(settings);
                }
                );
        }

        private static void EnsurePaths(ISettings settings)
        {
            if (settings == null)
            {
                return;
            }
            try
            {
                var logsDirInfo = new DirectoryInfo(settings.LogsFolder);
                if (!logsDirInfo.Exists)
                {
                    logsDirInfo.Create();
                }
                var msiDirInfo = new DirectoryInfo(settings.MsiFolder);
                if (!msiDirInfo.Exists)
                {
                    msiDirInfo.Create();
                }
                var rootDirInfo = new DirectoryInfo(settings.RootFolder);
                if (!rootDirInfo.Exists)
                {
                    rootDirInfo.Create();
                }
            }
            catch
            {}
        }

        internal static void RefreshLogConfiguration(ISettings settings)
        {
            if (settings == null)
            {
                return;
            }
            try
            {
                log4net.GlobalContext.Properties["LogFilePath"] = Path.Combine(settings.LogsFolder, LogFileName);
                log4net.Config.XmlConfigurator.Configure();
            }
            catch
            {}
        }
    }
}
