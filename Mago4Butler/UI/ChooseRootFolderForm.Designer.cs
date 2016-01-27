namespace Microarea.Mago4Butler
{
    partial class ChooseRootFolderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseRootFolderForm));
            this.btnRootFolder = new System.Windows.Forms.Button();
            this.txtRootFolder = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblExplanation = new System.Windows.Forms.Label();
            this.lblRootFolder = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRootFolder
            // 
            this.btnRootFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRootFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRootFolder.Location = new System.Drawing.Point(493, 76);
            this.btnRootFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRootFolder.Name = "btnRootFolder";
            this.btnRootFolder.Size = new System.Drawing.Size(37, 22);
            this.btnRootFolder.TabIndex = 4;
            this.btnRootFolder.Text = "...";
            this.btnRootFolder.UseVisualStyleBackColor = true;
            this.btnRootFolder.Click += new System.EventHandler(this.btnRootFolder_Click);
            // 
            // txtRootFolder
            // 
            this.txtRootFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRootFolder.Location = new System.Drawing.Point(13, 76);
            this.txtRootFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtRootFolder.Name = "txtRootFolder";
            this.txtRootFolder.ReadOnly = true;
            this.txtRootFolder.Size = new System.Drawing.Size(472, 22);
            this.txtRootFolder.TabIndex = 3;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Location = new System.Drawing.Point(322, 124);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 25);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(430, 125);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblExplanation
            // 
            this.lblExplanation.Location = new System.Drawing.Point(10, 10);
            this.lblExplanation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExplanation.Name = "lblExplanation";
            this.lblExplanation.Size = new System.Drawing.Size(520, 40);
            this.lblExplanation.TabIndex = 8;
            this.lblExplanation.Text = "All Mago4 instances will be installed in a single root folder. You can set the pa" +
    "th here or you can modify it later using the Settings form";
            // 
            // lblRootFolder
            // 
            this.lblRootFolder.AutoSize = true;
            this.lblRootFolder.Location = new System.Drawing.Point(13, 59);
            this.lblRootFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRootFolder.Name = "lblRootFolder";
            this.lblRootFolder.Size = new System.Drawing.Size(81, 14);
            this.lblRootFolder.TabIndex = 9;
            this.lblRootFolder.Text = "Root folder:";
            // 
            // ChooseRootFolderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(544, 162);
            this.Controls.Add(this.lblRootFolder);
            this.Controls.Add(this.lblExplanation);
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
            this.Name = "ChooseRootFolderForm";
            this.Text = "Choose root folder for all Mago4 installations";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnRootFolder;
        private System.Windows.Forms.TextBox txtRootFolder;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblExplanation;
        private System.Windows.Forms.Label lblRootFolder;
    }
}