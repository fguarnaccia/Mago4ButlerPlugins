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
            this.components = new System.ComponentModel.Container();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.lsvInstances = new System.Windows.Forms.ListView();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAddInstance = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
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
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.lsvInstances);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.btnRemove);
            this.splitContainer.Panel2.Controls.Add(this.btnAddInstance);
            this.splitContainer.Panel2.Controls.Add(this.btnUpdate);
            this.splitContainer.Size = new System.Drawing.Size(400, 400);
            this.splitContainer.SplitterDistance = 345;
            this.splitContainer.TabIndex = 0;
            // 
            // lsvInstances
            // 
            this.lsvInstances.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvInstances.Location = new System.Drawing.Point(0, 0);
            this.lsvInstances.Name = "lsvInstances";
            this.lsvInstances.Size = new System.Drawing.Size(400, 345);
            this.lsvInstances.TabIndex = 0;
            this.lsvInstances.UseCompatibleStateImageBehavior = false;
            this.lsvInstances.View = System.Windows.Forms.View.List;
            this.lsvInstances.SelectedIndexChanged += new System.EventHandler(this.lsvInstances_SelectedIndexChanged);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Enabled = false;
            this.btnRemove.Image = global::Microarea.Mago4Butler.Properties.Resources.TrashCan;
            this.btnRemove.Location = new System.Drawing.Point(245, 0);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(48, 48);
            this.btnRemove.TabIndex = 0;
            this.toolTip.SetToolTip(this.btnRemove, "Remove instance");
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAddInstance
            // 
            this.btnAddInstance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddInstance.Image = global::Microarea.Mago4Butler.Properties.Resources.Add;
            this.btnAddInstance.Location = new System.Drawing.Point(137, 0);
            this.btnAddInstance.Name = "btnAddInstance";
            this.btnAddInstance.Size = new System.Drawing.Size(48, 48);
            this.btnAddInstance.TabIndex = 0;
            this.toolTip.SetToolTip(this.btnAddInstance, "Add instance");
            this.btnAddInstance.UseVisualStyleBackColor = true;
            this.btnAddInstance.Click += new System.EventHandler(this.btnAddInstance_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Image = global::Microarea.Mago4Butler.Properties.Resources.Update;
            this.btnUpdate.Location = new System.Drawing.Point(191, 0);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(48, 48);
            this.btnUpdate.TabIndex = 0;
            this.toolTip.SetToolTip(this.btnUpdate, "Update instance");
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // UINormalUse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.MaximumSize = new System.Drawing.Size(400, 400);
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "UINormalUse";
            this.Size = new System.Drawing.Size(400, 400);
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
    }
}
