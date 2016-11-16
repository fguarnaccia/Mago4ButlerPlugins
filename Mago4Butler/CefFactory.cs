using CefSharp;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler
{
    public class CefFactory
    {
        IKernel ioc;

        public CefFactory(IKernel ioc)
        {
            this.ioc = ioc;
        }
        public IContextMenuHandler CreateContextMenuHandler()
        {
            return new ContextMenuHandler(this.ioc.Get<PluginService>());
        }

        public ISchemeHandlerFactory CreateSchemeHandlerFactory()
        {
            return new ButlerSchemeHandlerFactory();
        }
    }
}
