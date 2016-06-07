using Microarea.Mago4Butler.Plugins;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PluginExample
{
    public class MyButlerPlugin : Mago4ButlerPlugin
    {
        public override IEnumerable<ContextMenuItem> GetContextMenuItems()
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

            App.Instance.ShutdownApplication();
        }

        public override DoubleClickHandler GetDoubleClickHandler()
        {
            var dch = new DoubleClickHandler() { Name = "MyButlerPlugin.DoubleClick" };
            dch.Command = (instance) => { MessageBox.Show("MyButlerPlugin: double click on" + instance.Name); };

            return dch;
        }

        public override void OnUpdating(CmdLineInfo cmdLineInfo)
        {
            cmdLineInfo.ClassicApplicationPoolPipeline = false;
        }

        public override void OnInstalling(CmdLineInfo cmdLineInfo)
        {
            cmdLineInfo.ClassicApplicationPoolPipeline = false;
        }

        public override void OnApplicationStarted()
        {
            throw new NotImplementedException();
        }

        public override void OnInstallerServiceStopped()
        {
            
        }

        public override void OnInstallerServiceStarted()
        {
          
        }

        public override void OnRemoving(Instance[] instances)
        {
            
        }
    }
}
