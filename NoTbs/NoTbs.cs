using Microarea.Mago4Butler.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.NoTbs
{
    public class NoTbs : Mago4ButlerPlugin
    {
        public override void OnInstalling(CmdLineInfo cmdLineInfo)
        {
            SetNoTbs(cmdLineInfo);
        }

        public override void OnUpdating(CmdLineInfo cmdLineInfo)
        {
            SetNoTbs(cmdLineInfo);
        }

        private void SetNoTbs(CmdLineInfo cmdLineInfo)
        {
            var q = from f in cmdLineInfo.Features
                    where string.Compare(f.Description, "TaskBuilder Studio", StringComparison.InvariantCultureIgnoreCase) == 0
                    select f;

            var tbsFeature = q.FirstOrDefault();

            if (tbsFeature != null)
            {
                cmdLineInfo.Features.Remove(tbsFeature);
            }
        }
    }
}
