namespace Microarea.Mago4Butler
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageAbout = new System.Windows.Forms.TabPage();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPagePlugins = new System.Windows.Forms.TabPage();
            this.lblPlugins = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabPageAbout.SuspendLayout();
            this.tabPagePlugins.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageAbout);
            this.tabControl.Controls.Add(this.tabPagePlugins);
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // tabPageAbout
            // 
            this.tabPageAbout.Controls.Add(this.btnCancel);
            this.tabPageAbout.Controls.Add(this.label1);
            resources.ApplyResources(this.tabPageAbout, "tabPageAbout");
            this.tabPageAbout.Name = "tabPageAbout";
            this.tabPageAbout.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tabPagePlugins
            // 
            this.tabPagePlugins.Controls.Add(this.lblPlugins);
            resources.ApplyResources(this.tabPagePlugins, "tabPagePlugins");
            this.tabPagePlugins.Name = "tabPagePlugins";
            this.tabPagePlugins.UseVisualStyleBackColor = true;
            // 
            // lblPlugins
            // 
            resources.ApplyResources(this.lblPlugins, "lblPlugins");
            this.lblPlugins.Name = "lblPlugins";
            // 
            // AboutForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.tabControl);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.tabControl.ResumeLayout(false);
            this.tabPageAbout.ResumeLayout(false);
            this.tabPagePlugins.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageAbout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPagePlugins;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblPlugins;
    }
}