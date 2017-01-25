using Ninject;
using System;

namespace Microarea.Mago4Butler
{
    public class MainUIFactory
    {
        IKernel ioc;

        public MainUIFactory(IKernel ioc)
        {
            this.ioc = ioc;
        }
        public IMainUI CreateMainUI()
        {
            //var svc = this.ioc.Get<ShouldUseProvisioningProvider>();

            //IMainUI mainUI = svc.ShouldUseProvisioning
            //    ?
            //    this.ioc.Get<MainForm>() as IMainUI
            //    :
            //    this.ioc.Get<CefForm>()
            //    ;

            //return mainUI;

            return this.ioc.Get<MainForm>();
        }
    }
}