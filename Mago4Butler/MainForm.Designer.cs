namespace Microarea.Mago4Butler
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbSettings = new System.Windows.Forms.ToolStripButton();
            this.bntAbout = new System.Windows.Forms.ToolStripButton();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.tsbViewLogs = new System.Windows.Forms.ToolStripButton();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbViewLogs,
            this.tsbSettings,
            this.bntAbout});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(384, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // tsbSettings
            // 
            this.tsbSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbSettings.Image = ((System.Drawing.Image)(resources.GetObject("tsbSettings.Image")));
            this.tsbSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSettings.Name = "tsbSettings";
            this.tsbSettings.Size = new System.Drawing.Size(53, 22);
            this.tsbSettings.Text = "Settings";
            this.tsbSettings.Click += new System.EventHandler(this.tsbSettings_Click);
            // 
            // bntAbout
            // 
            this.bntAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.bntAbout.Image = ((System.Drawing.Image)(resources.GetObject("bntAbout.Image")));
            this.bntAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bntAbout.Name = "bntAbout";
            this.bntAbout.Size = new System.Drawing.Size(23, 22);
            this.bntAbout.Text = "?";
            this.bntAbout.Click += new System.EventHandler(this.bntAbout_Click);
            // 
            // pnlContent
            // 
            this.pnlContent.BackgroundImage = global::Microarea.Mago4Butler.Properties.Resources.logo;
            this.pnlContent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 25);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(384, 437);
            this.pnlContent.TabIndex = 1;
            // 
            // tsbViewLogs
            // 
            this.tsbViewLogs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbViewLogs.Image = ((System.Drawing.Image)(resources.GetObject("tsbViewLogs.Image")));
            this.tsbViewLogs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbViewLogs.Name = "tsbViewLogs";
            this.tsbViewLogs.Size = new System.Drawing.Size(64, 22);
            this.tsbViewLogs.Text = "View Logs";
            this.tsbViewLogs.Click += new System.EventHandler(this.tsbViewLogs_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 462);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 500);
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "MainForm";
            this.Text = "Mago4 Butler";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton bntAbout;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.ToolStripButton tsbSettings;
        private System.Windows.Forms.ToolStripButton tsbViewLogs;
    }
}

