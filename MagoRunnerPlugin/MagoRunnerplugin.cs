using Microarea.Mago4Butler.Plugins;
using System.Xml;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Linq;
namespace MagoRunnerPlugin
{
    public class MagoRunner : Mago4ButlerPlugin
    {
        string root = string.Empty;
        string instance = string.Empty;
       
  
        public override IEnumerable<ContextMenuItem> GetContextMenuItems()
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

            #region Set Title
            //cmi = new ContextMenuItem();
            //cmi.Name = "runSetTitle";
            //cmi.Text = "set as Title (CapsLock to restore)";
            //cmi.ShortcutKeys = Keys.Control | Keys.T;
            //cmi.Command = new Action<Instance>(
            //    (instance)
            //    =>
            //    RunSetTitle(instance)
            //    );
            //cmis.Add(cmi);
            #endregion

            #region Madico
            cmi = new ContextMenuItem();
            cmi.Name = "runMadico";
            cmi.Text = "...Check Instance";
            cmi.ShortcutKeys = Keys.Control | Keys.M;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunMadico(instance)
                );
            cmis.Add(cmi);
            #endregion

            #region COD
            cmi = new ContextMenuItem();
            cmi.Name = "runCOD";
            cmi.Text = "...ClickOnceDeployer";
            cmi.ShortcutKeys = Keys.Control | Keys.O;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunCOD(instance)
                );
            cmis.Add(cmi);
            #endregion

            #region Mago
            cmi = new ContextMenuItem();
            cmi.Name = "runMago";
            cmi.Text = "...Mago";
            cmi.ShortcutKeys = Keys.F9;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunMago(instance)
                );
            cmis.Add(cmi);
            #endregion

            #region Console
            cmi = new ContextMenuItem();
            cmi.Name = "runConsole";
            cmi.Text = "...Admin Console";
            cmi.ShortcutKeys = Keys.Control | Keys.A;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunConsole(instance)
                );
            cmis.Add(cmi);
            #endregion

            #region Custom
            cmi = new ContextMenuItem();
            cmi.Name = "runCustom";
            cmi.Text = "...Custom";
            cmi.ShortcutKeys = Keys.Control | Keys.C;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunCustom(instance)
                );
            cmis.Add(cmi);
            #endregion

            #region Publish
            cmi = new ContextMenuItem();
            cmi.Name = "runPublish";
            cmi.Text = "...Publish";
            cmi.ShortcutKeys = Keys.Control | Keys.P;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunPublish(instance)
                );
            cmis.Add(cmi);
            #endregion

            #region Standard
            cmi = new ContextMenuItem();
            cmi.Name = "runStandard";
            cmi.Text = "...Standard";
            cmi.ShortcutKeys = Keys.Control | Keys.S;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunStandard(instance)
                );
            cmis.Add(cmi);
            #endregion

            #region Web
            cmi = new ContextMenuItem();
            cmi.Name = "runMagoWeb";
            cmi.Text = "...Easylook/MagoWeb";
            cmi.ShortcutKeys = Keys.Control | Keys.W;
            cmi.Command = new Action<Instance>(
                (instance)
                =>
                RunMagoWeb(instance)
                );
            cmis.Add(cmi); 
            #endregion

            return cmis;
        
        }

     
        private void RunSetTitle(Instance instance)
        {
            string BrandMn = @"Standard\Applications\ERP\Solutions\main.brand.xml";
            string BrandM4Pro = @"Standard\Applications\ERP\Solutions\Mago4-Pro.Brand.xml";
            string BrandM4Ent = @"Standard\Applications\ERP\Solutions\Mago4-Ent.Brand.xml";
                     
            string xmlfile = string.Empty;
            bool RestoreDefault = Control.IsKeyLocked(Keys.CapsLock);
        
            if (instance.ProductType == Microarea.Mago4Butler.Model.ProductType.Mago4)
            {
                
                xmlfile = Path.Combine(App.Instance.Settings.RootFolder, instance.Name, BrandM4Pro);
                SetM4Title(xmlfile, instance.Name, RestoreDefault);
                xmlfile = Path.Combine(App.Instance.Settings.RootFolder, instance.Name, BrandM4Ent);
                SetM4Title(xmlfile, instance.Name, RestoreDefault);

            }
            else
            {
                xmlfile = Path.Combine(App.Instance.Settings.RootFolder, instance.Name, BrandMn);              
                SetMnTitle(xmlfile, instance.Name, RestoreDefault);

            }

        }

        internal void SetM4Title(string BrandFile, string Title, bool RestoreDefaultTitle)

        {
            if (!File.Exists(BrandFile)) return;
            const string cstBrandM4 = "Mago4";
            if (RestoreDefaultTitle) Title = cstBrandM4;
       
            XElement xe = XElement.Load(BrandFile);

            var brands =
                from el in xe.Descendants("BrandedKey")

                where el.Attribute("source").Value == "M4"
               select el;

            var element = brands.FirstOrDefault();
            element.Attribute("branded").Value = Title;

            xe.Save(BrandFile);
        }

        internal void SetMnTitle(string BrandFile, string Title, bool RestoreDefaultTitle)
        {
            const string cstBrandMn = "Mago.net";

            XmlDocument domdoc = new XmlDocument();
            if (!File.Exists(BrandFile)) return;

            domdoc.Load(BrandFile);

            if (RestoreDefaultTitle) Title = cstBrandMn;
           
            domdoc.Load(BrandFile);
            if (domdoc.GetElementsByTagName("BrandedKeys")[0].FirstChild.Attributes.GetNamedItem("source").Value == "MN")
                domdoc.GetElementsByTagName("BrandedKeys")[0].FirstChild.Attributes.GetNamedItem("branded").Value = Title;

            domdoc.Save(BrandFile);

        }
        private void RunNothing(Instance instance)
        {
            return;
        }

        
        public override DoubleClickHandler GetDoubleClickHandler()
        {

            var dch = new DoubleClickHandler() { Name = "MagoRunnerPlugin.Doubleclick" };
            dch.Command = (instance) => RunMago(instance);
            return dch;
                 
        }


        void RunRoot(Instance istanza)
        {

            string FileName = string.Empty;
            FileName = Path.Combine(App.Instance.Settings.RootFolder,istanza.Name);

            Process.Start(FileName);

        }
     
        void RunMago(Instance istanza)
        {

            string fileName = string.Empty;
            string executable = string.Empty;

            RunSetTitle(istanza);


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
            string arguments = "/autologin yes";

            if (IsMago4(istanza.Version))

            {
                executable = "AdministrationConsole.exe";
     
            }
            else
            {
                executable = "MicroareaConsole.exe";

            }
         
            fileName = Path.Combine(App.Instance.Settings.RootFolder, istanza.Name, @"Apps\Publish\", executable);

            Process.Start(fileName,arguments);

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
                FileName = "http://localhost:" + porta + "/" + istanza.Name + "/M4Client/index.html";

            }
            else
            { 
                FileName = "http://localhost:" + porta + "/" + istanza.Name + "/EasyLook/Default.aspx";
            }
            Process.Start(FileName);

        }
        void RunMadico(Instance istanza)
        {

            string fileName = string.Empty;
            string executable = "Madico.msc";

            fileName = Path.Combine(App.Instance.Settings.RootFolder, istanza.Name, @"Apps\Microarea Diagnostics\", executable);
            Process.Start(fileName, "ui");
        }

        void RunCOD(Instance istanza)
        {
            string fileName = string.Empty;
            string executable = "ClickOnceDeployer.exe";

            fileName = Path.Combine(App.Instance.Settings.RootFolder, istanza.Name, @"Apps\ClickOnceDeployer\", executable);
            Process.Start(fileName, "ui");
        }

        bool IsMago4(string version)
        {

            var versione = new Version(version);
            if (versione.Major < 3)
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





    }
}



