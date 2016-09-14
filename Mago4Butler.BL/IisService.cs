using Microarea.Mago4Butler.Log;
using Microarea.Mago4Butler.Model;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.BL
{
    public class IisService : ILogger
    {
        readonly ISettings settings;

        public IisService(ISettings settings)
        {
            this.settings = settings;
        }
        public void RemoveApplicationPools(Instance instance)
        {
            try
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
            catch (Exception exc)
            {
                this.LogError("Error removing application pool for " + instance.Name, exc);
            }
        }

        public void RemoveVirtualFoldersAndApplications(Instance instance)
        {
            try
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
            catch (Exception exc)
            {
                this.LogError("Error removing virtual folders and applications for " + instance.Name, exc);
            }
        }

        public IEnumerable<WebSiteInfo> GetAvailableWebSites()
        {
            List<WebSiteInfo> webSites = new List<WebSiteInfo>();

            try
            {
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
            }
            catch (Exception exc)
            {
                this.LogError("Error getting web sites", exc);
            }

            return webSites;
        }

        public void RestartLoginManager(Instance instance)
        {
            var webConfigFullPath = Path.Combine(this.settings.RootFolder, instance.Name, "Standard", "TaskBuilder", "WebFramework", "LoginManager", "web.config");

            if (File.Exists(webConfigFullPath))
            {
                try
                {
                    Process.Start("cmd", string.Format(CultureInfo.InvariantCulture, @"/C attrib -r {0}", webConfigFullPath));

                    Thread.Sleep(500);
                    Process.Start("cmd", string.Format(CultureInfo.InvariantCulture, @"/C copy /Y /B {0}+,, {0}", webConfigFullPath));

                    Thread.Sleep(500);
                    Process.Start("cmd", string.Format(CultureInfo.InvariantCulture, @"/C attrib +r {0}", webConfigFullPath));
                }
                catch (Exception exc)
                {
                    this.LogError("Exception restarting login manager for " + instance.Name, exc);
                }
            }
            else
            {
                this.LogInfo("Unable to restart login manager for " + instance.Name + " because the file " + webConfigFullPath + " does not exist");
            }
        }
    }
}
