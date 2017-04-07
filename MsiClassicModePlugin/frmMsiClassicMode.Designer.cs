namespace MsiClassicModePlugin
{
    partial class frmMsiClassicMode
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMsiClassicMode));
            this.txtInstanceName = new System.Windows.Forms.TextBox();
            this.lblInstanceName = new System.Windows.Forms.Label();
            this.lblFileMsi = new System.Windows.Forms.Label();
            this.txtboxFileMsi = new System.Windows.Forms.TextBox();
            this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.errProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnOk = new System.Windows.Forms.Button();
            this.btnEsc = new System.Windows.Forms.Button();
            this.propgrdSettings = new System.Windows.Forms.PropertyGrid();
            this.dropdownMsiFrom = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itemCCNet = new System.Windows.Forms.ToolStripMenuItem();
            this.itemFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.itemSite = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).BeginInit();
            this.dropdownMsiFrom.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtInstanceName
            // 
            this.errProvider.SetIconAlignment(this.txtInstanceName, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.txtInstanceName.Location = new System.Drawing.Point(50, 62);
            this.txtInstanceName.Name = "txtInstanceName";
            this.txtInstanceName.Size = new System.Drawing.Size(365, 20);
            this.txtInstanceName.TabIndex = 4;
            this.txtInstanceName.Leave += new System.EventHandler(this.txtInstanceName_Leave);
            // 
            // lblInstanceName
            // 
            this.lblInstanceName.AutoSize = true;
            this.lblInstanceName.Location = new System.Drawing.Point(11, 65);
            this.lblInstanceName.Name = "lblInstanceName";
            this.lblInstanceName.Size = new System.Drawing.Size(38, 13);
            this.lblInstanceName.TabIndex = 0;
            this.lblInstanceName.Text = "Name:";
            // 
            // lblFileMsi
            // 
            this.lblFileMsi.AutoSize = true;
            this.lblFileMsi.Location = new System.Drawing.Point(11, 27);
            this.lblFileMsi.Name = "lblFileMsi";
            this.lblFileMsi.Size = new System.Drawing.Size(26, 13);
            this.lblFileMsi.TabIndex = 2;
            this.lblFileMsi.Text = "Msi:";
            // 
            // txtboxFileMsi
            // 
            this.txtboxFileMsi.AllowDrop = true;
            this.errProvider.SetIconAlignment(this.txtboxFileMsi, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.txtboxFileMsi.Location = new System.Drawing.Point(50, 24);
            this.txtboxFileMsi.Name = "txtboxFileMsi";
            this.txtboxFileMsi.Size = new System.Drawing.Size(365, 20);
            this.txtboxFileMsi.TabIndex = 1;
            this.txtboxFileMsi.TextChanged += new System.EventHandler(this.txtboxFileMsi_TextChanged);
            // 
            // dlgOpenFile
            // 
            this.dlgOpenFile.Filter = "Msi file|*.msi";
            // 
            // errProvider
            // 
            this.errProvider.ContainerControl = this;
            // 
            // btnOk
            // 
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.errProvider.SetIconAlignment(this.btnOk, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.btnOk.Location = new System.Drawing.Point(14, 99);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(182, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnEsc
            // 
            this.btnEsc.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnEsc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEsc.Location = new System.Drawing.Point(253, 99);
            this.btnEsc.Name = "btnEsc";
            this.btnEsc.Size = new System.Drawing.Size(182, 23);
            this.btnEsc.TabIndex = 6;
            this.btnEsc.Text = "&Annulla";
            this.btnEsc.UseVisualStyleBackColor = true;
            // 
            // propgrdSettings
            // 
            this.propgrdSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.propgrdSettings.HelpVisible = false;
            this.propgrdSettings.Location = new System.Drawing.Point(2, 151);
            this.propgrdSettings.Name = "propgrdSettings";
            this.propgrdSettings.Size = new System.Drawing.Size(464, 238);
            this.propgrdSettings.TabIndex = 7;
            // 
            // dropdownMsiFrom
            // 
            this.dropdownMsiFrom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemCCNet,
            this.itemFolder,
            this.itemSite});
            this.dropdownMsiFrom.Name = "contextMenuStrip1";
            this.dropdownMsiFrom.ShowImageMargin = false;
            this.dropdownMsiFrom.Size = new System.Drawing.Size(128, 92);
            // 
            // itemCCNet
            // 
            this.itemCCNet.Name = "itemCCNet";
            this.itemCCNet.Size = new System.Drawing.Size(127, 22);
            this.itemCCNet.Text = "CCNet";
            this.itemCCNet.Click += new System.EventHandler(this.itemCCNet_Click);
            // 
            // itemFolder
            // 
            this.itemFolder.Name = "itemFolder";
            this.itemFolder.Size = new System.Drawing.Size(127, 22);
            this.itemFolder.Text = "Local";
            this.itemFolder.Click += new System.EventHandler(this.itemFolder_Click);
            // 
            // itemSite
            // 
            this.itemSite.Name = "itemSite";
            this.itemSite.Size = new System.Drawing.Size(127, 22);
            this.itemSite.Text = "&Official";
            this.itemSite.Visible = false;
            this.itemSite.Click += new System.EventHandler(this.itemSite_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 379);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(467, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // frmMsiClassicMode
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnEsc;
            this.ClientSize = new System.Drawing.Size(467, 401);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.propgrdSettings);
            this.Controls.Add(this.btnEsc);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtboxFileMsi);
            this.Controls.Add(this.lblFileMsi);
            this.Controls.Add(this.lblInstanceName);
            this.Controls.Add(this.txtInstanceName);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMsiClassicMode";
            this.ShowInTaskbar = false;
            this.Text = "Add Instance";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMsiClassicMode_FormClosing);
            this.Load += new System.EventHandler(this.frmMsiClassicMode_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).EndInit();
            this.dropdownMsiFrom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txtInstanceName;
        private System.Windows.Forms.Label lblInstanceName;
        private System.Windows.Forms.Label lblFileMsi;
        public System.Windows.Forms.TextBox txtboxFileMsi;
        private System.Windows.Forms.OpenFileDialog dlgOpenFile;
        private System.Windows.Forms.ErrorProvider errProvider;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnEsc;     
        private System.Windows.Forms.PropertyGrid propgrdSettings;
        private System.Windows.Forms.ContextMenuStrip dropdownMsiFrom;
        private System.Windows.Forms.ToolStripMenuItem itemFolder;
        private System.Windows.Forms.ToolStripMenuItem itemCCNet;
        private System.Windows.Forms.ToolStripMenuItem itemSite;
        private System.Windows.Forms.StatusStrip statusStrip1;
    }
}