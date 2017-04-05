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
            this.lstboxMain = new System.Windows.Forms.ListBox();
            this.tabControlMsi = new System.Windows.Forms.TabControl();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.tabAux = new System.Windows.Forms.TabPage();
            this.lstboxAux = new System.Windows.Forms.ListBox();
            this.chkShowHotFix = new System.Windows.Forms.CheckBox();
            this.tabControlMsi.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabAux.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstboxMain
            // 
            this.lstboxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstboxMain.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstboxMain.FormattingEnabled = true;
            this.lstboxMain.Location = new System.Drawing.Point(3, 3);
            this.lstboxMain.Name = "lstboxMain";
            this.lstboxMain.Size = new System.Drawing.Size(296, 164);
            this.lstboxMain.TabIndex = 0;
            this.lstboxMain.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstbox_DrawItem);
            this.lstboxMain.DoubleClick += new System.EventHandler(this.lstbox_DoubleClick);
            this.lstboxMain.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstbox_KeyUp);
            // 
            // tabControlMsi
            // 
            this.tabControlMsi.Controls.Add(this.tabMain);
            this.tabControlMsi.Controls.Add(this.tabAux);
            this.tabControlMsi.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControlMsi.Location = new System.Drawing.Point(0, 0);
            this.tabControlMsi.Name = "tabControlMsi";
            this.tabControlMsi.SelectedIndex = 0;
            this.tabControlMsi.Size = new System.Drawing.Size(310, 196);
            this.tabControlMsi.TabIndex = 1;
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.lstboxMain);
            this.tabMain.Location = new System.Drawing.Point(4, 22);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(302, 170);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "Mago4";
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // tabAux
            // 
            this.tabAux.Controls.Add(this.lstboxAux);
            this.tabAux.Location = new System.Drawing.Point(4, 22);
            this.tabAux.Name = "tabAux";
            this.tabAux.Padding = new System.Windows.Forms.Padding(3);
            this.tabAux.Size = new System.Drawing.Size(302, 170);
            this.tabAux.TabIndex = 1;
            this.tabAux.Text = "Mago.net";
            this.tabAux.UseVisualStyleBackColor = true;
            // 
            // lstboxAux
            // 
            this.lstboxAux.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstboxAux.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstboxAux.FormattingEnabled = true;
            this.lstboxAux.Location = new System.Drawing.Point(3, 3);
            this.lstboxAux.Name = "lstboxAux";
            this.lstboxAux.Size = new System.Drawing.Size(296, 164);
            this.lstboxAux.TabIndex = 1;
            this.lstboxAux.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstbox_DrawItem);
            this.lstboxAux.DoubleClick += new System.EventHandler(this.lstbox_DoubleClick);
            this.lstboxAux.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstbox_KeyUp);
            // 
            // chkShowHotFix
            // 
            this.chkShowHotFix.AutoSize = true;
            this.chkShowHotFix.Location = new System.Drawing.Point(7, 202);
            this.chkShowHotFix.Name = "chkShowHotFix";
            this.chkShowHotFix.Size = new System.Drawing.Size(84, 17);
            this.chkShowHotFix.TabIndex = 2;
            this.chkShowHotFix.Text = "Show hot fix";
            this.chkShowHotFix.UseVisualStyleBackColor = true;
            this.chkShowHotFix.Visible = false;
            // 
            // MSISelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.chkShowHotFix);
            this.Controls.Add(this.tabControlMsi);
            this.Name = "MSISelector";
            this.Size = new System.Drawing.Size(310, 223);
            this.tabControlMsi.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabAux.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkShowHotFix;
        internal System.Windows.Forms.ListBox lstboxMain;
        internal System.Windows.Forms.ListBox lstboxAux;
        public System.Windows.Forms.TabPage tabMain;
        public System.Windows.Forms.TabPage tabAux;
        public System.Windows.Forms.TabControl tabControlMsi;
    }
}
