namespace Microarea.Mago4Butler.AutomaticUpdates
{
    partial class AskForUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AskForUpdate));
            this.btnInstall = new System.Windows.Forms.Button();
            this.btnNoThanks = new System.Windows.Forms.Button();
            this.pnlButlerImg = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnInstall
            // 
            this.btnInstall.BackColor = System.Drawing.Color.ForestGreen;
            this.btnInstall.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnInstall.FlatAppearance.BorderColor = System.Drawing.Color.ForestGreen;
            this.btnInstall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInstall.ForeColor = System.Drawing.Color.White;
            this.btnInstall.Location = new System.Drawing.Point(155, 327);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(111, 23);
            this.btnInstall.TabIndex = 1;
            this.btnInstall.Text = "Yes, please";
            this.btnInstall.UseVisualStyleBackColor = false;
            // 
            // btnNoThanks
            // 
            this.btnNoThanks.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnNoThanks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNoThanks.Location = new System.Drawing.Point(272, 327);
            this.btnNoThanks.Name = "btnNoThanks";
            this.btnNoThanks.Size = new System.Drawing.Size(111, 23);
            this.btnNoThanks.TabIndex = 1;
            this.btnNoThanks.Text = "Not now, thanks";
            this.btnNoThanks.UseVisualStyleBackColor = true;
            // 
            // pnlButlerImg
            // 
            this.pnlButlerImg.BackgroundImage = global::Microarea.Mago4Butler.AutomaticUpdates.Properties.Resources.UpdateRequest;
            this.pnlButlerImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlButlerImg.Location = new System.Drawing.Point(12, 12);
            this.pnlButlerImg.Name = "pnlButlerImg";
            this.pnlButlerImg.Size = new System.Drawing.Size(366, 300);
            this.pnlButlerImg.TabIndex = 2;
            // 
            // AskForUpdate
            // 
            this.AcceptButton = this.btnInstall;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnNoThanks;
            this.ClientSize = new System.Drawing.Size(390, 362);
            this.Controls.Add(this.pnlButlerImg);
            this.Controls.Add(this.btnNoThanks);
            this.Controls.Add(this.btnInstall);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(406, 400);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(406, 400);
            this.Name = "AskForUpdate";
            this.Text = "Updates available";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Button btnNoThanks;
        private System.Windows.Forms.Panel pnlButlerImg;
    }
}