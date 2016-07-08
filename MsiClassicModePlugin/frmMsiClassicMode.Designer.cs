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
            this.btnSelectFileMsi = new System.Windows.Forms.Button();
            this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.errProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnOk = new System.Windows.Forms.Button();
            this.btnEsc = new System.Windows.Forms.Button();
            this.propgrdSettings = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnSplitPanel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtInstanceName
            // 
            this.errProvider.SetIconAlignment(this.txtInstanceName, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.txtInstanceName.Location = new System.Drawing.Point(108, 33);
            this.txtInstanceName.Name = "txtInstanceName";
            this.txtInstanceName.Size = new System.Drawing.Size(218, 20);
            this.txtInstanceName.TabIndex = 1;
            // 
            // lblInstanceName
            // 
            this.lblInstanceName.AutoSize = true;
            this.lblInstanceName.Location = new System.Drawing.Point(20, 36);
            this.lblInstanceName.Name = "lblInstanceName";
            this.lblInstanceName.Size = new System.Drawing.Size(82, 13);
            this.lblInstanceName.TabIndex = 0;
            this.lblInstanceName.Text = "Instance Name:";
            // 
            // lblFileMsi
            // 
            this.lblFileMsi.AutoSize = true;
            this.lblFileMsi.Location = new System.Drawing.Point(20, 72);
            this.lblFileMsi.Name = "lblFileMsi";
            this.lblFileMsi.Size = new System.Drawing.Size(44, 13);
            this.lblFileMsi.TabIndex = 2;
            this.lblFileMsi.Text = "File msi:";
            // 
            // txtboxFileMsi
            // 
            this.errProvider.SetIconAlignment(this.txtboxFileMsi, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.txtboxFileMsi.Location = new System.Drawing.Point(108, 69);
            this.txtboxFileMsi.Name = "txtboxFileMsi";
            this.txtboxFileMsi.Size = new System.Drawing.Size(359, 20);
            this.txtboxFileMsi.TabIndex = 3;
            // 
            // btnSelectFileMsi
            // 
            this.btnSelectFileMsi.Location = new System.Drawing.Point(473, 69);
            this.btnSelectFileMsi.Name = "btnSelectFileMsi";
            this.btnSelectFileMsi.Size = new System.Drawing.Size(27, 20);
            this.btnSelectFileMsi.TabIndex = 4;
            this.btnSelectFileMsi.Text = "...";
            this.btnSelectFileMsi.UseVisualStyleBackColor = true;
            this.btnSelectFileMsi.Click += new System.EventHandler(this.btnSelectFileMsi_Click);
            // 
            // dlgOpenFile
            // 
            this.dlgOpenFile.FileName = "openFileDialog1";
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
            this.btnOk.Location = new System.Drawing.Point(23, 99);
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
            this.btnEsc.Location = new System.Drawing.Point(339, 99);
            this.btnEsc.Name = "btnEsc";
            this.btnEsc.Size = new System.Drawing.Size(182, 23);
            this.btnEsc.TabIndex = 6;
            this.btnEsc.Text = "&Annulla";
            this.btnEsc.UseVisualStyleBackColor = true;
            // 
            // propgrdSettings
            // 
            this.propgrdSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propgrdSettings.HelpVisible = false;
            this.propgrdSettings.Location = new System.Drawing.Point(0, 0);
            this.propgrdSettings.Name = "propgrdSettings";
            this.propgrdSettings.Size = new System.Drawing.Size(533, 192);
            this.propgrdSettings.TabIndex = 7;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 159);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnSplitPanel);

            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propgrdSettings);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(533, 258);
            this.splitContainer1.SplitterDistance = 62;
            this.splitContainer1.TabIndex = 8;
            // 
            // btnSplitPanel
            // 
            this.btnSplitPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSplitPanel.Location = new System.Drawing.Point(0, 0);
            this.btnSplitPanel.Name = "btnSplitPanel";
            this.btnSplitPanel.Size = new System.Drawing.Size(533, 258);
            this.btnSplitPanel.TabIndex = 0;
            this.btnSplitPanel.Text = "Advanced &options";
            this.btnSplitPanel.UseVisualStyleBackColor = true;
            this.btnSplitPanel.Click += new System.EventHandler(this.btnSplitPanel_Click);
            // 
            // frmMsiClassicMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 429);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnEsc);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnSelectFileMsi);
            this.Controls.Add(this.txtboxFileMsi);
            this.Controls.Add(this.lblFileMsi);
            this.Controls.Add(this.lblInstanceName);
            this.Controls.Add(this.txtInstanceName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMsiClassicMode";
            this.Text = "Parameters";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMsiClassicMode_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txtInstanceName;
        private System.Windows.Forms.Label lblInstanceName;
        private System.Windows.Forms.Label lblFileMsi;
        public System.Windows.Forms.TextBox txtboxFileMsi;
        private System.Windows.Forms.Button btnSelectFileMsi;
        private System.Windows.Forms.OpenFileDialog dlgOpenFile;
        private System.Windows.Forms.ErrorProvider errProvider;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnEsc;
        private System.Windows.Forms.PropertyGrid propgrdSettings;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnSplitPanel;
    }
}