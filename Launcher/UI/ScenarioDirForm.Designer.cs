namespace ShanoRPGWin.UI
{
    partial class ScenarioDirForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.scenarioListControlPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnAddLibrary = new System.Windows.Forms.Button();
            this.libTree = new ShanoRPGWin.UI.Scenarios.LibTree();
            this.libraryDetails = new ShanoRPGWin.UI.Scenarios.LibraryDetails();
            this.scenarioDetails = new ShanoRPGWin.UI.Scenarios.ScenarioDetails();
            this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.scenarioListControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(12, 9);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.scenarioListControlPanel);
            this.splitContainer1.Panel1.Controls.Add(this.libTree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.libraryDetails);
            this.splitContainer1.Panel2.Controls.Add(this.scenarioDetails);
            this.splitContainer1.Size = new System.Drawing.Size(518, 359);
            this.splitContainer1.SplitterDistance = 170;
            this.splitContainer1.TabIndex = 1;
            // 
            // scenarioListControlPanel
            // 
            this.scenarioListControlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scenarioListControlPanel.ColumnCount = 3;
            this.scenarioListControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.scenarioListControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.scenarioListControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.scenarioListControlPanel.Controls.Add(this.btnRemove, 0, 0);
            this.scenarioListControlPanel.Controls.Add(this.btnRefresh, 0, 0);
            this.scenarioListControlPanel.Controls.Add(this.btnAddLibrary, 0, 0);
            this.scenarioListControlPanel.Location = new System.Drawing.Point(-1, 0);
            this.scenarioListControlPanel.Margin = new System.Windows.Forms.Padding(0);
            this.scenarioListControlPanel.Name = "scenarioListControlPanel";
            this.scenarioListControlPanel.RowCount = 1;
            this.scenarioListControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.scenarioListControlPanel.Size = new System.Drawing.Size(169, 22);
            this.scenarioListControlPanel.TabIndex = 2;
            // 
            // btnRemove
            // 
            this.btnRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.Location = new System.Drawing.Point(100, 0);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(0);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(69, 22);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "-";
            this.toolTip1.SetToolTip(this.btnRemove, "Remove this. ");
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(67, 0);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(33, 22);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "↻";
            this.toolTip1.SetToolTip(this.btnRefresh, "Refresh libraries. ");
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnAddLibrary
            // 
            this.btnAddLibrary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddLibrary.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddLibrary.Location = new System.Drawing.Point(0, 0);
            this.btnAddLibrary.Margin = new System.Windows.Forms.Padding(0);
            this.btnAddLibrary.Name = "btnAddLibrary";
            this.btnAddLibrary.Size = new System.Drawing.Size(67, 22);
            this.btnAddLibrary.TabIndex = 0;
            this.btnAddLibrary.Text = "+";
            this.toolTip1.SetToolTip(this.btnAddLibrary, "Add a library. ");
            this.btnAddLibrary.UseVisualStyleBackColor = true;
            this.btnAddLibrary.Click += new System.EventHandler(this.btnAddLibrary_Click);
            // 
            // libTree
            // 
            this.libTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.libTree.Location = new System.Drawing.Point(0, 22);
            this.libTree.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.libTree.Name = "libTree";
            this.libTree.Size = new System.Drawing.Size(169, 335);
            this.libTree.TabIndex = 0;
            // 
            // libraryDetails
            // 
            this.libraryDetails.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.libraryDetails.Location = new System.Drawing.Point(22, 152);
            this.libraryDetails.Name = "libraryDetails";
            this.libraryDetails.Size = new System.Drawing.Size(132, 77);
            this.libraryDetails.TabIndex = 1;
            this.libraryDetails.Visible = false;
            // 
            // scenarioDetails
            // 
            this.scenarioDetails.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.scenarioDetails.Location = new System.Drawing.Point(6, 9);
            this.scenarioDetails.Margin = new System.Windows.Forms.Padding(0);
            this.scenarioDetails.Name = "scenarioDetails";
            this.scenarioDetails.Scenario = null;
            this.scenarioDetails.Size = new System.Drawing.Size(218, 112);
            this.scenarioDetails.TabIndex = 0;
            this.scenarioDetails.Visible = false;
            this.scenarioDetails.ScenarioSelected += new System.Action<Shanism.ScenarioLib.ScenarioConfig>(this.scenarioDetails_ScenarioSelected);
            // 
            // ScenarioDirForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 380);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ScenarioDirForm";
            this.Text = "Scenario Libraries";
            this.Load += new System.EventHandler(this.ScenarioDirForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.scenarioListControlPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Scenarios.LibTree libTree;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel scenarioListControlPanel;
        private System.Windows.Forms.Button btnAddLibrary;
        private System.Windows.Forms.FolderBrowserDialog folderDialog;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnRefresh;
        private Scenarios.ScenarioDetails scenarioDetails;
        private Scenarios.LibraryDetails libraryDetails;

    }
}