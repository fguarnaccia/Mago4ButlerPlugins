using System;
using System.Linq;
using System.Collections.Generic;

namespace Microarea.Mago4Butler.BL
{
    public class WebSiteInfo
    {
        static WebSiteInfo defaultWebSite;
        public string SiteName { get; set; }
        public int SiteID { get; set; }
        public int SitePort { get; set; }

        public static WebSiteInfo DefaultWebSite
        {
            get
            {
                if (defaultWebSite == null)
                {
                    defaultWebSite = new WebSiteInfo()
                    {
                        SiteID = 0,
                        SiteName = "Default Web Site",
                        SitePort = 80
                    };
                }
                return defaultWebSite;
            }
        }
    }
}