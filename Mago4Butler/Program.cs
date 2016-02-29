using Microarea.Mago4Butler.BL;
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
            try
            {
                //Logica per l'aggiornamento del file di proprietà.
                //Come scritto nei remarks di http://msdn.microsoft.com/en-us/library/system.configuration.localfilesettingsprovider.upgrade.aspx
                //Standard Windows Forms and console applications must manually call Upgrade,
                //because there is not a general, automatic way to determine when such an application
                //is first run. The two common ways to do this are either from the installation program
                //or using from the application itself, using a persisted property, often named something
                //like IsFirstRun.
                //Questo vale solo quando l'applicazione gira solo come installazione server perchè,
                //quando invece gira come installazione client, è click once che pensa all'aggiornamento
                //del file di proprietà:
                //http://msdn.microsoft.com/en-us/library/ms228995.aspx come scritto al titolo "Version Upgrades"
                if (Settings.Default.IsFirstRun)
                {
                    try { Settings.Default.Upgrade(); }
                    catch { }
                    Settings.Default.IsFirstRun = false;
                    Settings.Default.Save();
                }
            }
            catch { }

            var settings = IoCContainer.Instance.Get<ISettings>();
            log4net.GlobalContext.Properties["LogFilePath"] = settings.LogsFolder;
            log4net.Config.XmlConfigurator.Configure();


        }
    }
}
