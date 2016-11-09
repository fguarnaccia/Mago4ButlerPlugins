using Ninject;

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
            return this.ioc.Get<IMainUI>();
        }
    }
}