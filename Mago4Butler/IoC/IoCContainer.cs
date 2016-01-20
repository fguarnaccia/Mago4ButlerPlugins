using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler
{
    public class IoCContainer
    {
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
