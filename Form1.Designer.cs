namespace MSIChecker
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnScan = new System.Windows.Forms.Button();
            this.grid = new System.Windows.Forms.DataGridView();
            this.ctx = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itmSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.itmProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.itmUninstall = new System.Windows.Forms.ToolStripMenuItem();
            this.itmRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.itmRegistry = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.cProductname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cGUID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFilename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cParent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cUninstallable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.ctx.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnScan
            // 
            this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScan.Location = new System.Drawing.Point(1194, 12);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 0;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cProductname,
            this.cType,
            this.cKey,
            this.cGUID,
            this.cFilename,
            this.cSize,
            this.cStatus,
            this.cUser,
            this.cParent,
            this.cUninstallable});
            this.grid.ContextMenuStrip = this.ctx;
            this.grid.Location = new System.Drawing.Point(15, 48);
            this.grid.MultiSelect = false;
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RowHeadersVisible = false;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(1254, 410);
            this.grid.TabIndex = 1;
            // 
            // ctx
            // 
            this.ctx.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmSearch,
            this.itmProperties,
            this.itmUninstall,
            this.itmRemove,
            this.itmRegistry});
            this.ctx.Name = "ctx";
            this.ctx.Size = new System.Drawing.Size(180, 114);
            // 
            // itmSearch
            // 
            this.itmSearch.Name = "itmSearch";
            this.itmSearch.Size = new System.Drawing.Size(179, 22);
            this.itmSearch.Text = "Search GUID";
            this.itmSearch.Click += new System.EventHandler(this.itmSearch_Click);
            // 
            // itmProperties
            // 
            this.itmProperties.Name = "itmProperties";
            this.itmProperties.Size = new System.Drawing.Size(179, 22);
            this.itmProperties.Text = "MSI/MSP Properties";
            this.itmProperties.Click += new System.EventHandler(this.itmProperties_Click);
            // 
            // itmUninstall
            // 
            this.itmUninstall.Name = "itmUninstall";
            this.itmUninstall.Size = new System.Drawing.Size(179, 22);
            this.itmUninstall.Text = "Uninstall";
            this.itmUninstall.Click += new System.EventHandler(this.itmUninstall_Click);
            // 
            // itmRemove
            // 
            this.itmRemove.Name = "itmRemove";
            this.itmRemove.Size = new System.Drawing.Size(179, 22);
            this.itmRemove.Text = "Remove File";
            this.itmRemove.Click += new System.EventHandler(this.itmRemove_Click);
            // 
            // itmRegistry
            // 
            this.itmRegistry.Name = "itmRegistry";
            this.itmRegistry.Size = new System.Drawing.Size(179, 22);
            this.itmRegistry.Text = "Cleanup Registry";
            this.itmRegistry.Click += new System.EventHandler(this.itmRegistry_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(459, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Installer\\UserData";
            // 
            // lblSize
            // 
            this.lblSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSize.Location = new System.Drawing.Point(1044, 461);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(103, 23);
            this.lblSize.TabIndex = 3;
            this.lblSize.Text = "0";
            this.lblSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cProductname
            // 
            this.cProductname.HeaderText = "Product";
            this.cProductname.Name = "cProductname";
            this.cProductname.ReadOnly = true;
            this.cProductname.Width = 300;
            // 
            // cType
            // 
            this.cType.HeaderText = "Type";
            this.cType.Name = "cType";
            this.cType.ReadOnly = true;
            this.cType.Width = 50;
            // 
            // cKey
            // 
            this.cKey.HeaderText = "Key";
            this.cKey.Name = "cKey";
            this.cKey.ReadOnly = true;
            this.cKey.Width = 240;
            // 
            // cGUID
            // 
            this.cGUID.HeaderText = "GUID";
            this.cGUID.Name = "cGUID";
            this.cGUID.ReadOnly = true;
            this.cGUID.Width = 240;
            // 
            // cFilename
            // 
            this.cFilename.HeaderText = "File Name";
            this.cFilename.Name = "cFilename";
            this.cFilename.ReadOnly = true;
            this.cFilename.Width = 200;
            // 
            // cSize
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N0";
            this.cSize.DefaultCellStyle = dataGridViewCellStyle1;
            this.cSize.HeaderText = "Size";
            this.cSize.Name = "cSize";
            this.cSize.ReadOnly = true;
            // 
            // cStatus
            // 
            this.cStatus.HeaderText = "Status";
            this.cStatus.Name = "cStatus";
            this.cStatus.ReadOnly = true;
            // 
            // cUser
            // 
            this.cUser.HeaderText = "User";
            this.cUser.Name = "cUser";
            this.cUser.ReadOnly = true;
            // 
            // cParent
            // 
            this.cParent.HeaderText = "Parent";
            this.cParent.Name = "cParent";
            this.cParent.ReadOnly = true;
            this.cParent.Width = 240;
            // 
            // cUninstallable
            // 
            this.cUninstallable.HeaderText = "Uninstallable";
            this.cUninstallable.Name = "cUninstallable";
            this.cUninstallable.ReadOnly = true;
            this.cUninstallable.Width = 80;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1281, 485);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.btnScan);
            this.Name = "Form1";
            this.Text = "MSIChecker";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ctx.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip ctx;
        private System.Windows.Forms.ToolStripMenuItem itmSearch;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.ToolStripMenuItem itmProperties;
        private System.Windows.Forms.ToolStripMenuItem itmUninstall;
        private System.Windows.Forms.ToolStripMenuItem itmRemove;
        private System.Windows.Forms.ToolStripMenuItem itmRegistry;
        private System.Windows.Forms.DataGridViewTextBoxColumn cProductname;
        private System.Windows.Forms.DataGridViewTextBoxColumn cType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn cGUID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFilename;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn cStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn cUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn cParent;
        private System.Windows.Forms.DataGridViewTextBoxColumn cUninstallable;
    }
}

