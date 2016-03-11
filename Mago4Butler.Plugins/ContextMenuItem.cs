using System;
using System.Windows.Forms;

namespace Microarea.Mago4Butler.Plugins
{
    public class ContextMenuItem
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public Action<Instance> Command { get; set; }
        public Keys ShortcutKeys { get; set; }
    }
}