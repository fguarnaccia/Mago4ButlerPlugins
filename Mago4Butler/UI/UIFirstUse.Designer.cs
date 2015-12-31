namespace Microarea.Mago4Butler
{
    partial class UIFirstUse
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lnkSelectMsi = new System.Windows.Forms.LinkLabel();
            this.lblNoInstallationsYet = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // lnkSelectMsi
            // 
            this.lnkSelectMsi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkSelectMsi.AutoSize = true;
            this.lnkSelectMsi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkSelectMsi.Location = new System.Drawing.Point(111, 136);
            this.lnkSelectMsi.Name = "lnkSelectMsi";
            this.lnkSelectMsi.Size = new System.Drawing.Size(162, 20);
            this.lnkSelectMsi.TabIndex = 3;
            this.lnkSelectMsi.TabStop = true;
            this.lnkSelectMsi.Text = "Select a MSI to install";
            this.lnkSelectMsi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkSelectMsi.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelectMsi_LinkClicked);
            // 
            // lblNoInstallationsYet
            // 
            this.lblNoInstallationsYet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNoInstallationsYet.AutoSize = true;
            this.lblNoInstallationsYet.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoInstallationsYet.ForeColor = System.Drawing.Color.Gray;
            this.lblNoInstallationsYet.Location = new System.Drawing.Point(39, 97);
            this.lblNoInstallationsYet.Name = "lblNoInstallationsYet";
            this.lblNoInstallationsYet.Size = new System.Drawing.Size(309, 39);
            this.lblNoInstallationsYet.TabIndex = 2;
            this.lblNoInstallationsYet.Text = "No installations yet";
            this.lblNoInstallationsYet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox
            // 
            this.pictureBox.BackgroundImage = global::Microarea.Mago4Butler.Properties.Resources.logo;
            this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox.Location = new System.Drawing.Point(174, 27);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(48, 67);
            this.pictureBox.TabIndex = 9;
            this.pictureBox.TabStop = false;
            // 
            // UIFirstUse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.lnkSelectMsi);
            this.Controls.Add(this.lblNoInstallationsYet);
            this.MaximumSize = new System.Drawing.Size(400, 400);
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "UIFirstUse";
            this.Size = new System.Drawing.Size(400, 400);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkSelectMsi;
        private System.Windows.Forms.Label lblNoInstallationsYet;
        private System.Windows.Forms.PictureBox pictureBox;
    }
}
