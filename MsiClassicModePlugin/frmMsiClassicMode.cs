﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microarea.Mago4Butler.Plugins;

namespace MsiClassicModePlugin
{
    public partial class frmMsiClassicMode : Form
    {

        private bool isupdating;

        //List<Feature> FeatureCollection;
        public bool IsUpdating
        {
            get { return isupdating; }
            set { isupdating = value; }
        }

        public frmMsiClassicMode(bool isupdating/*, CmdLineInfo listfeature*/  )
        {

            //var FeatureCollection = new List<Feature>(listfeature.Features);


            InitializeComponent();

            ListViewItem lvi = new ListViewItem();

            txtInstanceName.Enabled = !isupdating;
            //listFeature.Visible = !isupdating;


            propgrdSettings.SelectedObject = Properties.Settings.Default;
            propgrdSettings.ToolbarVisible = false;

                                                        //////foreach (var feat in listfeature.Features)
                                                        //////{
                                                        //////    lvi.SubItems.Add(feat.Name);
                                                        //////    lvi.SubItems.Add(feat.Description);

                                                        //////    listFeature.Items.Add(lvi);
                                                        //////}

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

            errProvider.Clear();

            if (!FieldsHaveErrors())
            { this.DialogResult = System.Windows.Forms.DialogResult.OK; }

        }

        private bool FieldsHaveErrors()
        {
            var boxes = Controls.OfType<TextBox>();

            int ctr = 0;
            foreach (var box in boxes)
            {
                if (string.IsNullOrWhiteSpace(box.Text))
                {

                    errProvider.SetIconAlignment(box, ErrorIconAlignment.MiddleLeft);
                    errProvider.SetError(box, "Please fill the required field");
                    ctr++;
                }


            }

            return ctr > 0;

        }



        private void frmMsiClassicMode_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FieldsHaveErrors())
            { this.DialogResult = System.Windows.Forms.DialogResult.Cancel; }


        }
 



        private void btnSplitPanel_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
        }
    }

}