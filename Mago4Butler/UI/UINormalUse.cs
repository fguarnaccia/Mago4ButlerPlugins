using AutoMapper;
using Microarea.Mago4Butler.BL;
using Microarea.Mago4Butler.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Microarea.Mago4Butler
{
    public partial class UINormalUse : UserControl, ILogger
    {
        MapperConfiguration config;
        IMapper mapper;

        Model model;
        PluginService pluginService;
        List<DoubleClickHandler> doubleClickHandlers = new List<DoubleClickHandler>();

        public event EventHandler<UpdateInstanceEventArgs> UpdateInstance;
        public event EventHandler<RemoveInstanceEventArgs> RemoveInstance;
        public event EventHandler<InstallInstanceEventArgs> InstallNewInstance;

        protected virtual void OnUpdateInstance(UpdateInstanceEventArgs e)
        {
            var handler = UpdateInstance;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnRemoveInstance(RemoveInstanceEventArgs e)
        {
            var handler = RemoveInstance;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnInstallNewInstance(InstallInstanceEventArgs e)
        {
            var handler = InstallNewInstance;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public UINormalUse(Model model, PluginService pluginService)
        {
            config = new MapperConfiguration(cfg => cfg.CreateMap<BL.Instance, Plugins.Instance>());
            mapper = config.CreateMapper();

            this.model = model;
            this.model.InstanceAdded += Model_InstanceAdded;
            this.model.InstanceRemoved += Model_InstanceRemoved;
            this.model.InstanceUpdated += Model_InstanceUpdated;

            this.pluginService = pluginService;

            InitializeComponent();
            InitContextMenus();
            InitDoubleClickHandlers();
        }

        private void InitContextMenus()
        {
            this.contextMenuStrip.Items.Clear();
            foreach (var plugin in this.pluginService.Plugins)
            {
                if (plugin != null)
                {
                    var contextMenuItems = plugin.GetContextMenuItems();
                    if (contextMenuItems != null && contextMenuItems.Count() > 0)
                    {
                        this.AddContextMenuItems(contextMenuItems); 
                    }
                }
            }
        }
        private void InitDoubleClickHandlers()
        {
            this.doubleClickHandlers.Clear();
            foreach (var plugin in this.pluginService.Plugins)
            {
                if (plugin != null)
                {
                    var doubleClickHandler = plugin.GetDoubleClickHandler();
                    if (doubleClickHandler != null)
                    {
                        this.doubleClickHandlers.Add(doubleClickHandler);
                    }
                }
            }
        }

        private void Model_InstanceUpdated(object sender, InstanceEventArgs e)
        {
            var idx = this.lsvInstances.Items.IndexOfKey(e.Instance.Name);
            if (idx < 0)
            {
                return;
            }

            var item = this.lsvInstances.Items[idx];
            if (item == null)
            {
                return;
            }
            item.Text = e.Instance.ToString();
            item.Tag = e.Instance;
        }

        private void Model_InstanceRemoved(object sender, InstanceEventArgs e)
        {
            this.lsvInstances.Items.RemoveByKey(e.Instance.Name);
        }

        private void Model_InstanceAdded(object sender, InstanceEventArgs e)
        {
            var instance = e.Instance;
            AddInstanceToListView(instance);
        }

        private void AddInstanceToListView(BL.Instance instance)
        {
            ListViewItem item = new ListViewItem(instance.Name);
            item.SubItems.Add(instance.Version.ToString());
            item.SubItems.Add(instance.InstalledOn.ToString("d MMM yyyy"));
            item.Tag = instance;

            var listViewItem = this.lsvInstances.Items.Add(item);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.lsvInstances.Items.Clear();
            foreach (var instance in this.model.Instances)
            {
                AddInstanceToListView(instance);
            }
        }

        private void lsvInstances_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnRemove.Enabled =
                this.btnUpdate.Enabled =
                this.lsvInstances.SelectedIndices != null && this.lsvInstances.SelectedIndices.Count > 0;

            var selectedItems = this.lsvInstances.SelectedItems;
            if (selectedItems == null || selectedItems.Count == 0)
            {
                return;
            }
            var instance = selectedItems[0].Tag as BL.Instance;

            foreach (ToolStripMenuItem item in this.contextMenuStrip.Items)
            {
                var handler = item.Tag as ContextMenuItemClickHandler;
                if (handler != null)
                {
                    handler.Instance = mapper.Map<Plugins.Instance>(instance);
                }
            }
        }

        private void btnAddInstance_Click(object sender, EventArgs e)
        {
            this.OnInstallNewInstance(new InstallInstanceEventArgs());
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            List<BL.Instance> selectedInstances = new List<BL.Instance>();
            foreach (ListViewItem selectedItem in this.lsvInstances.SelectedItems)
            {
                selectedInstances.Add(selectedItem.Tag as BL.Instance);
            }

            if (selectedInstances.Count > 0)
            {
                this.OnUpdateInstance(new UpdateInstanceEventArgs() { Instances = selectedInstances });
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            List<BL.Instance> selectedInstances = new List<BL.Instance>();
            foreach (ListViewItem selectedItem in this.lsvInstances.SelectedItems)
            {
                selectedInstances.Add(selectedItem.Tag as BL.Instance);
            }

            if (selectedInstances.Count > 0)
            {
                using (var form = new AskConfirmationForUninstallForm(selectedInstances))
                {
                    if (form.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                }
                this.OnRemoveInstance(new RemoveInstanceEventArgs() { Instances = selectedInstances });
            }
        }
        internal void AddContextMenuItems(IEnumerable<ContextMenuItem> contextMenuItems)
        {
            foreach (var contextMenuItem in contextMenuItems)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem();
                menuItem.Name = contextMenuItem.Name;
                menuItem.Text = contextMenuItem.Text;
                if (contextMenuItem.ShortcutKeys != Keys.None)
                {
                    menuItem.ShortcutKeys =  contextMenuItem.ShortcutKeys;
                    menuItem.ShowShortcutKeys = true;
                }

                ContextMenuItemClickHandler handler = new ContextMenuItemClickHandler() { ContextMenuItem = contextMenuItem };
                menuItem.Click += handler.MenuItem_Click;
                menuItem.Tag = handler;

                this.contextMenuStrip.Items.Add(menuItem);
            }
        }

        private void lsvInstances_DoubleClick(object sender, EventArgs e)
        {
            var pointClicked = this.lsvInstances.PointToClient(MousePosition);
            var lvi = this.lsvInstances.GetItemAt(pointClicked.X, pointClicked.Y);
            if (lvi == null)
            {
                return;
            }
            var instance = lvi.Tag as BL.Instance;

            var pluginInstance = mapper.Map<Plugins.Instance>(instance);

            foreach (var dch in this.doubleClickHandlers)
            {
                try
                {
                    dch.Command(pluginInstance);
                }
                catch (Exception exc)
                {
                    this.LogError(String.Concat("Error managing double click on instances for ", dch.Name), exc);
                }
            }
        }
    }
}
