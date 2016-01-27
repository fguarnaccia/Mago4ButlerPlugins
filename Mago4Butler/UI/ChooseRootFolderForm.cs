using Microarea.Mago4Butler.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microarea.Mago4Butler
{
    public partial class ChooseRootFolderForm : Form
    {
        ISettings settings;

        public ChooseRootFolderForm(ISettings settings)
        {
            this.settings = settings;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.txtRootFolder.Text = this.settings.RootFolder;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            if (this.DialogResult == DialogResult.OK)
            {
                this.settings.RootFolder = this.txtRootFolder.Text;

                this.settings.Save();
            }
        }

        private void btnRootFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                fbd.Description = "Select the path where all Mago4 installations will live";
                fbd.ShowNewFolderButton = true;

                var res = fbd.ShowDialog(this);

                if (res != DialogResult.OK)
                {
                    return;
                }

                this.txtRootFolder.Text = fbd.SelectedPath;
            }
        }

      
        
    }
}
