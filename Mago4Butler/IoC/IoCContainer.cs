using Microarea.Mago4Butler.BL;
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
        class IoCModule : NinjectModule
        {
            public override void Load()
            {
                Bind<Model>()
                    .ToMethod(context =>
                    {
                        var settings = context.Kernel.Get<ISettings>();
                        var model = new Model();
                        model.Init(settings.RootFolder);

                        return model;
                    })
                    .InSingletonScope();

                Bind<MsiService>().ToSelf();
                Bind<InstallerService>().ToSelf();
                Bind<ProvisioningService>().ToSelf();
                Bind<FileSystemService>().ToSelf();
                Bind<IisService>().ToSelf();
                Bind<MsiZapper>().ToSelf();
                Bind<RegistryService>().ToSelf();

                Bind<ISettings>().ToMethod(context => Settings.Default);

                Bind<MainForm>().ToSelf();
                Bind<UIEmpty>().ToSelf();
                Bind<UIWaiting>().ToSelf();
                Bind<UINormalUse>().ToSelf();
                Bind<UIError>().ToSelf();

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
            ioc.Load<IoCModule>();
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
    }
}
