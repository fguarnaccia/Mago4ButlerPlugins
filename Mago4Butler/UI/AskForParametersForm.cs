using Microarea.Mago4Butler.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Microarea.Mago4Butler
{
    public partial class AskForParametersForm : Form
    {
        Model model;
        ISettings settings;

        public string InstanceName { get; set; }
        public string MsiFullPath { get; set; }

        public AskForParametersForm(Model model, ISettings settings)
        {
            this.settings = settings;
            this.model = model;
            InitializeComponent();
            this.txtInstanceName.TextChanged += TxtInstanceName_TextChanged;
        }

        private void TxtInstanceName_TextChanged(object sender, EventArgs e)
        {
            this.errorProviderInstanceName.Clear();
            if (this.model.ContainsInstance(this.txtInstanceName.Text.Trim()))
            {
                this.errorProviderInstanceName.SetError(this.txtInstanceName, "An instance with the given name already exists");
            }
            if (!Model.IsInstanceNameValid(this.txtInstanceName.Text))
            {
                this.errorProviderInstanceName.SetError(this.txtInstanceName, "Only letters, digits and '-' are allowed");
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            if (this.DialogResult == DialogResult.OK)
            {
                this.InstanceName = this.txtInstanceName.Text;
            }
            else
            {
                this.InstanceName = String.Empty;
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            string candiateInstanceName = "";
            this.btnOK.Enabled =
                (candiateInstanceName = this.txtInstanceName.Text.Trim()).Length > 0 &&
                !this.model.ContainsInstance(candiateInstanceName);
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
