namespace Microarea.Mago4Butler
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.txtRootFolder = new System.Windows.Forms.TextBox();
            this.btnRootFolder = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblRootFolder = new System.Windows.Forms.Label();
            this.lblWebSites = new System.Windows.Forms.Label();
            this.cmbWebSites = new System.Windows.Forms.ComboBox();
            this.ckbAlsoDeleteCustom = new System.Windows.Forms.CheckBox();
            this.ckbCreateMsiLog = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.tabPageWebSite = new System.Windows.Forms.TabPage();
            this.tabPageProxy = new System.Windows.Forms.TabPage();
            this.grpProxy = new System.Windows.Forms.GroupBox();
            this.ckbUseCredentials = new System.Windows.Forms.CheckBox();
            this.txtProxyPort = new System.Windows.Forms.TextBox();
            this.txtServerUrl = new System.Windows.Forms.TextBox();
            this.lblProxyPort = new System.Windows.Forms.Label();
            this.lblServerUrl = new System.Windows.Forms.Label();
            this.grpCredentials = new System.Windows.Forms.GroupBox();
            this.lblDomain = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.ckbUseProxy = new System.Windows.Forms.CheckBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.tabControl.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageWebSite.SuspendLayout();
            this.tabPageProxy.SuspendLayout();
            this.grpProxy.SuspendLayout();
            this.grpCredentials.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // txtRootFolder
            // 
            this.txtRootFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRootFolder.Location = new System.Drawing.Point(7, 23);
            this.txtRootFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtRootFolder.Name = "txtRootFolder";
            this.txtRootFolder.ReadOnly = true;
            this.txtRootFolder.Size = new System.Drawing.Size(362, 22);
            this.txtRootFolder.TabIndex = 1;
            // 
            // btnRootFolder
            // 
            this.btnRootFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRootFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRootFolder.Location = new System.Drawing.Point(377, 23);
            this.btnRootFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRootFolder.Name = "btnRootFolder";
            this.btnRootFolder.Size = new System.Drawing.Size(37, 22);
            this.btnRootFolder.TabIndex = 2;
            this.btnRootFolder.Text = "...";
            this.btnRootFolder.UseVisualStyleBackColor = true;
            this.btnRootFolder.Click += new System.EventHandler(this.btnRootFolder_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Location = new System.Drawing.Point(231, 373);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 25);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(339, 373);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblRootFolder
            // 
            this.lblRootFolder.AutoSize = true;
            this.lblRootFolder.Location = new System.Drawing.Point(7, 3);
            this.lblRootFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRootFolder.Name = "lblRootFolder";
            this.lblRootFolder.Size = new System.Drawing.Size(81, 14);
            this.lblRootFolder.TabIndex = 0;
            this.lblRootFolder.Text = "Root folder:";
            // 
            // lblWebSites
            // 
            this.lblWebSites.AutoSize = true;
            this.lblWebSites.Location = new System.Drawing.Point(7, 3);
            this.lblWebSites.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWebSites.Name = "lblWebSites";
            this.lblWebSites.Size = new System.Drawing.Size(76, 14);
            this.lblWebSites.TabIndex = 0;
            this.lblWebSites.Text = "Web Sites:";
            this.lblWebSites.Visible = false;
            // 
            // cmbWebSites
            // 
            this.cmbWebSites.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWebSites.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbWebSites.FormattingEnabled = true;
            this.cmbWebSites.Location = new System.Drawing.Point(7, 22);
            this.cmbWebSites.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbWebSites.Name = "cmbWebSites";
            this.cmbWebSites.Size = new System.Drawing.Size(284, 22);
            this.cmbWebSites.TabIndex = 1;
            this.cmbWebSites.Visible = false;
            this.cmbWebSites.SelectedIndexChanged += new System.EventHandler(this.cmbWebSites_SelectedIndexChanged);
            // 
            // ckbAlsoDeleteCustom
            // 
            this.ckbAlsoDeleteCustom.AutoSize = true;
            this.ckbAlsoDeleteCustom.Location = new System.Drawing.Point(7, 65);
            this.ckbAlsoDeleteCustom.Name = "ckbAlsoDeleteCustom";
            this.ckbAlsoDeleteCustom.Size = new System.Drawing.Size(242, 18);
            this.ckbAlsoDeleteCustom.TabIndex = 3;
            this.ckbAlsoDeleteCustom.Text = "Delete \'Custom\' folder on uninstall";
            this.ckbAlsoDeleteCustom.UseVisualStyleBackColor = true;
            // 
            // ckbCreateMsiLog
            // 
            this.ckbCreateMsiLog.AutoSize = true;
            this.ckbCreateMsiLog.Location = new System.Drawing.Point(7, 89);
            this.ckbCreateMsiLog.Name = "ckbCreateMsiLog";
            this.ckbCreateMsiLog.Size = new System.Drawing.Size(117, 18);
            this.ckbCreateMsiLog.TabIndex = 4;
            this.ckbCreateMsiLog.Text = "Create msi log";
            this.ckbCreateMsiLog.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageGeneral);
            this.tabControl.Controls.Add(this.tabPageWebSite);
            this.tabControl.Controls.Add(this.tabPageProxy);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(429, 355);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.lblRootFolder);
            this.tabPageGeneral.Controls.Add(this.ckbCreateMsiLog);
            this.tabPageGeneral.Controls.Add(this.txtRootFolder);
            this.tabPageGeneral.Controls.Add(this.ckbAlsoDeleteCustom);
            this.tabPageGeneral.Controls.Add(this.btnRootFolder);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 23);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(421, 328);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // tabPageWebSite
            // 
            this.tabPageWebSite.Controls.Add(this.lblWebSites);
            this.tabPageWebSite.Controls.Add(this.cmbWebSites);
            this.tabPageWebSite.Location = new System.Drawing.Point(4, 23);
            this.tabPageWebSite.Name = "tabPageWebSite";
            this.tabPageWebSite.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWebSite.Size = new System.Drawing.Size(421, 328);
            this.tabPageWebSite.TabIndex = 1;
            this.tabPageWebSite.Text = "WebSite";
            this.tabPageWebSite.UseVisualStyleBackColor = true;
            // 
            // tabPageProxy
            // 
            this.tabPageProxy.Controls.Add(this.grpProxy);
            this.tabPageProxy.Controls.Add(this.ckbUseProxy);
            this.tabPageProxy.Location = new System.Drawing.Point(4, 23);
            this.tabPageProxy.Name = "tabPageProxy";
            this.tabPageProxy.Size = new System.Drawing.Size(421, 328);
            this.tabPageProxy.TabIndex = 2;
            this.tabPageProxy.Text = "Proxy configuration";
            this.tabPageProxy.UseVisualStyleBackColor = true;
            // 
            // grpProxy
            // 
            this.grpProxy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpProxy.Controls.Add(this.ckbUseCredentials);
            this.grpProxy.Controls.Add(this.txtProxyPort);
            this.grpProxy.Controls.Add(this.txtServerUrl);
            this.grpProxy.Controls.Add(this.lblProxyPort);
            this.grpProxy.Controls.Add(this.lblServerUrl);
            this.grpProxy.Controls.Add(this.grpCredentials);
            this.grpProxy.Enabled = false;
            this.grpProxy.Location = new System.Drawing.Point(9, 27);
            this.grpProxy.Name = "grpProxy";
            this.grpProxy.Size = new System.Drawing.Size(409, 298);
            this.grpProxy.TabIndex = 1;
            this.grpProxy.TabStop = false;
            // 
            // ckbUseCredentials
            // 
            this.ckbUseCredentials.AutoSize = true;
            this.ckbUseCredentials.Location = new System.Drawing.Point(10, 78);
            this.ckbUseCredentials.Name = "ckbUseCredentials";
            this.ckbUseCredentials.Size = new System.Drawing.Size(239, 18);
            this.ckbUseCredentials.TabIndex = 4;
            this.ckbUseCredentials.Text = "Use credentials for HTTP requests";
            this.ckbUseCredentials.UseVisualStyleBackColor = true;
            this.ckbUseCredentials.CheckedChanged += new System.EventHandler(this.ckbUseCredentials_CheckedChanged);
            // 
            // txtProxyPort
            // 
            this.txtProxyPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProxyPort.Location = new System.Drawing.Point(314, 40);
            this.txtProxyPort.Name = "txtProxyPort";
            this.txtProxyPort.Size = new System.Drawing.Size(69, 22);
            this.txtProxyPort.TabIndex = 3;
            // 
            // txtServerUrl
            // 
            this.txtServerUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServerUrl.Location = new System.Drawing.Point(10, 40);
            this.txtServerUrl.Name = "txtServerUrl";
            this.txtServerUrl.Size = new System.Drawing.Size(296, 22);
            this.txtServerUrl.TabIndex = 1;
            // 
            // lblProxyPort
            // 
            this.lblProxyPort.AutoSize = true;
            this.lblProxyPort.Location = new System.Drawing.Point(311, 22);
            this.lblProxyPort.Name = "lblProxyPort";
            this.lblProxyPort.Size = new System.Drawing.Size(72, 14);
            this.lblProxyPort.TabIndex = 2;
            this.lblProxyPort.Text = "Proxy port";
            // 
            // lblServerUrl
            // 
            this.lblServerUrl.AutoSize = true;
            this.lblServerUrl.Location = new System.Drawing.Point(7, 22);
            this.lblServerUrl.Name = "lblServerUrl";
            this.lblServerUrl.Size = new System.Drawing.Size(106, 14);
            this.lblServerUrl.TabIndex = 0;
            this.lblServerUrl.Text = "Proxy server url";
            // 
            // grpCredentials
            // 
            this.grpCredentials.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpCredentials.Controls.Add(this.lblDomain);
            this.grpCredentials.Controls.Add(this.lblUsername);
            this.grpCredentials.Controls.Add(this.lblPassword);
            this.grpCredentials.Controls.Add(this.txtPassword);
            this.grpCredentials.Controls.Add(this.txtDomain);
            this.grpCredentials.Controls.Add(this.txtUsername);
            this.grpCredentials.Location = new System.Drawing.Point(10, 92);
            this.grpCredentials.Name = "grpCredentials";
            this.grpCredentials.Size = new System.Drawing.Size(393, 200);
            this.grpCredentials.TabIndex = 5;
            this.grpCredentials.TabStop = false;
            // 
            // lblDomain
            // 
            this.lblDomain.AutoSize = true;
            this.lblDomain.Location = new System.Drawing.Point(6, 18);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new System.Drawing.Size(54, 14);
            this.lblDomain.TabIndex = 0;
            this.lblDomain.Text = "Domain";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(6, 61);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(71, 14);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Username";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(6, 104);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(69, 14);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(9, 122);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(364, 22);
            this.txtPassword.TabIndex = 5;
            // 
            // txtDomain
            // 
            this.txtDomain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDomain.Location = new System.Drawing.Point(9, 36);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(364, 22);
            this.txtDomain.TabIndex = 1;
            // 
            // txtUsername
            // 
            this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsername.Location = new System.Drawing.Point(9, 79);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(364, 22);
            this.txtUsername.TabIndex = 3;
            // 
            // ckbUseProxy
            // 
            this.ckbUseProxy.AutoSize = true;
            this.ckbUseProxy.Location = new System.Drawing.Point(9, 12);
            this.ckbUseProxy.Name = "ckbUseProxy";
            this.ckbUseProxy.Size = new System.Drawing.Size(209, 18);
            this.ckbUseProxy.TabIndex = 0;
            this.ckbUseProxy.Text = "Configure proxy for web calls";
            this.ckbUseProxy.UseVisualStyleBackColor = true;
            this.ckbUseProxy.CheckedChanged += new System.EventHandler(this.ckbUseProxy_CheckedChanged);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(453, 410);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.tabControl.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.tabPageWebSite.ResumeLayout(false);
            this.tabPageWebSite.PerformLayout();
            this.tabPageProxy.ResumeLayout(false);
            this.tabPageProxy.PerformLayout();
            this.grpProxy.ResumeLayout(false);
            this.grpProxy.PerformLayout();
            this.grpCredentials.ResumeLayout(false);
            this.grpCredentials.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtRootFolder;
        private System.Windows.Forms.Button btnRootFolder;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblRootFolder;
        private System.Windows.Forms.Label lblWebSites;
        private System.Windows.Forms.ComboBox cmbWebSites;
        private System.Windows.Forms.CheckBox ckbAlsoDeleteCustom;
        private System.Windows.Forms.CheckBox ckbCreateMsiLog;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageWebSite;
        private System.Windows.Forms.TabPage tabPageProxy;
        private System.Windows.Forms.GroupBox grpProxy;
        private System.Windows.Forms.CheckBox ckbUseProxy;
        private System.Windows.Forms.TextBox txtServerUrl;
        private System.Windows.Forms.Label lblServerUrl;
        private System.Windows.Forms.TextBox txtProxyPort;
        private System.Windows.Forms.Label lblProxyPort;
        private System.Windows.Forms.CheckBox ckbUseCredentials;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtDomain;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblDomain;
        private System.Windows.Forms.GroupBox grpCredentials;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}