using Microarea.Mago4Butler.Plugins;
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
            
            InsertRegKey();

            OpenFileDialog filedialog = new OpenFileDialog();
            string FileName = string.Empty;

            filedialog.ShowDialog();
            FileName = filedialog.FileName;

            //Process.Start("msiexec -i" , FileName);

        }


        void InsertRegKey( )

        {
            string regvalue = @"aaa{0}";
            regvalue = string.Format(regvalue, instance);

        }


    }
}
