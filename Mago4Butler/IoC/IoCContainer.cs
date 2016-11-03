using Microarea.Mago4Butler.BL;
using Microarea.Mago4Butler.Log;
using Microarea.Mago4Butler.Model;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler
{
    public class IoCContainer
    {
        IoCModule iocModule;
        class IoCModule : NinjectModule
        {
            public override void Load()
            {
                Bind<Model.Model>()
                    .ToMethod(context =>
                    {
                        var settings = context.Kernel.Get<ISettings>();
                        var model = new Model.Model(settings.RootFolder);
                        model.Init();

                        return model;
                    })
                    .InSingletonScope();

                Bind<HttpService>().ToSelf();
                Bind<ShouldUseProvisioningProvider>()
                    .ToSelf()
                    .InSingletonScope();

                Bind<UpdatesDownloaderService>().ToSelf();
                Bind<MsiService>().ToSelf();
                Bind<WcfService>().ToSelf();
                Bind<CompanyDBUpdateService>().ToSelf();
                Bind<FileSystemService>().ToSelf();
                Bind<IisService>().ToSelf();
                Bind<MsiZapper>().ToSelf();
                Bind<RegistryService>().ToSelf();
                Bind<LoggerService>().ToSelf();
                Bind<PluginService>()
                    .ToSelf()
                    .InSingletonScope();
                Bind<InstallerService>()
                    .ToSelf()
                    .InSingletonScope();

                Bind<ISalesModulesConfiguratorService>()
                    .To<SalesModulesConfiguratorService>()
                    .When(
                    (r) =>
                    {
                        var svc = r.ParentContext.Kernel.Get<ShouldUseProvisioningProvider>();
                        return svc.ShouldUseProvisioning;
                    });
                Bind<ISalesModulesConfiguratorService>()
                    .To<NoSalesModulesConfigurationService>()
                    .When(
                    (r) =>
                    {
                        var svc = r.ParentContext.Kernel.Get<ShouldUseProvisioningProvider>();
                        return !svc.ShouldUseProvisioning;
                    });

                Bind<IProvisioningService>()
                    .To<Mago4ProvisioningService>()
                    .Named("mago4");
                Bind<IProvisioningService>()
                    .To<NoProvisioningService>()
                    .Named("mago.net");
                Bind<IProvisioningService>()
                    .To<NoProvisioningService>()
                    .Named(string.Empty);

                Bind<IAdmConsoleToLaunchNameProvider>()
                    .To<AdmConsoleToLaunchNameProvider>();

                Bind<ISettings>().ToMethod(context => Settings.Default);

                Bind<IUIMediator>().To<UIMediator>();

                Bind<AboutForm>().ToSelf();
                Bind<SettingsForm>().ToSelf();
                Bind<AskForParametersForm>().ToSelf();
                Bind<UIEmpty>().ToSelf();
                Bind<UIWaiting>().ToSelf();
                Bind<UIWaitingMinimized>().ToSelf();
                Bind<UINormalUse>().ToSelf();
                Bind<UIError>().ToSelf();

                Bind<ContextMenuHandler>().ToSelf();
                Bind<ButlerSchemeHandlerFactory>().ToSelf();

                //Bind<System.Windows.Forms.Form>().To<MainForm>();
                Bind<System.Windows.Forms.Form>()
                    .To<MainForm>()
                    .When(
                    (r) =>
                    {
                        var svc = this.Kernel.Get<ShouldUseProvisioningProvider>();
                        return svc.ShouldUseProvisioning;
                    });
                Bind<System.Windows.Forms.Form>()
                    .To<CefForm>()
                    .When(
                    (r) =>
                    {
                        var svc = this.Kernel.Get<ShouldUseProvisioningProvider>();
                        return !svc.ShouldUseProvisioning;
                    });


                Bind<Batch>().ToSelf();

                Bind<IForrest>()
                    .To<UIRunner>()
                    .When(
                    (r) =>
                    {
                        var parameters = r.Parameters;
                        if (parameters == null || parameters.Count == 0)
                        {
                            return true;
                        }

                        return false;
                    });
                Bind<IForrest>()
                    .To<ConsoleRunner>()
                    .When(
                    (r) =>
                    {
                        var parameters = r.Parameters;
                        if (parameters == null || parameters.Count == 0)
                        {
                            return false;
                        }
                        return true;
                    });
            }
        }

        static IoCContainer instance;
        public static IoCContainer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IoCContainer();
                }
                return instance;
            }
        }
        private IoCContainer()
        {
            ioc = new StandardKernel();
            this.iocModule = new Mago4Butler.IoCContainer.IoCModule();
            ioc.Load(this.iocModule);
        }
        IKernel ioc;
        public T Get<T>(params Parameter[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return ioc.Get<T>();
            }
            var injectParams = new List<Ninject.Parameters.IParameter>();
            foreach (var parameter in parameters)
            {
                if (parameter != null)
                {
                    injectParams.Add(new Ninject.Parameters.ConstructorArgument(parameter.Name, parameter.Value));
                }
            }
            return ioc.Get<T>(injectParams.ToArray());
        }
        public T Get<T>(string name, params Parameter[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return ioc.Get<T>(name);
            }
            var injectParams = new List<Ninject.Parameters.IParameter>();
            foreach (var parameter in parameters)
            {
                if (parameter != null)
                {
                    injectParams.Add(new Ninject.Parameters.ConstructorArgument(parameter.Name, parameter.Value));
                }
            }
            return ioc.Get<T>(name, injectParams.ToArray());
        }

        public IProvisioningService GetProvisioningService(string productName)
        {
            IProvisioningService provisioningService = null;
            var shouldUseProvisioningProvider = IoCContainer.Instance.Get<ShouldUseProvisioningProvider>();
            if (shouldUseProvisioningProvider.ShouldUseProvisioning)
            {
                provisioningService = IoCContainer.Instance.Get<IProvisioningService>(productName);
            }
            else
            {
                provisioningService = IoCContainer.Instance.Get<IProvisioningService>(string.Empty);
            }

            return provisioningService;
        }
    }
}
