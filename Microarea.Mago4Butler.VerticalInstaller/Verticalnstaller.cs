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
        
        RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true);
        string m4key = "Microarea\\Mago4\\E51B08A3-8D02-44BE-B3BC-85144A6C7EBA";
        string mnkey = "Microarea\\Magonet\\94003900-4A72-4209-99B9-C7C1BCF7927F";

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
          
            InsertRegKey(istanza);

            OpenFileDialog filedialog = new OpenFileDialog();

            filedialog.ShowDialog();

   

            try
            {
                App.Instance.InstallMsi(filedialog.FileName);
            }
            catch (System.Exception )
            {
                return;
            }

        }

        void InsertRegKey(Instance istanza )

        {

            switch (istanza.ProductType)
            {
                case Microarea.Mago4Butler.Model.ProductType.Mago4:
                    key.CreateSubKey(m4key);
                    key = key.OpenSubKey(m4key, true);
                    break;

                case Microarea.Mago4Butler.Model.ProductType.Magonet:
                    key.CreateSubKey(mnkey);
                    key = key.OpenSubKey(mnkey, true);
                    break;

                case Microarea.Mago4Butler.Model.ProductType.None:
  
                    break;
            }
               
            key.SetValue("InstallDir", Path.Combine(App.Instance.Settings.RootFolder, istanza.Name));

        }


    }
}
