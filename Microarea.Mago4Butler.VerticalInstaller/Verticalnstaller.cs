using Microarea.Mago4Butler.Plugins;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VerticalInstallerPlugin
{
    public class Verticalnstaller : Mago4ButlerPlugin
    {
        string root = string.Empty;
        string instance = string.Empty;

        public override IEnumerable<ContextMenuItem> GetContextMenuItems()
        {
            var cmis = new List<ContextMenuItem>();

            ContextMenuItem cmi = new ContextMenuItem();
            cmi.Name = "runVerticalInstaller";
            cmi.Text = "...add Vertical App";
            cmi.ShortcutKeys = Keys.Control | Keys.V;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunVerticalInstaller(instance)
                );
            cmis.Add(cmi);

            return cmis;
        }


        void RunVerticalInstaller(Instance istanza)
        {
            instance = istanza.Name;

            InsertRegKey(istanza);

            OpenFileDialog filedialog = new OpenFileDialog();
            string FileName = string.Empty;

            filedialog.ShowDialog();
            //TODO togliere -i quando matteo sistema il butler
            FileName = "-i " + filedialog.FileName;

            App.Instance.InstallMsi(FileName.ToString());          

        }

        void InsertRegKey(Instance istanza )

        {

            RegistryKey keym4 = Registry.LocalMachine.OpenSubKey("Software", true);
            RegistryKey keymn = Registry.LocalMachine.OpenSubKey("Software", true);

            keym4.CreateSubKey("Microarea\\Mago4\\E51B08A3-8D02-44BE-B3BC-85144A6C7EBA");
            keymn.CreateSubKey("Microarea\\Magonet\\94003900-4A72-4209-99B9-C7C1BCF7927F");

            keym4 = keym4.OpenSubKey("Microarea\\Mago4\\E51B08A3-8D02-44BE-B3BC-85144A6C7EBA", true);
            keymn =  keymn.OpenSubKey("Microarea\\Magonet\\94003900-4A72-4209-99B9-C7C1BCF7927F", true                );

            keym4.SetValue("InstallDir", Path.Combine(App.Instance.Settings.RootFolder, istanza.Name));
            keymn.SetValue("InstallDir" , Path.Combine(App.Instance.Settings.RootFolder, istanza.Name));

            }


    }
}
