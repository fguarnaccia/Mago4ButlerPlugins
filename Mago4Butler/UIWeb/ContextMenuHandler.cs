using CefSharp;
using Microarea.Mago4Butler.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microarea.Mago4Butler
{
    public class ContextMenuHandler : IContextMenuHandler
    {
        const int ShowDevTools = 26501;
        const int CloseDevTools = 26502;

        PluginService pluginService;
        Dictionary<CefMenuCommand, ContextMenuItemClickHandler> commands = new Dictionary<CefMenuCommand, ContextMenuItemClickHandler>();

        public ContextMenuHandler(PluginService pluginService)
        {
            this.pluginService = pluginService;
        }
        public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            model.Clear();
            commands.Clear();
            foreach (var plugin in this.pluginService.Plugins)
            {
                if (plugin != null)
                {
                    var contextMenuItems = plugin.GetContextMenuItems();
                    if (contextMenuItems != null && contextMenuItems.Count() > 0)
                    {
                        this.AddContextMenuItems(contextMenuItems, model);
                    }
                }
            }
            model.AddItem((CefMenuCommand)ShowDevTools, "Show DevTools");
            model.AddItem((CefMenuCommand)CloseDevTools, "Close DevTools");
        }
        internal void AddContextMenuItems(IEnumerable<ContextMenuItem> contextMenuItems, IMenuModel model)
        {
            int i = 3;
            foreach (var contextMenuItem in contextMenuItems)
            {
                ContextMenuItemClickHandler handler = new ContextMenuItemClickHandler() { ContextMenuItem = contextMenuItem };

                model.AddItem(CefMenuCommand.UserFirst + i, contextMenuItem.Text);
                commands.Add(CefMenuCommand.UserFirst + i, handler);

                i += 1;
            }
        }

        public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            ContextMenuItemClickHandler handler = null;
            if (commands.TryGetValue(commandId, out handler))
            {
                handler.MenuItem_Click(this, EventArgs.Empty);
                return true;
            }
            if ((int)commandId == ShowDevTools)
            {
                browser.ShowDevTools();
            }
            if ((int)commandId == CloseDevTools)
            {
                browser.CloseDevTools();
            }
            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
            
        }

        public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }
}
