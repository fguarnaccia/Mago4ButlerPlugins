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
    public partial class AskConfirmationForUninstallForm : Form
    {
        public AskConfirmationForUninstallForm(List<Instance> candidatesForDeletion)
        {
            InitializeComponent();
            this.txtInstancesToDelete.Text = String.Join(Environment.NewLine, candidatesForDeletion.Select(i => i.Name));
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
