using Microarea.Mago4Butler.Plugins;
using System.Xml;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MagoRunnerPlugin
{
    public class MagoRunner : IPlugin     
    {
        string root = string.Empty;
        string instance = string.Empty;
       
        //string IPlugin.Root
        //{
        //    set
        //    {
        //        root = value;
        //    }
        //    get
        //    { return root; }
        //}

        //string IPlugin.Instance
        //{
        //    set
        //    {
        //        instance = value;
        //    }
        //    get
        //    { return instance; }
        //}

        //void IPlugin.RunCompo(string Compo)
        //{
        //    Process process = new Process();

        //    if (Compo == "ac")
        //    {
        //        process.StartInfo.FileName = root + instance + @"\Apps\Publish\" + "AdministrationConsole.exe";
        //        process.Start();
        //    }
            
        //}

        public IEnumerable<ContextMenuItem> GetContextMenuItems()
        {

            var cmis = new List<ContextMenuItem>();

            ContextMenuItem cmi = new ContextMenuItem();

            cmi.Name = "runRoot";
            cmi.Text = "RootFolder";
            cmi.ShortcutKeys = Keys.Control | Keys.R;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunRoot(instance)
                );

            cmis.Add(cmi);

            cmi = new ContextMenuItem();

            cmi.Name = "runMadico";
            cmi.Text = "check Instance";
            cmi.ShortcutKeys = Keys.Control | Keys.M;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunMadico(instance)
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

            cmi.Name = "runMago";
            cmi.Text = "Mago";
            cmi.ShortcutKeys = Keys.F9;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunMago(instance)
                );

            cmis.Add(cmi);

            cmi = new ContextMenuItem();

            cmi.Name = "runConsole";
            cmi.Text = "Admin Console";
            cmi.ShortcutKeys = Keys.Control | Keys.A;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunConsole(instance)
                );

            cmis.Add(cmi);

            cmi = new ContextMenuItem();

            cmi.Name = "runCustom";
            cmi.Text = "Custom";
            cmi.ShortcutKeys = Keys.Control | Keys.C;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunCustom(instance)
                );
            cmis.Add(cmi);

            cmi = new ContextMenuItem();

            cmi.Name = "runPublish";
            cmi.Text = "Publish";
            cmi.ShortcutKeys = Keys.Control | Keys.P;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunPublish(instance)
                );
            cmis.Add(cmi);

            cmi = new ContextMenuItem();

            cmi.Name = "runStandard";
            cmi.Text = "Standard";
            cmi.ShortcutKeys = Keys.Control | Keys.S;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunStandard(instance)
                );
            cmis.Add(cmi);

            cmi = new ContextMenuItem();

            cmi.Name = "runMagoWeb";
            cmi.Text = "Easylook/MagoWeb";
            cmi.ShortcutKeys = Keys.Control | Keys.W;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunMagoWeb(instance)
                );
            cmis.Add(cmi);

            return cmis;
            //return CreateMenu();
        }

        

        public DoubleClickHandler GetDoubleClickHandler()
        {

            var dch = new DoubleClickHandler() { Name = "MagoRunnerPlugin.Doubleclick" };
            dch.Command = (instance) => RunMago(instance);
            return dch;
                 
        }

        private List<ContextMenuItem> CreateMenu()
        {

         
            var cmis = new List<ContextMenuItem>();
            ContextMenuItem cmi = new ContextMenuItem();

            cmi.Name = "runRoot";
            cmi.Text = "ROOT";
            cmi.ShortcutKeys = Keys.Control | Keys.R;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunRoot(instance)
                );

            cmis.Add(cmi);

            cmi = new ContextMenuItem();

            cmi.Name = "runMago";
            cmi.Text = "Mago";
            cmi.ShortcutKeys = Keys.F9;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunMago(instance)
                );

            cmis.Add(cmi);

            cmi = new ContextMenuItem();

            cmi.Name = "runConsole";
            cmi.Text = "Admin Console";
            cmi.ShortcutKeys = Keys.Control | Keys.A;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunConsole(instance)
                );

            cmis.Add(cmi);

            cmi = new ContextMenuItem();

            cmi.Name = "runCustom";
            cmi.Text = "Custom";
            cmi.ShortcutKeys = Keys.Control | Keys.C;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunCustom(instance)
                );
            cmis.Add(cmi);

            cmi = new ContextMenuItem();

            cmi.Name = "runPublish";
            cmi.Text = "Publish";
            cmi.ShortcutKeys = Keys.Control | Keys.P;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunPublish(instance)
                );
            cmis.Add(cmi);

            cmi = new ContextMenuItem();

            cmi.Name = "runStandard";
            cmi.Text = "Standard";
            cmi.ShortcutKeys = Keys.Control | Keys.S;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunStandard(instance)
                );
            cmis.Add(cmi);

            cmi = new ContextMenuItem();

            cmi.Name = "runMagoWeb";
            cmi.Text = "Easylook/MagoWeb";
            cmi.ShortcutKeys = Keys.Control | Keys.W;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunMagoWeb(instance)
                );
            cmis.Add(cmi);

            return cmis;
        }

        void RunRoot(Instance istanza)
        {

            string FileName = string.Empty;
            FileName = Path.Combine(App.Instance.Settings.RootFolder);

            Process.Start(FileName);

        }
        private void RunMadico(Instance istanza)
        {

            string fileName = string.Empty;
            string executable = "Madico.msc";

            fileName = Path.Combine(App.Instance.Settings.RootFolder, istanza.Name, @"Apps\Microarea Diagnostics\", executable);
            Process.Start(fileName, "ui");
        }

        private void RunCOD(Instance istanza)
        {
            string fileName = string.Empty;
            string executable = "ClickOnceDeployer.exe";

            fileName = Path.Combine(App.Instance.Settings.RootFolder, istanza.Name, @"Apps\ClickOnceDeployer\", executable);
            Process.Start(fileName, "ui");
        }
        void RunMago(Instance istanza)
        {

            string fileName = string.Empty;
            string executable = string.Empty;

            if (IsMago4(istanza.Version))

            {
                executable = "TbAppManager.exe";
            

            }
            else
            {
                executable = "Mago.Net.Exe";
  
            }

            fileName = Path.Combine(App.Instance.Settings.RootFolder, istanza.Name, @"Apps\Publish\", executable);
            Process.Start(fileName);

        }
        void RunConsole(Instance istanza)
        {

            string fileName = string.Empty;
            string executable = string.Empty;

            if (IsMago4(istanza.Version))

            {
                executable = "AdministrationConsole.exe";
     

            }
            else
            {
                executable = "MicroareaConsole.exe";

            }

            fileName = Path.Combine(App.Instance.Settings.RootFolder, istanza.Name, @"Apps\Publish\", executable);

            Process.Start(fileName);

        }
        void RunCustom(Instance istanza)
        {

            string FileName = string.Empty;

            FileName = Path.Combine(App.Instance.Settings.RootFolder, istanza.Name, @"Custom");

            Process.Start(FileName);

        }
        void RunPublish(Instance istanza)
        {

            string FileName = string.Empty;

            FileName = Path.Combine(App.Instance.Settings.RootFolder, istanza.Name, @"Apps\Publish");

            Process.Start(FileName);

        }
        void RunStandard(Instance istanza)
        {

          string FileName = string.Empty;

            FileName = Path.Combine(App.Instance.Settings.RootFolder, istanza.Name, @"Standard");

            Process.Start(FileName);

        }
        void RunMagoWeb(Instance istanza)
        {

            string porta = GetSitePort(istanza);
            string FileName = string.Empty;

            if (IsMago4 (istanza.Version))
            {
                return;
                //FileName = "http://localhost:" + porta + "/" + istanza.Name + "/MagoWeb.aspx";

            }
            else
            { 
                FileName = "http://localhost:" + porta + "/" + istanza.Name + "/EasyLook/Default.aspx";
            }
            Process.Start(FileName);

        }

        bool IsMago4(string version)
        {

            var versione = new Version(version);
            if (versione.Major == 1)
            {
                return true;
            }

            
            return false;

        }
        string GetSitePort (Instance istanza)
            {
                string tcpport = "80";
                string xmlfile = string.Empty;
                XmlDocument domdoc = new XmlDocument();
                
                xmlfile = Path.Combine (App.Instance.Settings.RootFolder, istanza.Name , @"Custom\ServerConnection.config");
                domdoc.Load(xmlfile);

                tcpport = domdoc.GetElementsByTagName("WebServicesPort")[0].Attributes["value"].Value.ToString();
      
                return tcpport;


            }

        public void OnUpdating(CmdLineInfo cmdLineInfo)
        {
            cmdLineInfo.ClassicApplicationPoolPipeline = false;
        }

        public void OnInstalling(CmdLineInfo cmdLineInfo)
        {
            cmdLineInfo.ClassicApplicationPoolPipeline = false;
        }
    }
    }



