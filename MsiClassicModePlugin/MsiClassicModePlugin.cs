﻿using System.Collections.Generic;
using Microarea.Mago4Butler.Plugins;
using System.Diagnostics;
using System.Threading.Tasks;
using System;
using System.ComponentModel;

namespace MsiClassicModePlugin
{
    public class MsiClassicMode : Mago4ButlerPlugin
    {

        //List<Feature> FixedFeatures = new List<Feature>() {
        //    new Feature() { Description = "Mago4" },
        //    new Feature() {  Description = "MagoNet" } ,
        //    new Feature() { Description = "Italian (Italy)" },
        //    new Feature() { Description = "Language Packages" },
        //     new Feature() { Description = "TaskBuilder Framework" },
        //};

        System.Collections.Specialized.StringCollection FixedFeatures = new System.Collections.Specialized.StringCollection()
        {
           "Mago4" , "Mago.net" , "Italian (Italy)", "Language Packages" , "TaskBuilder Framework"

        };


        static MsiClassicMode()
        {
            if (Properties.Settings.Default.UpgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
                         
            }

        #if DEBUG 

            Properties.Settings.Default.Reset();

        #endif

                
        }

        public enum ProductSignature
        {
            [Description("MagoNet-Pro")]
            MagoNetPro,
            M4GO
            
        }
       
        public override void OnApplicationStarted()
        {
            //stimola il WS per accelerare la risposta quando richiesto
            MABuilds.UpdatesService ccnet = new MABuilds.UpdatesService();    
            Task.Factory.StartNew(() => ccnet.GetNightlyBuilds());

            
        }
        public override void OnInstalling(CmdLineInfo cmdLineInfo)
        {

            SetClassicModeParameter(cmdLineInfo);
            
        }

        public override void OnUpdating(CmdLineInfo cmdLineInfo)
        {
            SetClassicModeParameter(cmdLineInfo);

        }

        private void SetClassicModeParameter(CmdLineInfo cmdLineInfo)
        {

            cmdLineInfo.ClassicApplicationPoolPipeline = Properties.Settings.Default.ClassicApplicationPoolPipeline;
            cmdLineInfo.NoEveryone = Properties.Settings.Default.NoEveryone;
            cmdLineInfo.NoShares = Properties.Settings.Default.NoShares;
            cmdLineInfo.NoShortcuts = Properties.Settings.Default.NoShortcuts;
            cmdLineInfo.SkipClickOnceDeployer = Properties.Settings.Default.SkipClickOnceDeployer;
            cmdLineInfo.NoEnvVar = Properties.Settings.Default.NoEnvVar;

            //    var feature = cmdLineInfo
            //        .Features
            //        .Where(f => String.Compare(f.Description, description, StringComparison.InvariantCultureIgnoreCase) == 0)
            //        .FirstOrDefault();     


                var clonedCollection = new List<Feature>(cmdLineInfo.Features);

                foreach (Feature feature in clonedCollection)
                {
                    if (!Properties.Settings.Default.KeepFeatures.Contains(feature.Description))
                    {

                        if (FixedFeatures.Contains(feature.Description)) continue;

                             cmdLineInfo.Features.Remove(feature);
                    }
                }

        }  

        void StopSharedFolder(Instance istanza)
        {
          
            string customshare = "share {0}_Custom {1}";
            string standardshare = "share {0}_Standard {1}";

            customshare = string.Format(customshare, istanza.Name, "/Delete");
            standardshare = string.Format(standardshare, istanza.Name, "/Delete");
            DeleteShare(customshare);
            DeleteShare(standardshare);


        }
        //bool IsMago4Setup(CmdLineInfo cmdLineInfo)
        //{
        //    bool ismago4setup = false; 
        //    foreach (Feature feature in cmdLineInfo.Features)

        //    {
             
        //        if (feature.Description.ToString() == "Mago4")
        //        {
        //            ismago4setup =  true;
        //        }
        //        else
        //        {
        //            ismago4setup =  false;
        //        }
        //    }

        //    return ismago4setup;
        //}

        void DeleteShare(string sharestring)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "net";
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.Arguments = sharestring;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.Start();

        }
        public override void OnRemoving(Instance[] instances)
        {

            foreach (Instance istanza in instances)
            {
                string str = istanza.Name;

                StopSharedFolder(istanza);

            }
        }
        
        public override bool ShouldUseProvisioning()
        {
            return false;
        }
        public override void OnAskForParametersForInstall(AskForParametersBag bag)
        {
            
            frmMsiClassicMode frm = new frmMsiClassicMode(false);

            frm.ShowDialog();

            if (frm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                bag.InstanceName = frm.txtInstanceName.Text;
                bag.MsiFullFilePath = frm.txtboxFileMsi.Text;

            }
            else
            {
                bag.Cancel = true;
            }
        }

        public override void OnAskForParametersForUpdate(AskForParametersBag bag)
        {
            frmMsiClassicMode frm = new frmMsiClassicMode(true);

            frm.txtInstanceName.Text = "don't worry";
            frm.ShowDialog();
            if (frm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {

                bag.MsiFullFilePath = frm.txtboxFileMsi.Text;
               
            }
            else
            {
                bag.Cancel = true;
            }
        }

        public bool IsMago4(string version)
        {

            var versione = new Version(version);
            if (versione.Major == 1)
            {
                return true;
            }

            return false;

        }

        public ProductSignature Signature(string version)
        {

          
            if (IsMago4(version))
                return ProductSignature.M4GO;
            else
                return ProductSignature.MagoNetPro;

        }
    }
}

