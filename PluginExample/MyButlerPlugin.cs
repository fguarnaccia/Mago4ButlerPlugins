using Microarea.Mago4Butler.Plugins;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PluginExample
{
    public class MyButlerPlugin : IPlugin
    {
        public IEnumerable<ContextMenuItem> GetContextMenuItems()
        {
            ContextMenuItem myItem = new ContextMenuItem();
            myItem.Name = "MyItem";
            myItem.Text = "Mt Item...";
            myItem.Command = new Action<Instance>(CommandHandler);
            myItem.ShortcutKeys = Keys.Alt | Keys.S;

            return new ContextMenuItem[] { myItem };
        }

        public void CommandHandler(Instance instance)
        {
            var settings = App.Instance.Settings;
            System.Windows.Forms.MessageBox.Show("MyButlerPlugin: " + instance.Name + ", in " + settings.RootFolder);
        }
    }
}
