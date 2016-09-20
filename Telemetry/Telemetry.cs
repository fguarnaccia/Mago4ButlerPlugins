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
        readonly object lockTicket = new object();
        Queue<TelemetryData> telemetryDatas = new Queue<TelemetryData>();

        string machineName;
        string appVersion;
        IEnumerable<PluginData> pluginsData;

        public override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            machineName = Environment.MachineName;
            appVersion = App.Instance.GetVersion("Mago4Butler").ToString();
            pluginsData = App.Instance.GetPluginsData()
                .Select(pd => new PluginData() { Name = pd.Split('-')[0], Version = pd.Split('-')[1] });

            var thread = new Thread(() => SenderThread());
            thread.IsBackground = true;
            thread.Start();

            Enqueue(new TelemetryData() { Event = TelemetryEvent.ApplicationStartup });//To log application startup
        }

        void Enqueue(TelemetryData request)
        {
            lock (this.lockTicket)
            {
                this.telemetryDatas.Enqueue(request);
            }
        }
        TelemetryData Dequeue()
        {
            lock (this.lockTicket)
            {
                if (this.telemetryDatas.Count > 0)
                {
                    return this.telemetryDatas.Dequeue();
                }

                return null;
            }
        }

        private void SenderThread()
        {
            using (var svc = new TelemetryService())
            {
#if DEBUG
                svc.Url = "http://usr-canessamat1/PAASUpdates/TelemetryService.asmx";
#else
                svc.Url = "http://spp-hotfix/PAASUpdates/TelemetryService.asmx";
#endif
                while (true)
                {
                    var currentTelemetryData = this.Dequeue();

                    while (currentTelemetryData == null)
                    {
                        Thread.Sleep(1000);
                        currentTelemetryData = this.Dequeue();
                    }

                    currentTelemetryData.MachineName = machineName;
                    currentTelemetryData.PluginsData = pluginsData.ToArray();
                    currentTelemetryData.Mago4ButlerVersion = appVersion;

                    svc.StoreTelemetryData(currentTelemetryData);
                    Thread.Sleep(1000);//Not to DOS our server...
                }
            }
        }
    }
}
