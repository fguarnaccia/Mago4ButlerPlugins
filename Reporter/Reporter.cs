using Microarea.Mago4Butler.Plugins;
using Microarea.Mago4Butler.Reporter.PAASUpdates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.Reporter
{
    public class Reporter : Mago4ButlerPlugin, IWorker<ReportData>
    {
        string appVersion;
        string[] pluginsData;

        public override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            this.Start();

            appVersion = App.Instance.GetVersion("Mago4Butler").ToString();
            pluginsData = App.Instance.GetPluginsData();

            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
        }
        public void OnRequestReceived(ReportData currentRequest)
        {
            using (var svc = new ReportService())
            {
#if DEBUG
                svc.Url = "http://usr-canessamat1/PAASUpdates/ReportService.asmx";
#else
                svc.Url = "http://spp-hotfix/PAASUpdates/ReportService.asmx";
#endif
                try
                {
                    svc.StoreReportData(currentRequest);
                }
                catch (Exception exc)
                {
                    App.Instance.Error("Exception sending reporting data", exc);
                }
            }
        }

        private void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            this.Enqueue(new ReportData()
            {
                MachineName = Environment.MachineName,
                OSVersion = Environment.OSVersion.ToString(),
                NetFxVersion = Environment.Version.ToString(),
                Sender = sender.ToString(),
                Exception = e.Exception.ToString()
            });
        }
    }
}
