using Microarea.Mago4Butler.Plugins;
using Microarea.Mago4Butler.Telemetry.PAASUpdates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.Telemetry
{
    public class Telemetry : Mago4ButlerPlugin, IWorker<TelemetryData>
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

                svc.StoreTelemetryData(currentRequest);
            }
        }
    }
}
