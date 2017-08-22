using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler.Plugins
{
    public class ToolstripMenuItemClickHandler
    {
        public ToolstripMenuItem ToolstripMenuItem { get; set; }

        public void MenuItem_Click(object sender, EventArgs e)
        {
            ToolstripMenuItem.Command();
        }
    }
}
