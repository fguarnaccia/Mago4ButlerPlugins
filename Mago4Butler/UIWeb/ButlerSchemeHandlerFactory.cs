using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler
{
    internal class ButlerSchemeHandlerFactory : ISchemeHandlerFactory
    {
        const string butlerSchemeName = "butler";
        public string ButlerSchemeName { get { return butlerSchemeName; } }

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            if (schemeName == null)
            {
                throw new ArgumentNullException("scheme name is null");
            }
            if (string.IsNullOrWhiteSpace(schemeName))
            {
                throw new ArgumentException("Empty or white spaces scheme name");
            }
            if (string.Compare(schemeName, ButlerSchemeName, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                throw new Exception("Unknown scheme name: " + schemeName);
            }

            return new ButlerSchemeHandler();
        }
    }
}
