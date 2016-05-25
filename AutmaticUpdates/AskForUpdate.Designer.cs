namespace Microarea.Mago4Butler.AutmaticUpdates
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
            this.lblUpdatesAvailable = new System.Windows.Forms.Label();
            this.btnInstall = new System.Windows.Forms.Button();
            this.btnNoThanks = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblUpdatesAvailable
            // 
            this.lblUpdatesAvailable.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpdatesAvailable.Location = new System.Drawing.Point(12, 48);
            this.lblUpdatesAvailable.Name = "lblUpdatesAvailable";
            this.lblUpdatesAvailable.Size = new System.Drawing.Size(366, 87);
            this.lblUpdatesAvailable.TabIndex = 0;
            this.lblUpdatesAvailable.Text = "A new version of Mago4Butler is available, do you want to proceed with the instal" +
    "lation?";
            // 
            // btnInstall
            // 
            this.btnInstall.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnInstall.Location = new System.Drawing.Point(186, 209);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(75, 23);
            this.btnInstall.TabIndex = 1;
            this.btnInstall.Text = "Install";
            this.btnInstall.UseVisualStyleBackColor = true;
            // 
            // btnNoThanks
            // 
            this.btnNoThanks.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnNoThanks.Location = new System.Drawing.Point(267, 209);
            this.btnNoThanks.Name = "btnNoThanks";
            this.btnNoThanks.Size = new System.Drawing.Size(111, 23);
            this.btnNoThanks.TabIndex = 1;
            this.btnNoThanks.Text = "Not now, thanks";
            this.btnNoThanks.UseVisualStyleBackColor = true;
            // 
            // AskForUpdate
            // 
            this.AcceptButton = this.btnInstall;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnNoThanks;
            this.ClientSize = new System.Drawing.Size(390, 244);
            this.Controls.Add(this.btnNoThanks);
            this.Controls.Add(this.btnInstall);
            this.Controls.Add(this.lblUpdatesAvailable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(406, 282);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(406, 282);
            this.Name = "AskForUpdate";
            this.Text = "Updates available";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblUpdatesAvailable;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Button btnNoThanks;
    }
}