using Microarea.Mago4Butler.BL;
using Microarea.Mago4Butler.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microarea.Mago4Butler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
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
        }
    }
}
