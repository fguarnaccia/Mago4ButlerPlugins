namespace Microarea.Mago4Butler
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.txtRootFolder = new System.Windows.Forms.TextBox();
            this.btnRootFolder = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblRootFolder = new System.Windows.Forms.Label();
            this.lblWebSites = new System.Windows.Forms.Label();
            this.cmbWebSites = new System.Windows.Forms.ComboBox();
            this.ckbAlsoDeleteCustom = new System.Windows.Forms.CheckBox();
            this.ckbCreateMsiLog = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtRootFolder
            // 
            this.txtRootFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRootFolder.Location = new System.Drawing.Point(16, 33);
            this.txtRootFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtRootFolder.Name = "txtRootFolder";
            this.txtRootFolder.ReadOnly = true;
            this.txtRootFolder.Size = new System.Drawing.Size(466, 22);
            this.txtRootFolder.TabIndex = 0;
            // 
            // btnRootFolder
            // 
            this.btnRootFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRootFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRootFolder.Location = new System.Drawing.Point(491, 33);
            this.btnRootFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRootFolder.Name = "btnRootFolder";
            this.btnRootFolder.Size = new System.Drawing.Size(37, 22);
            this.btnRootFolder.TabIndex = 1;
            this.btnRootFolder.Text = "...";
            this.btnRootFolder.UseVisualStyleBackColor = true;
            this.btnRootFolder.Click += new System.EventHandler(this.btnRootFolder_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Location = new System.Drawing.Point(322, 219);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 25);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(430, 219);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblRootFolder
            // 
            this.lblRootFolder.AutoSize = true;
            this.lblRootFolder.Location = new System.Drawing.Point(16, 13);
            this.lblRootFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRootFolder.Name = "lblRootFolder";
            this.lblRootFolder.Size = new System.Drawing.Size(81, 14);
            this.lblRootFolder.TabIndex = 2;
            this.lblRootFolder.Text = "Root folder:";
            // 
            // lblWebSites
            // 
            this.lblWebSites.AutoSize = true;
            this.lblWebSites.Location = new System.Drawing.Point(16, 138);
            this.lblWebSites.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWebSites.Name = "lblWebSites";
            this.lblWebSites.Size = new System.Drawing.Size(76, 14);
            this.lblWebSites.TabIndex = 3;
            this.lblWebSites.Text = "Web Sites:";
            this.lblWebSites.Visible = false;
            // 
            // cmbWebSites
            // 
            this.cmbWebSites.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWebSites.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbWebSites.FormattingEnabled = true;
            this.cmbWebSites.Location = new System.Drawing.Point(16, 157);
            this.cmbWebSites.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbWebSites.Name = "cmbWebSites";
            this.cmbWebSites.Size = new System.Drawing.Size(284, 22);
            this.cmbWebSites.TabIndex = 4;
            this.cmbWebSites.Visible = false;
            this.cmbWebSites.SelectedIndexChanged += new System.EventHandler(this.cmbWebSites_SelectedIndexChanged);
            // 
            // ckbAlsoDeleteCustom
            // 
            this.ckbAlsoDeleteCustom.AutoSize = true;
            this.ckbAlsoDeleteCustom.Location = new System.Drawing.Point(16, 75);
            this.ckbAlsoDeleteCustom.Name = "ckbAlsoDeleteCustom";
            this.ckbAlsoDeleteCustom.Size = new System.Drawing.Size(242, 18);
            this.ckbAlsoDeleteCustom.TabIndex = 5;
            this.ckbAlsoDeleteCustom.Text = "Delete \'Custom\' folder on uninstall";
            this.ckbAlsoDeleteCustom.UseVisualStyleBackColor = true;
            // 
            // ckbCreateMsiLog
            // 
            this.ckbCreateMsiLog.AutoSize = true;
            this.ckbCreateMsiLog.Location = new System.Drawing.Point(16, 99);
            this.ckbCreateMsiLog.Name = "ckbCreateMsiLog";
            this.ckbCreateMsiLog.Size = new System.Drawing.Size(117, 18);
            this.ckbCreateMsiLog.TabIndex = 6;
            this.ckbCreateMsiLog.Text = "Create msi log";
            this.ckbCreateMsiLog.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(544, 256);
            this.Controls.Add(this.ckbCreateMsiLog);
            this.Controls.Add(this.ckbAlsoDeleteCustom);
            this.Controls.Add(this.cmbWebSites);
            this.Controls.Add(this.lblWebSites);
            this.Controls.Add(this.lblRootFolder);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRootFolder);
            this.Controls.Add(this.txtRootFolder);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRootFolder;
        private System.Windows.Forms.Button btnRootFolder;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblRootFolder;
        private System.Windows.Forms.Label lblWebSites;
        private System.Windows.Forms.ComboBox cmbWebSites;
        private System.Windows.Forms.CheckBox ckbAlsoDeleteCustom;
        private System.Windows.Forms.CheckBox ckbCreateMsiLog;
    }
}