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
    public class Reporter : Mago4ButlerPlugin
    {
        readonly object lockTicket = new object();
        Queue<ReportData> reportDatas = new Queue<ReportData>();

        string appVersion;
        string[] pluginsData;

        public override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            appVersion = App.Instance.GetVersion("Mago4Butler").ToString();
            pluginsData = App.Instance.GetPluginsData();

            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;

            var thread = new Thread(() => ReporterThread());
            thread.IsBackground = true;
            thread.Start();
        }

        void Enqueue(ReportData request)
        {
            lock (this.lockTicket)
            {
                this.reportDatas.Enqueue(request);
            }
        }
        ReportData Dequeue()
        {
            lock (this.lockTicket)
            {
                if (this.reportDatas.Count > 0)
                {
                    return this.reportDatas.Dequeue();
                }

                return null;
            }
        }

        private void ReporterThread()
        {
            using (var svc = new ReportService())
            {
#if DEBUG
                svc.Url = "http://usr-canessamat1/PAASUpdates/ReportService.asmx";
#else
                svc.Url = "http://spp-hotfix/PAASUpdates/ReportService.asmx";
#endif
                while (true)
                {
                    var currentReportData = this.Dequeue();

                    while (currentReportData == null)
                    {
                        Thread.Sleep(1000);
                        currentReportData = this.Dequeue();
                    }

                    svc.StoreReportData(currentReportData);
                    Thread.Sleep(1000);//Not to DOS our server...
                }
            }
        }

        private void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            Enqueue(new ReportData()
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
