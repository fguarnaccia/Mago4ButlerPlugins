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

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            foreach (var instance in this.model.Instances)
            {
                var listViewItem = this.lsvInstances.Items.Add(instance, instance, -1);
                listViewItem.Tag = instance;
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
            List<String> selectedInstances = new List<string>();
            foreach (ListViewItem selectedItem in this.lsvInstances.SelectedItems)
            {
                selectedInstances.Add(selectedItem.Tag as string);
            }

            if (selectedInstances.Count > 0)
            {
                this.OnUpdateInstance(new UpdateInstanceEventArgs() { InstanceNames = selectedInstances });
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            List<String> selectedInstances = new List<string>();
            foreach (ListViewItem selectedItem in this.lsvInstances.SelectedItems)
            {
                selectedInstances.Add(selectedItem.Tag as string);
            }

            if (selectedInstances.Count > 0)
            {
                this.OnRemoveInstance(new RemoveInstanceEventArgs() { InstanceNames = selectedInstances });
            }
        }
    }
}
