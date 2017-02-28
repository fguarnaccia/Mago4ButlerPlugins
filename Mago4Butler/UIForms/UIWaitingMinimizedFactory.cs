using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler
{
    public class UIWaitingMinimizedFactory
    {
        public UIWaitingMinimized CreateWaitingWindow()
        {
            return new UIWaitingMinimized();
        }
    }
}
