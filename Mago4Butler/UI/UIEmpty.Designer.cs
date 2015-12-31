namespace Microarea.Mago4Butler
{
    partial class UIEmpty
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
            this.lblNoMoreInstallations = new System.Windows.Forms.Label();
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
            this.lnkSelectMsi.TabIndex = 5;
            this.lnkSelectMsi.TabStop = true;
            this.lnkSelectMsi.Text = "Select a MSI to install";
            this.lnkSelectMsi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkSelectMsi.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelectMsi_LinkClicked);
            // 
            // lblNoMoreInstallations
            // 
            this.lblNoMoreInstallations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNoMoreInstallations.AutoSize = true;
            this.lblNoMoreInstallations.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoMoreInstallations.ForeColor = System.Drawing.Color.Gray;
            this.lblNoMoreInstallations.Location = new System.Drawing.Point(39, 97);
            this.lblNoMoreInstallations.Name = "lblNoMoreInstallations";
            this.lblNoMoreInstallations.Size = new System.Drawing.Size(342, 39);
            this.lblNoMoreInstallations.TabIndex = 4;
            this.lblNoMoreInstallations.Text = "No more installations";
            this.lblNoMoreInstallations.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox
            // 
            this.pictureBox.BackgroundImage = global::Microarea.Mago4Butler.Properties.Resources.logo;
            this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox.Location = new System.Drawing.Point(179, 27);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(48, 67);
            this.pictureBox.TabIndex = 9;
            this.pictureBox.TabStop = false;
            // 
            // UIEmpty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.lnkSelectMsi);
            this.Controls.Add(this.lblNoMoreInstallations);
            this.MaximumSize = new System.Drawing.Size(400, 400);
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "UIEmpty";
            this.Size = new System.Drawing.Size(400, 400);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkSelectMsi;
        private System.Windows.Forms.Label lblNoMoreInstallations;
        private System.Windows.Forms.PictureBox pictureBox;
    }
}
