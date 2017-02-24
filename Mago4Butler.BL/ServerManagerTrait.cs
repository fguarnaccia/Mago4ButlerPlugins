using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microarea.Mago4Butler.BL
{
    public static class ServerManagerTrait
    {
        public static Application FindApplication(this ApplicationCollection @this, string applicationPath)
        {
            var query = from application in @this
                        where String.Compare(application.Path, applicationPath, StringComparison.InvariantCultureIgnoreCase) == 0
                        select application
            ;

            return query.FirstOrDefault();
        }

        public static IEnumerable<Application> FindApplications(this ApplicationCollection @this, string applicationPathQuery)
        {
            //hack per impedire che in caso di istanze coesistenti con nome del tipo M4_1_3 e M4_1_3_1
            //vengano prese in considerazione anche le applicazioni di M4_1_3_1 quando si rimuove solo M4_1_3
            if (!applicationPathQuery.EndsWith("/", StringComparison.InvariantCultureIgnoreCase))
            {
                applicationPathQuery += "/";
            }
            var query = from application in @this
                        where application.Path.StartsWith(applicationPathQuery, StringComparison.InvariantCultureIgnoreCase)
                        select application
                        ;

            return query.ToList();
        }

        public static VirtualDirectory FindVirtualDirectory(this VirtualDirectoryCollection @this, string virtualDirPath)
        {
            var query = from virtualDir in @this
                        where String.Compare(virtualDir.Path, virtualDirPath, StringComparison.InvariantCultureIgnoreCase) == 0
                        select virtualDir
                        ;

            return query.FirstOrDefault();
        }
    }
}