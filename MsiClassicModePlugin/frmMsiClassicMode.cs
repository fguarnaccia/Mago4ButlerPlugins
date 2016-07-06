using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MsiClassicModePlugin
{
    public partial class frmMsiClassicMode : Form
    {

        private bool isupdating;

        public bool IsUpdating   
        {
            get { return isupdating; }
            set { isupdating = value; }
        }



        public frmMsiClassicMode(bool isupdating)
        {
            InitializeComponent();
            lblInstanceName.Visible = !isupdating;
            txtInstanceName.Visible = !isupdating;

          
        }


        public frmMsiClassicMode()
        {

            InitializeComponent();
        }


        private void btnSelectFileMsi_Click(object sender, EventArgs e)
        {
            dlgOpenFile.ShowDialog();
            txtboxFileMsi.Text = dlgOpenFile.FileName;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {


            checkFields();

            this.Close();
        }

        private void checkFields()
        {
            var boxes = Controls.OfType<TextBox>();


            foreach (var box in boxes)
            {
                if (string.IsNullOrWhiteSpace(box.Text))
                {
                    errProvider.SetError(box, "Please fill the required field");
                }
            }
        }

        private void frmMsiClassicMode_FormClosing(object sender, FormClosingEventArgs e)
        {
            checkFields();
        }
    }

}
