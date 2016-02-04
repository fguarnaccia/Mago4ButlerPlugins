using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microarea.Mago4Butler.BL;

namespace Microarea.Mago4Butler
{
    public partial class UINormalUse : UserControl
    {
        Model model;

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
        public UINormalUse(Model model)
        {
            this.model = model;
            this.model.InstanceAdded += Model_InstanceAdded;
            this.model.InstanceRemoved += Model_InstanceRemoved;
            this.model.InstanceUpdated += Model_InstanceUpdated;

            InitializeComponent();
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

        private void AddInstanceToListView(Instance instance)
        {
            var listViewItem = this.lsvInstances.Items.Add(instance.Name, instance.ToString(), -1);
            listViewItem.Tag = instance;
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
        }

        private void btnAddInstance_Click(object sender, EventArgs e)
        {
            this.OnInstallNewInstance(new InstallInstanceEventArgs());
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            List<Instance> selectedInstances = new List<Instance>();
            foreach (ListViewItem selectedItem in this.lsvInstances.SelectedItems)
            {
                selectedInstances.Add(selectedItem.Tag as Instance);
            }

            if (selectedInstances.Count > 0)
            {
                this.OnUpdateInstance(new UpdateInstanceEventArgs() { Instances = selectedInstances });
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            List<Instance> selectedInstances = new List<Instance>();
            foreach (ListViewItem selectedItem in this.lsvInstances.SelectedItems)
            {
                selectedInstances.Add(selectedItem.Tag as Instance);
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
    }
}
