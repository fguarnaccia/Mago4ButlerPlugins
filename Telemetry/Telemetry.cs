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
    public class Telemetry : Mago4ButlerPlugin
    {
        public override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            var thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    StoreTelemetry();
                }
                catch
                {}
            }));
            thread.IsBackground = true;
            thread.Start();
        }

        void StoreTelemetry()
        {
            string version = App.Instance.GetVersion("Mago4Butler").ToString();
            var pluginsData = App.Instance.GetPluginsData()
                .Select(pd => new PluginData() { Name = pd.Split('-')[0], Version = pd.Split('-')[1] });

            using (var svc = new TelemetryService())
            {
#if DEBUG
                svc.Url = "http://usr-canessamat1/PAASUpdates/TelemetryService.asmx";
#else
                svc.Url = "http://spp-hotfix/PAASUpdates/TelemetryService.asmx";
#endif
                svc.StoreTelemetryData(new PAASUpdates.TelemetryData()
                {
                    MachineName = Environment.MachineName,
                    Mago4ButlerVersion = version,
                    PluginsData = pluginsData.ToArray()
                });
            }
        }
    }
}
