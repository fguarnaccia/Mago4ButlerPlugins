using Microarea.Mago4Butler.BL;
using Microarea.Mago4Butler.Log;
using Microarea.Mago4Butler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler
{
    public class WebMediator : UIMediator
    {
        public WebCommand Command { get; } = new WebCommand();

        public WebMediator(
            Model.Model model,
            MsiService msiService,
            InstallerService installerService,
            LoggerService loggerService,
            PluginService pluginService,
            ISettings settings
            )
            :
            base (model, msiService, installerService, loggerService, pluginService, settings)
        { }

        protected override void OnProvisioningNeeded(ProvisioningEventArgs e)
        {
            base.OnProvisioningNeeded(e);
            //using (var provisioningForm = new ProvisioningFormLITE(instanceName: e.InstanceName, preconfigurationMode: true, loadDataFromFile: false))
            //{
            //    var provisioningSuggestions = new ProvisioningData()
            //    {
            //        CompanyDbName = string.Format("{0}_DB", e.InstanceName),
            //        DMSDbName = string.Format("{0}_DBDMS", e.InstanceName),
            //        SystemDbName = string.Format("{0}_SystemDB", e.InstanceName),
            //        AdminLoginName = string.Format("{0}_admin", e.InstanceName),
            //        UserLoginName = string.Format("{0}_user", e.InstanceName)
            //    };

            //    provisioningForm.ProvisioningData = provisioningSuggestions;

            //    var diagRes = provisioningForm.ShowDialog(this);

            //    if (diagRes == DialogResult.Cancel)
            //    {
            //        e.Cancel = true;
            //        return;
            //    }
            //    e.ProvisioningCommandLine = provisioningForm.PreconfigurationCommandLine;
            //}
        }

        protected override void OnAskForParametersForInstall(JobEventArgs e)
        {
            base.OnAskForParametersForInstall(e);
            //using (var askForParametersDialog = IoCContainer.Instance.Get<AskForParametersForm>())
            //{
            //    var diagRes = askForParametersDialog.ShowDialog(this);

            //    if (diagRes != DialogResult.OK)
            //    {
            //        e.Bag.Cancel = true;
            //        return;
            //    }
            //    e.Bag.InstanceName = askForParametersDialog.InstanceName;
            //}
        }
        protected override void OnJobNotification(JobEventArgs e)
        {
            base.OnJobNotification(e);
            if ((e.NotificationType & NotificationTypes.JobStarted) == NotificationTypes.JobStarted)
            {

            }
            if ((e.NotificationType & NotificationTypes.JobEnded) == NotificationTypes.JobEnded)
            {

            }
            if ((e.NotificationType & NotificationTypes.Progress) == NotificationTypes.Progress)
            {

            }
            if ((e.NotificationType & NotificationTypes.Notification) == NotificationTypes.Notification)
            {

            }
            if ((e.NotificationType & NotificationTypes.Error) == NotificationTypes.Error)
            {

            }
        }

        public void Execute()
        {
            if (Command == null)
            {
                return;
            }

            WebCommandType res = WebCommandType.None;
            if (!Enum.TryParse(Command.Type, out res))
            {
                return;
            }
            
            switch (res)
            {
                case WebCommandType.Install:
                    this.InstallInstance();
                    break;
                case WebCommandType.Remove:
                    this.RemoveInstances(null);
                    break;
                case WebCommandType.Update:
                    this.UpdateInstances(null);
                    break;
                default:
                    break;
            }
        }
    }

    public class WebCommand
    {
        public string Type { get; set; }
        public int InstanceIndex { get; set; }
    }

    public enum WebCommandType
    {
        None,
        Install,
        Remove,
        Update
    }
}
