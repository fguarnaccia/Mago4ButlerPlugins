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
            this.txtInstanceName = new System.Windows.Forms.TextBox();
            this.lblInstanceName = new System.Windows.Forms.Label();
            this.lblFileMsi = new System.Windows.Forms.Label();
            this.txtboxFileMsi = new System.Windows.Forms.TextBox();
            this.btnSelectFileMsi = new System.Windows.Forms.Button();
            this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.errProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // txtInstanceName
            // 
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
            this.btnOk.Location = new System.Drawing.Point(23, 162);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmMsiClassicMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 220);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnSelectFileMsi);
            this.Controls.Add(this.txtboxFileMsi);
            this.Controls.Add(this.lblFileMsi);
            this.Controls.Add(this.lblInstanceName);
            this.Controls.Add(this.txtInstanceName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMsiClassicMode";
            this.Text = "Parameters";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMsiClassicMode_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).EndInit();
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
    }
}