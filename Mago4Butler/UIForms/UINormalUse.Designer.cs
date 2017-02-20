namespace Microarea.Mago4Butler
{
    partial class UINormalUse
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
            if (disposing)
            {
                if (this.listViewSortManager != null)
                {
                    this.listViewSortManager.Dispose();
                    this.listViewSortManager = null;
                }
                if (components != null)
                {
                    components.Dispose();
                    components = null;
                }
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UINormalUse));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.lsvInstances = new System.Windows.Forms.ListView();
            this.instanceColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.versionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.installedOnColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAddInstance = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.IsSplitterFixed = true;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.lsvInstances);
            this.splitContainer.Panel1.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.btnRemove);
            this.splitContainer.Panel2.Controls.Add(this.btnAddInstance);
            this.splitContainer.Panel2.Controls.Add(this.btnUpdate);
            this.splitContainer.Size = new System.Drawing.Size(500, 400);
            this.splitContainer.SplitterDistance = 345;
            this.splitContainer.TabIndex = 0;
            // 
            // lsvInstances
            // 
            this.lsvInstances.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvInstances.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.instanceColumnHeader,
            this.versionColumnHeader,
            this.installedOnColumnHeader});
            this.lsvInstances.ContextMenuStrip = this.contextMenuStrip;
            this.lsvInstances.FullRowSelect = true;
            this.lsvInstances.Location = new System.Drawing.Point(20, 0);
            this.lsvInstances.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lsvInstances.Name = "lsvInstances";
            this.lsvInstances.ShowItemToolTips = true;
            this.lsvInstances.Size = new System.Drawing.Size(456, 345);
            this.lsvInstances.TabIndex = 0;
            this.lsvInstances.UseCompatibleStateImageBehavior = false;
            this.lsvInstances.View = System.Windows.Forms.View.Details;
            this.lsvInstances.SelectedIndexChanged += new System.EventHandler(this.lsvInstances_SelectedIndexChanged);
            this.lsvInstances.DoubleClick += new System.EventHandler(this.lsvInstances_DoubleClick);
            // 
            // instanceColumnHeader
            // 
            this.instanceColumnHeader.Text = "Instance";
            this.instanceColumnHeader.Width = 25;
            // 
            // versionColumnHeader
            // 
            this.versionColumnHeader.Text = "Version";
            this.versionColumnHeader.Width = 25;
            // 
            // installedOnColumnHeader
            // 
            this.installedOnColumnHeader.Text = "Installed on";
            this.installedOnColumnHeader.Width = 25;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Enabled = false;
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.Image")));
            this.btnRemove.Location = new System.Drawing.Point(303, 7);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(40, 32);
            this.btnRemove.TabIndex = 0;
            this.toolTip.SetToolTip(this.btnRemove, "Remove instance");
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAddInstance
            // 
            this.btnAddInstance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddInstance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddInstance.Image = ((System.Drawing.Image)(resources.GetObject("btnAddInstance.Image")));
            this.btnAddInstance.Location = new System.Drawing.Point(159, 7);
            this.btnAddInstance.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAddInstance.Name = "btnAddInstance";
            this.btnAddInstance.Size = new System.Drawing.Size(40, 32);
            this.btnAddInstance.TabIndex = 0;
            this.toolTip.SetToolTip(this.btnAddInstance, "Add instance");
            this.btnAddInstance.UseVisualStyleBackColor = true;
            this.btnAddInstance.Click += new System.EventHandler(this.btnAddInstance_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Enabled = false;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.Location = new System.Drawing.Point(231, 7);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(40, 32);
            this.btnUpdate.TabIndex = 0;
            this.toolTip.SetToolTip(this.btnUpdate, "Update instance");
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // UINormalUse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.splitContainer);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximumSize = new System.Drawing.Size(500, 400);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "UINormalUse";
            this.Size = new System.Drawing.Size(500, 400);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ListView lsvInstances;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnAddInstance;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ColumnHeader instanceColumnHeader;
        private System.Windows.Forms.ColumnHeader versionColumnHeader;
        private System.Windows.Forms.ColumnHeader installedOnColumnHeader;
    }
}
