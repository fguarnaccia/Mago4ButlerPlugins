using Microarea.Mago4Butler.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Microarea.Mago4Butler
{
    public partial class AskForParametersForm : Form
    {
        Model model;

        public string InstanceName { get; set; }
        public string MsiFullPath { get; set; }

        public AskForParametersForm(Model model)
        {
            this.model = model;
            InitializeComponent();
            this.txtInstanceName.TextChanged += TxtInstanceName_TextChanged;
        }

        private void TxtInstanceName_TextChanged(object sender, EventArgs e)
        {
            this.errorProviderInstanceName.Clear();
            if (this.model.Instances.Contains(this.txtInstanceName.Text.Trim()))
            {
                this.errorProviderInstanceName.SetError(this.txtInstanceName, "An instance with the given name already exists");
            }
            if (this.txtInstanceName.Text.IndexOf(" ") != -1)
            {
                this.errorProviderInstanceName.SetError(this.txtInstanceName, "Instance name cannot contains blanks");
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            if (this.DialogResult == DialogResult.OK)
            {
                this.InstanceName = this.txtInstanceName.Text;
                this.MsiFullPath = this.txtMsiFullPath.Text;
            }
            else
            {
                this.InstanceName = String.Empty;
                this.MsiFullPath = String.Empty;
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            string candiateInstanceName = "";
            this.btnOK.Enabled =
                this.txtMsiFullPath.Text.Trim().Length > 0 &&
                (candiateInstanceName = this.txtInstanceName.Text.Trim()).Length > 0 &&
                !this.model.Instances.Contains(candiateInstanceName);
        }

        private void btnOpenFileDialog_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                ofd.Multiselect = false;
                ofd.Title = "Select Mago4 msi file";
                var res = ofd.ShowDialog(this);

                if (res != DialogResult.OK)
                {
                    return;
                }

                this.txtMsiFullPath.Text = ofd.FileName;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
