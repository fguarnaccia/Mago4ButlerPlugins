using Microarea.Mago4Butler.Plugins;
using Microarea.Mago4Butler.Telemetry.PAASUpdates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microarea.Mago4Butler.Telemetry
{
    public class Telemetry : Mago4ButlerPlugin, IWorker<TelemetryData>//, IMessageFilter
    {
        string machineName;
        string appVersion;
        IEnumerable<PluginData> pluginsData;

        public override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            this.Start();

            machineName = Environment.MachineName;
            appVersion = App.Instance.GetVersion("Mago4Butler").ToString();
            pluginsData = App.Instance.GetPluginsData()
                .Select(pd => new PluginData() { Name = pd.Split('-')[0], Version = pd.Split('-')[1] });

            this.Enqueue(new TelemetryData() { Event = TelemetryEvent.ApplicationStartup });//To log application startup

            //Application.AddMessageFilter(this);
        }
        public void OnRequestReceived(TelemetryData currentRequest)
        {
            using (var svc = new TelemetryService())
            {
#if DEBUG
                svc.Url = "http://usr-canessamat1/PAASUpdates/TelemetryService.asmx";
#else
                svc.Url = "http://spp-hotfix/PAASUpdates/TelemetryService.asmx";
#endif
                currentRequest.MachineName = machineName;
                currentRequest.PluginsData = pluginsData.ToArray();
                currentRequest.Mago4ButlerVersion = appVersion;

                try
                {
                    svc.StoreTelemetryData(currentRequest);
                }
                catch (Exception exc)
                {
                    App.Instance.Error("Exception sending telemetry data", exc);
                }
            }
        }

        //public bool PreFilterMessage(ref Message m)
        //{
        //    if (m.Msg == 513)
        //    {
        //        var control = Control.FromHandle(m.HWnd);
        //        if (control != null && control.Enabled)
        //        {
        //            if (control.GetType() == typeof(Button))
        //            {
        //                this.Enqueue(new TelemetryData() { Event = TelemetryEvent.ButtonClicked, Data = "control text: " + control.Text + "\t\tcontrol name: " + control.Name });
        //            }
        //        }
        //    }
        //    using (var sw = new StreamWriter("C:\\Log.txt", true))
        //    {
        //        if (m.Msg != 512 && m.Msg != 160)
        //        {
        //            var control = Control.FromHandle(m.HWnd);
        //            if (control != null)
        //            {
        //                sw.WriteLine("Message: " + m.Msg + "\t\tcontrol text: " + control.Text + "\t\tcontrol name: " + control.Name + "\t\tcontrol type: " + control.GetType().FullName);
        //            }
        //        }
        //        return false;
        //    }
        //}
    }
}
