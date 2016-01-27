namespace Microarea.Mago4Butler
{
    partial class AskForParametersForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AskForParametersForm));
            this.txtInstanceName = new System.Windows.Forms.TextBox();
            this.lblInstanceName = new System.Windows.Forms.Label();
            this.txtMsiFullPath = new System.Windows.Forms.TextBox();
            this.lblMsiFullPath = new System.Windows.Forms.Label();
            this.btnOpenFileDialog = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.errorProviderInstanceName = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderInstanceName)).BeginInit();
            this.SuspendLayout();
            // 
            // txtInstanceName
            // 
            this.txtInstanceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInstanceName.Location = new System.Drawing.Point(125, 27);
            this.txtInstanceName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtInstanceName.Name = "txtInstanceName";
            this.txtInstanceName.Size = new System.Drawing.Size(354, 22);
            this.txtInstanceName.TabIndex = 1;
            this.txtInstanceName.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // lblInstanceName
            // 
            this.lblInstanceName.AutoSize = true;
            this.lblInstanceName.Location = new System.Drawing.Point(16, 27);
            this.lblInstanceName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInstanceName.Name = "lblInstanceName";
            this.lblInstanceName.Size = new System.Drawing.Size(107, 14);
            this.lblInstanceName.TabIndex = 0;
            this.lblInstanceName.Text = "Instance Name:";
            this.lblInstanceName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMsiFullPath
            // 
            this.txtMsiFullPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMsiFullPath.Location = new System.Drawing.Point(125, 70);
            this.txtMsiFullPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtMsiFullPath.Name = "txtMsiFullPath";
            this.txtMsiFullPath.ReadOnly = true;
            this.txtMsiFullPath.Size = new System.Drawing.Size(354, 22);
            this.txtMsiFullPath.TabIndex = 3;
            this.txtMsiFullPath.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // lblMsiFullPath
            // 
            this.lblMsiFullPath.Location = new System.Drawing.Point(16, 70);
            this.lblMsiFullPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMsiFullPath.Name = "lblMsiFullPath";
            this.lblMsiFullPath.Size = new System.Drawing.Size(105, 14);
            this.lblMsiFullPath.TabIndex = 2;
            this.lblMsiFullPath.Text = "MSI path:";
            this.lblMsiFullPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOpenFileDialog
            // 
            this.btnOpenFileDialog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFileDialog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFileDialog.Location = new System.Drawing.Point(489, 70);
            this.btnOpenFileDialog.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOpenFileDialog.Name = "btnOpenFileDialog";
            this.btnOpenFileDialog.Size = new System.Drawing.Size(37, 22);
            this.btnOpenFileDialog.TabIndex = 4;
            this.btnOpenFileDialog.Text = "...";
            this.btnOpenFileDialog.UseVisualStyleBackColor = true;
            this.btnOpenFileDialog.Click += new System.EventHandler(this.btnOpenFileDialog_Click);
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
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(322, 124);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 25);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // errorProviderInstanceName
            // 
            this.errorProviderInstanceName.ContainerControl = this;
            // 
            // AskForParametersForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(544, 162);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOpenFileDialog);
            this.Controls.Add(this.lblMsiFullPath);
            this.Controls.Add(this.txtMsiFullPath);
            this.Controls.Add(this.lblInstanceName);
            this.Controls.Add(this.txtInstanceName);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AskForParametersForm";
            this.Text = "Parameters";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderInstanceName)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtInstanceName;
        private System.Windows.Forms.Label lblInstanceName;
        private System.Windows.Forms.TextBox txtMsiFullPath;
        private System.Windows.Forms.Label lblMsiFullPath;
        private System.Windows.Forms.Button btnOpenFileDialog;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ErrorProvider errorProviderInstanceName;
    }
}