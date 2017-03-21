namespace MsiClassicModePlugin
{
    partial class MSISelector 
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
            this.lstboxMsi = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lstboxMsi
            // 
            this.lstboxMsi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstboxMsi.FormattingEnabled = true;
            this.lstboxMsi.Location = new System.Drawing.Point(0, 0);
            this.lstboxMsi.Name = "lstboxMsi";
            this.lstboxMsi.Size = new System.Drawing.Size(150, 150);
            this.lstboxMsi.TabIndex = 0;
            this.lstboxMsi.DoubleClick += new System.EventHandler(this.lstboxMsi_DoubleClick);
            this.lstboxMsi.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstboxMsi_KeyUp);
            // 
            // CCNetManagerClientUsrCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstboxMsi);
            this.Name = "CCNetManagerClientUsrCtrl";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstboxMsi;
    }
}
