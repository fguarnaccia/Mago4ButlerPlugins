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