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
        public T Get<T>()
        {
            return ioc.Get<T>();
        }

       public object Get(Type type)
        {
            return ioc.Get(type);
        }
    }
}
