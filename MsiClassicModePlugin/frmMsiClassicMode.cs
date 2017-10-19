using System;
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
        
        bool IsUpdating { get; set; }
        bool FieldsHaveErrors { get; set; }
       

        MSISelector msiselector;
        public frmMsiClassicMode(bool isupdating = false/*, CmdLineInfo listfeature*/  )
        {

            IsUpdating = isupdating;
            InitializeComponent();
            InitializeOtherComponents();

        }


        public void InitializeOtherComponents()
        {
            //creo pumello con lista
            DropDownButton dropdownbtn = new DropDownButton();
            dropdownbtn.Click += new EventHandler(dropdownbtn_Click);
            dropdownbtn.Text = "...";
            dropdownbtn.Location = new System.Drawing.Point(423, 23);
            dropdownbtn.Size = new System.Drawing.Size(35, 23);
            dropdownbtn.Menu = new ContextMenuStrip();
            dropdownbtn.TabIndex = 2;

            itemCCNet.Visible = !IsUpdating;
            itemFolder.Visible = true;
            itemSite.Visible = true;

            Controls.Add(dropdownbtn);

            txtInstanceName.Enabled = !IsUpdating;

            propgrdSettings.SelectedObject = Properties.Settings.Default;
            propgrdSettings.ToolbarVisible = false;

            if (IsUpdating)
            {
                this.Text = "Update instance";
            }

            MSISelector.LocalFolderDestination = App.Instance.Settings.MsiFolder;

        }

        private void dropdownbtn_Click(object sender, EventArgs e)
        {
            dropdownMsiFrom.Show((Button)sender, new Point(0, ((Button)sender).Height));
        }

        private void btnSelectFileMsi_Click(object sender, EventArgs e)
        {
            dlgOpenFile.ShowDialog();
            txtboxFileMsi.Text = dlgOpenFile.FileName;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            //errProvider.Clear();
            if (!FieldsHaveErrors && !FieldsAreEmpty())
            { this.DialogResult = System.Windows.Forms.DialogResult.OK;
                errProvider.Clear();
            }
            
            
        }


        private bool FieldsAreEmpty()
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

        private bool InstanceAlreadyExists(string NomeIstanza)
        {
            bool result = false;


            foreach (var i in App.Instance.GetInstances())
            {

                if (string.Compare (i , NomeIstanza ,StringComparison.InvariantCultureIgnoreCase) == 0)
                                {
                    
                    result = true;
                    break;
                }

            }
            FieldsHaveErrors = result;
                return result;
        }

        private void frmMsiClassicMode_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FieldsHaveErrors)
            { this.DialogResult = System.Windows.Forms.DialogResult.Cancel; }
            Properties.Settings.Default.Save();
        }

        private void frmMsiClassicMode_Load(object sender, EventArgs e)
        {
            
            propgrdSettings.CollapseAllGridItems();
        }

        private void txtInstanceName_Leave(object sender, EventArgs e)
        {
            errProvider.Clear();

            if (InstanceAlreadyExists(txtInstanceName.Text))
            {

                errProvider.SetIconAlignment((TextBox)sender, ErrorIconAlignment.MiddleLeft);
                errProvider.SetError((TextBox)sender, "Instance Already exists!");
                return;
            }
            

            if (!App.Instance.IsInstanceNameValid(txtInstanceName.Text))
            {
                errProvider.SetIconAlignment((TextBox)sender, ErrorIconAlignment.MiddleLeft);
                errProvider.SetError((TextBox)sender, "Instance Name invalid!");
                FieldsHaveErrors = true;
                return;
            }
            else
            { FieldsHaveErrors = false; }

        }

        private void MsiSelector_MsiSelected(object sender, EventArgs e)
        {
            //DONE: aggiunto try-catch [verificare eccezione System.InvalidCastException: Unable to cast object of type 'MsiClassicModePlugin.SelectorFromCCNet' to type 'System.Windows.Forms.TextBox'.
            //  at MsiClassicModePlugin.frmMsiClassicMode.MsiSelector_MsiSelected(Object sender, EventArgs e)]
            try
            {
                txtboxFileMsi.Text = msiselector.SelectedMsiFilePath;
                if (msiselector != null)
                    msiselector.Visible = false;

                if (!IsUpdating && txtInstanceName.Text == string.Empty)
                    txtInstanceName.Text = msiselector.SelectedInstanceName;
            }
            catch (Exception ex)
            {
            
                    PluginException plgnex = new PluginException("", ex);

                    plgnex.ParamName = "SelectedMsiFilePath";
                    plgnex.ParamValue = msiselector.SelectedMsiFilePath;
                    plgnex.ToString();
                     plgnex.ParamName = "SelectedInstanceName";
                    plgnex.ParamValue = msiselector.SelectedInstanceName;
                     plgnex.ToString();
                throw plgnex;

                }
            
        }

        private void itemCCNet_Click(object sender, EventArgs e)
        {

            if (msiselector != null)
            {
                msiselector.MsiSelected -= MsiSelector_MsiSelected;
                msiselector.Dispose();
            }
            msiselector = new SelectorFromCCNet();

            msiselector.MsiSelected += MsiSelector_MsiSelected;
            msiselector.DrawSelector();
            msiselector.Visible = true;
            Controls.Add(msiselector);
            Controls.SetChildIndex(msiselector, 0);

        }

        private void itemFolder_Click(object sender, EventArgs e)
        {
            if (msiselector != null) msiselector.Visible = false;
            dlgOpenFile.ShowDialog();
            txtboxFileMsi.Text = dlgOpenFile.FileName;
        }

        private void itemSite_Click(object sender, EventArgs e)
        {
            if (msiselector != null)
            {
                msiselector.MsiSelected -= MsiSelector_MsiSelected;
                msiselector.Dispose();
            }
            msiselector = new SelectorFromSite();

            msiselector.MsiSelected += MsiSelector_MsiSelected;
            msiselector.DrawSelector();
            msiselector.Visible = true;
            Controls.Add(msiselector);
            Controls.SetChildIndex(msiselector, 0);

        }

    }

    // ------   ***** ------------------
    public class DropDownButton : Button
    {
        [DefaultValue(null)]
        public ContextMenuStrip Menu { get; set; }


        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            if (Menu != null)
            {
                int arrowX = ClientRectangle.Width - 14;
                int arrowY = ClientRectangle.Height / 2 - 1;

                Brush brush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
                Point[] arrows = new Point[] { new Point(arrowX, arrowY), new Point(arrowX + 7, arrowY), new Point(arrowX + 3, arrowY + 4) };
                pevent.Graphics.FillPolygon(brush, arrows);
            }
        }
    }
}