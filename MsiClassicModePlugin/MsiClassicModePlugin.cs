using System;
using System.Collections.Generic;
using Microarea.Mago4Butler.Plugins;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MsiClassicModePlugin
{
    public class MsiClassicMode : Mago4ButlerPlugin
    {

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

            //foreach (var description in Properties.Settings.Default.KeepFeatures)
            //{
            //    var feature = cmdLineInfo
            //        .Features
            //        .Where(f => String.Compare(f.Description, description, StringComparison.InvariantCultureIgnoreCase) == 0)
            //        .FirstOrDefault();
            //    if (feature == null )
            //    {
            //        cmdLineInfo.Features.Remove(feature);
            //    }
            //}

            if (IsMago4Setup())
            {
                var clonedCollection = new List<Feature>(cmdLineInfo.Features);

                foreach (Feature feature in clonedCollection)
                {
                    if (!Properties.Settings.Default.KeepFeatures.Contains(feature.Description))
                    {
                        cmdLineInfo.Features.Remove(feature);
                    }
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
        bool IsMago4Setup()
        {

            return true;
        }


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
    }


    }

