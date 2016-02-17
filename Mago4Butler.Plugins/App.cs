using Microarea.Mago4Butler.BL;

namespace Microarea.Mago4Butler.Plugins
{
    public class App
    {
        static App _app;
        public static App Instance
        {
            get
            {
                if (_app == null)
                {
                    _app = new App();
                }
                return _app;
            }
        }
        protected App()
        {

        }

        public Plugins.Settings Settings
        {
            get
            {
                return new Settings() { RootFolder = Microarea.Mago4Butler.BL.Settings.Default.RootFolder };
            }
        }
    }
}
