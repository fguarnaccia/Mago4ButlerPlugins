using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    public class IisService
    {
        public void RemoveApplicationPools(Instance instance)
        {
            using (var mgr = new ServerManager())
            {
                var appPools = mgr.ApplicationPools;
                var appPoolsToRemove = appPools.Where(
                    pool =>
                        pool.Name.StartsWith(String.Format(CultureInfo.InvariantCulture, "MA_{0}", instance.Name), StringComparison.InvariantCultureIgnoreCase)
                        );

                foreach (var appPool in appPoolsToRemove)
                {
                    appPools.Remove(appPool);
                }
                mgr.CommitChanges();
            }
        }

        public void RemoveVirtualFoldersAndApplications(Instance instance)
        {
            using (var mgr = new ServerManager())
            {
                var site = mgr.Sites[instance.WebSiteInfo.SiteName];

                var applicationCollection = site.Applications;

                var rootPath = String.Concat("/", instance.Name);

                var applications = applicationCollection.FindApplications(rootPath);

                foreach (var application in applications)
                {
                    applicationCollection.Remove(application);
                }

                var rootApplication = applicationCollection.FindApplication("/");
                if (rootApplication != null)
                {
                    var virtualDirCollection = rootApplication.VirtualDirectories;
                    var instanceVirtualDir = virtualDirCollection.FindVirtualDirectory(rootPath);
                    if (instanceVirtualDir != null)
                    {
                        rootApplication.VirtualDirectories.Remove(instanceVirtualDir);
                    }
                }

                mgr.CommitChanges();
            }
        }
    }

}
