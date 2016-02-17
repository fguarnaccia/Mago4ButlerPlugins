using System;

namespace Microarea.Mago4Butler.Plugins
{
    public class ContextMenuItemClickHandler
    {
        public ContextMenuItem ContextMenuItem { get; set; }
        public Instance Instance { get; set; }

        public void MenuItem_Click(object sender, EventArgs e)
        {
            ContextMenuItem.Command(Instance);
        }
    }
}