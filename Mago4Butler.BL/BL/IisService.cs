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
                        ).ToList();

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

        public IEnumerable<WebSiteInfo> GetAvailableWebSites()
        {
            List<WebSiteInfo> webSites = new List<WebSiteInfo>();

            using (var serverManager = new ServerManager())
            {
                foreach (var site in serverManager.Sites)
                {
                    if (
                        site.State != ObjectState.Started ||
                        site.Bindings == null ||
                        site.Bindings.Count == 0
                        )
                    {
                        continue;
                    }

                    foreach (var bind in site.Bindings)
                    {
                        if (String.Compare(bind.Protocol, "http", StringComparison.OrdinalIgnoreCase) != 0)
                        {
                            continue;
                        }

                        string[] bindingTokens = bind.BindingInformation.Split(':');
                        //Il binding non esprime la porta.
                        if (bindingTokens == null || bindingTokens.Length < 2)
                            continue;

                        WebSiteInfo wsi = new WebSiteInfo()
                        {
                            SiteName = site.Name,
                            SiteID = (int)site.Id,
                            SitePort = Int32.Parse(bindingTokens[1])
                        };

                        webSites.Add(wsi);
                        break;
                    }
                }
            }

            return webSites;
        }
    }
}
