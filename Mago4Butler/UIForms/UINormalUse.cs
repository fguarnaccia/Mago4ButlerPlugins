using AutoMapper;
using Microarea.Mago4Butler.BL;
using Microarea.Mago4Butler.Log;
using Microarea.Mago4Butler.Model;
using Microarea.Mago4Butler.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Microarea.Mago4Butler
{
    public partial class UINormalUse : UserControl, ILogger
    {
        MapperConfiguration config;
        IMapper mapper;

        Model.Model model;
        PluginService pluginService;
        InstallerService installerService;
        List<DoubleClickHandler> doubleClickHandlers = new List<DoubleClickHandler>();
        ListViewSortManager listViewSortManager;

        ISettings settings;

        SynchronizationContext syncCtx;

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

        public UINormalUse(Model.Model model, PluginService pluginService, InstallerService installerService, ISettings settings)
        {
            this.syncCtx = SynchronizationContext.Current;
            if (this.syncCtx == null)
            {
                this.syncCtx = new WindowsFormsSynchronizationContext();
            }

            config = new MapperConfiguration(cfg => cfg.CreateMap<Model.Instance, Plugins.Instance>());
            mapper = config.CreateMapper();

            this.model = model;
            this.model.InstanceAdded += Model_InstanceAdded;
            this.model.InstanceRemoved += Model_InstanceRemoved;
            this.model.InstanceUpdated += Model_InstanceUpdated;
            this.model.ModelInitialized += Model_ModelInitialized;

            this.pluginService = pluginService;
            this.installerService = installerService;
            this.installerService.Installed += InstallerService_Installed;

            this.settings = settings;

            InitializeComponent();
            InitContextMenus();
            InitDoubleClickHandlers();

            listViewSortManager = new ListViewSortManager(this.lsvInstances, this.settings);
        }

        private void InstallerService_Installed(object sender, InstallInstanceEventArgs e)
        {
            this.syncCtx.Post(new SendOrPostCallback((obj) =>
            {
                try
                {
                    this.UpdateVersionAndTooltip(e.Instance);
                }
                catch
                {}
            })
            , null);
        }

        private void UpdateVersionAndTooltip(Model.Instance instance)
        {
            var idx = this.lsvInstances.Items.IndexOf(instance);
            if (idx < 0)
            {
                return;
            }
            var item = this.lsvInstances.Items[idx];
            if (item == null)
            {
                return;
            }

            var instanceDirInfo = new DirectoryInfo(Path.Combine(this.settings.RootFolder, instance.Name, "Standard"));
            instance.Version = Model.Instance.FromStandardDirectoryInfo(instanceDirInfo).Version;
            item.SubItems[1].Text = instance.Version.ToString();

            if (instance.Edition != Edition.None)
            {
                item.ToolTipText = string.Concat(instance.Edition.ToString(), " Edition");
            }
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

        private void Model_ModelInitialized(object sender, EventArgs e)
        {
            InitInstancesListView();
        }

        private void Model_InstanceUpdated(object sender, InstanceEventArgs e)
        {
            var idx = this.lsvInstances.Items.IndexOf(e.Instance);
            if (idx < 0)
            {
                return;
            }

            var item = this.lsvInstances.Items[idx];
            if (item == null)
            {
                return;
            }
            item.Tag = e.Instance;

            item.Text = e.Instance.Name;
            item.SubItems[1].Text = e.Instance.Version.ToString();
            item.SubItems[2].Text = e.Instance.InstalledOn.ToString("d MMM yyyy HH:mm");
        }

        private void Model_InstanceRemoved(object sender, InstanceEventArgs e)
        {
            int idx = this.lsvInstances.Items.IndexOf(e.Instance);
            if (idx > -1)
            {
                this.lsvInstances.Items.RemoveAt(idx);
            }
        }

        private void Model_InstanceAdded(object sender, InstanceEventArgs e)
        {
            var instance = e.Instance;
            AddInstanceToListView(instance);
        }

        private void AddInstanceToListView(Model.Instance instance)
        {
            ListViewItem item = new ListViewItem(instance.Name);
            item.SubItems.Add(instance.Version.ToString());
            item.SubItems.Add(instance.InstalledOn.ToString("d MMM yyyy HH:mm"));
            item.Tag = instance;

            if (instance.Edition != Edition.None)
            {
                item.ToolTipText = string.Concat(instance.Edition.ToString(), " Edition");
            }

            var listViewItem = this.lsvInstances.Items.Add(item);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitInstancesListView();

            this.listViewSortManager.SortByColumn(this.settings.LastColumnSortedIndex);
        }

        private void InitInstancesListView()
        {
            this.lsvInstances.Items.Clear();
            foreach (var instance in this.model.Instances)
            {
                AddInstanceToListView(instance);
            }
            foreach (ColumnHeader column in this.lsvInstances.Columns)
            {
                column.Width = -1;
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
            var instance = selectedItems[0].Tag as Model.Instance;

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
            var selectedInstances = new List<Model.Instance>();
            foreach (ListViewItem selectedItem in this.lsvInstances.SelectedItems)
            {
                selectedInstances.Add(selectedItem.Tag as Model.Instance);
            }

            if (selectedInstances.Count > 0)
            {
                this.OnUpdateInstance(new UpdateInstanceEventArgs() { Instances = selectedInstances.ToArray() });
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var selectedInstances = new List<Model.Instance>();
            foreach (ListViewItem selectedItem in this.lsvInstances.SelectedItems)
            {
                selectedInstances.Add(selectedItem.Tag as Model.Instance);
            }

            if (selectedInstances.Count > 0)
            {
                using (var form = new AskConfirmationForUninstallForm(selectedInstances))
                {
                    if (form.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }
                }
                this.OnRemoveInstance(new RemoveInstanceEventArgs() { Instances = selectedInstances.ToArray() });
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
            var instance = lvi.Tag as Model.Instance;

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

    internal static class ListViewItemCollectionExt
    {
        public static int IndexOf(this ListView.ListViewItemCollection @this, Model.Instance instance)
        {
            int idx = -1;
            for (int i = 0; i < @this.Count; i++)
            {
                if (string.Compare(@this[i].Text, instance.Name, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    idx = i;
                    break;
                }
            }
            return idx;
        }
    }
}
