using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Microarea.Mago4Butler
{
    internal class UIRunner : IForrest
    {
        public int Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = IoCContainer.Instance.Get<MainForm>();

            Application.Run(mainForm);

            return 0;
        }
    }
}