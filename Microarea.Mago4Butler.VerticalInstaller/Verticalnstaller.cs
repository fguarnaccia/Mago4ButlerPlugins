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
        
        readonly RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true);
        string m4ProductUpgradeCode = "E51B08A3-8D02-44BE-B3BC-85144A6C7EBA";
        string mnProductUpgradeCode = "94003900-4A72-4209-99B9-C7C1BCF7927F";
        string m4key = "Microarea\\Mago4\\{0}";
        string mnkey = "Microarea\\Magonet\\{0}";
        string masterProductUpgradeCode = string.Empty;
        string masterProductName = string.Empty;

        public override IEnumerable<ContextMenuItem> GetContextMenuItems()
        {

            m4key = string.Format(m4key, m4ProductUpgradeCode);
            mnkey = string.Format(mnkey, mnProductUpgradeCode);

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
          
            InsertRegKey(istanza);

            OpenFileDialog filedialog = new OpenFileDialog();

            filedialog.ShowDialog();

   

            try
            {
                App.Instance.InstallMsi(filedialog.FileName, masterProductUpgradeCode, masterProductName,  istanza.Name);
            }
            catch (System.Exception )
            {
                return;
            }

        }

        void InsertRegKey(Instance istanza )

        {
            RegistryKey localkey = null;


            switch (istanza.ProductType)
            {
                case Microarea.Mago4Butler.Model.ProductType.Mago4: //Mago4
                    key.CreateSubKey(m4key);
                    localkey = key.OpenSubKey(m4key, true);
                    masterProductUpgradeCode = m4ProductUpgradeCode;
                    masterProductName = "Mago4 ";
                    break;

                case Microarea.Mago4Butler.Model.ProductType.Magonet:  //Mago.netfatto
                    key.CreateSubKey(mnkey);
                    localkey = key.OpenSubKey(mnkey, true);
                    masterProductUpgradeCode = mnProductUpgradeCode;
                    masterProductName = "MagoNet";
                    break;

                case Microarea.Mago4Butler.Model.ProductType.None:

                    return;
            }
               
            localkey.SetValue("InstallDir", Path.Combine(App.Instance.Settings.RootFolder, istanza.Name));

        }


    }
}
