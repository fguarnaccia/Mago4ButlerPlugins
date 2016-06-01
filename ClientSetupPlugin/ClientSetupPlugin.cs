using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microarea.Mago4Butler.Plugins;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Windows.Forms;


namespace ClientSetupPlugin
{
    public class ClientSetup : IPlugin
    {

        //return this.GetType().Assembly.GetName().Version;
        string instance = string.Empty;
        string customshare = "share {0}_Custom={1}";
        string standardshare = "share {0}_Standard={1}";
        string applicationpath = string.Empty;
        string linkUrl = "http://{0}/{1}/Index.htm";

        string CODargs = @" /root ""{0}\Apps"" /installation {1} /version release /uiCulture ""it-IT"" /webServicesPort {2}";
        //Deploy /root \"[INSTALLLOCATION]Apps\" /installation \"[INSTANCENAME]\" /version release /uiCulture \"[UICULTURE]\" /webServicesPort [DEFAULTWEBSITEPORT]
        const string cstdeploy = @"Deploy"; 
        const string cstupdatedeployment = @"UpdateDeployment"; 

        const string cstlinkName = "MagoClientSetupbyButler";


        public IEnumerable<ContextMenuItem> GetContextMenuItems()
        {
            var cmis = new List<ContextMenuItem>();

            ContextMenuItem cmi = new ContextMenuItem();

            cmi = new ContextMenuItem();
            cmi.Name = "runMadico";
            cmi.Text = "Check Instance";
            cmi.ShortcutKeys = Keys.Control | Keys.M;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunMadico(instance)
                );
            cmis.Add(cmi);


            cmi = new ContextMenuItem();
            cmi.Name = "runClientSetup";
            cmi.Text = "Prepare Client Setup";
            //cmi.ShortcutKeys = Keys.Control | Keys.R;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunClientSetup(instance)
                );
            cmis.Add(cmi);

            cmi = new ContextMenuItem();
            cmi.Name = "runCOD";
            cmi.Text = "ClickOnceDeployer";
            cmi.ShortcutKeys = Keys.Control | Keys.O;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunCOD(instance)
                );
            cmis.Add(cmi);

            cmi = new ContextMenuItem();
            cmi.Name = "divisionline";
            cmi.Text = "*************************";
            cmi.Command = new Action<Instance>(
               (instance)
               =>
               RunNothing(instance)
               );
            cmis.Add(cmi);


            return cmis;
        }
        private void RunNothing(Instance instance)
        {
            return;
        }
    

        public DoubleClickHandler GetDoubleClickHandler()
        {
        var dch = new DoubleClickHandler() { Name = "ClientSetupPlugin.Doubleclick" };
        dch.Command = (instance) => RunNothing(instance);
        return dch;
    }

        public void OnInstalling(CmdLineInfo cmdLineInfo)
        {
            return;

        }

        public void OnUpdating(CmdLineInfo cmdLineInfo)
        {
            return;
        }

        void RunClientSetup(Instance istanza)
        {
            applicationpath = Path.Combine(App.Instance.Settings.RootFolder, istanza.Name);      
            string custompath = Path.Combine(applicationpath , @"Custom");
            string standardpath = Path.Combine(applicationpath, @"Standard");
            string CODpath = Path.Combine(App.Instance.Settings.RootFolder, istanza.Name, @"Apps\ClickOnceDeployer\", "ClickOnceDeployer.exe");
            

            customshare = string.Format(customshare,  istanza.Name , custompath);      
            standardshare = string.Format(standardshare, istanza.Name, standardpath);
            //crea condivisioni
            ShareFolder(customshare);
            ShareFolder(standardshare);

            //esegue ClickonceDeployer
            CODargs = string.Format(CODargs, Path.Combine(App.Instance.Settings.RootFolder, istanza.Name), istanza.Name, GetSitePort(istanza));
    
            LaunchProcess(CODpath, cstdeploy + CODargs, 1000);
            LaunchProcess(CODpath, cstupdatedeployment + CODargs, 1000);

            //crea shortcut           
            using (StreamWriter writer = new StreamWriter(custompath  + "\\" + cstlinkName + ".url"))
            {
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=" + string.Format(linkUrl, System.Environment.MachineName, istanza.Name));
                writer.Flush();
            }

            MessageBox.Show("Fine Preparazione Client Setup","GS ClientSetupPlugin");

            return;
        }

        void RunMadico(Instance istanza)
        {

            string fileName = string.Empty;
            string executable = "Madico.msc";

            fileName = Path.Combine(App.Instance.Settings.RootFolder, istanza.Name, @"Apps\Microarea Diagnostics\", executable);
            Process.Start(fileName, "ui");
        }


        void ShareFolder(string sharestring)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "net";
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.Arguments = sharestring;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.Start();

        }

           string GetSitePort(Instance istanza)
        {
            string tcpport = "80";
            string xmlfile = string.Empty;
            XmlDocument domdoc = new XmlDocument();

            xmlfile = Path.Combine(App.Instance.Settings.RootFolder, istanza.Name, @"Custom\ServerConnection.config");
            domdoc.Load(xmlfile);

            tcpport = domdoc.GetElementsByTagName("WebServicesPort")[0].Attributes["value"].Value.ToString();

            return tcpport;


        }

        private void RunCOD(Instance istanza)
        {
            string fileName = string.Empty;
            string executable = "ClickOnceDeployer.exe";

            fileName = Path.Combine(App.Instance.Settings.RootFolder, istanza.Name, @"Apps\ClickOnceDeployer\", executable);
            Process.Start(fileName, "ui");
        }

        void LaunchProcess(string processFilePath, string args, int timeoutInMillSecs)
        {
            ProcessStartInfo psi = new ProcessStartInfo(processFilePath, args);
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            Process p = Process.Start(psi);
            string output = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();

            p.WaitForExit(timeoutInMillSecs);
            if (p.ExitCode != 0)
            {
                throw new Exception(String.Format("Process '{0}' returned following errors: {1}, {2}", processFilePath, output, error));
            }
        }
    }
}
