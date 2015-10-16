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
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("");
            this.scenarioList = new ShanoRPGWin.UI.Scenarios.LibTree();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.scenarioListControlPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAddLibrary = new System.Windows.Forms.Button();
            this.scenarioDetails1 = new ShanoRPGWin.UI.Scenarios.ScenarioDetails();
            this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.scenarioListControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // scenarioList
            // 
            this.scenarioList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scenarioList.Location = new System.Drawing.Point(0, 22);
            this.scenarioList.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.scenarioList.Name = "scenarioList";
            treeNode2.Name = "";
            treeNode2.Text = "";
            this.scenarioList.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.scenarioList.Size = new System.Drawing.Size(121, 292);
            this.scenarioList.TabIndex = 0;
            // 
            // FormLibrary
            // 
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 9);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.scenarioListControlPanel);
            this.splitContainer1.Panel1.Controls.Add(this.scenarioList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.scenarioDetails1);
            this.splitContainer1.Size = new System.Drawing.Size(364, 314);
            this.splitContainer1.SplitterDistance = 120;
            this.splitContainer1.TabIndex = 1;
            // 
            // scenarioListControlPanel
            // 
            this.scenarioListControlPanel.ColumnCount = 2;
            this.scenarioListControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.scenarioListControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.scenarioListControlPanel.Controls.Add(this.btnRemove, 1, 0);
            this.scenarioListControlPanel.Controls.Add(this.btnAddLibrary, 0, 0);
            this.scenarioListControlPanel.Location = new System.Drawing.Point(1, 0);
            this.scenarioListControlPanel.Margin = new System.Windows.Forms.Padding(0);
            this.scenarioListControlPanel.Name = "scenarioListControlPanel";
            this.scenarioListControlPanel.RowCount = 1;
            this.scenarioListControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.scenarioListControlPanel.Size = new System.Drawing.Size(119, 22);
            this.scenarioListControlPanel.TabIndex = 2;
            // 
            // btnRemove
            // 
            this.btnRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemove.Location = new System.Drawing.Point(59, 0);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(0);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(60, 22);
            this.btnRemove.TabIndex = 1;
            this.btnRemove.Text = "-";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnAddLibrary
            // 
            this.btnAddLibrary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddLibrary.Location = new System.Drawing.Point(0, 0);
            this.btnAddLibrary.Margin = new System.Windows.Forms.Padding(0);
            this.btnAddLibrary.Name = "btnAddLibrary";
            this.btnAddLibrary.Size = new System.Drawing.Size(59, 22);
            this.btnAddLibrary.TabIndex = 0;
            this.btnAddLibrary.Text = "+";
            this.btnAddLibrary.UseVisualStyleBackColor = true;
            this.btnAddLibrary.Click += new System.EventHandler(this.btnAddLibrary_Click);
            // 
            // scenarioDetails1
            // 
            this.scenarioDetails1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scenarioDetails1.Location = new System.Drawing.Point(0, 0);
            this.scenarioDetails1.Name = "scenarioDetails1";
            this.scenarioDetails1.Size = new System.Drawing.Size(240, 314);
            this.scenarioDetails1.TabIndex = 0;
            // 
            // ScenarioDirForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 335);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ScenarioDirForm";
            this.Text = "ScenarioDirForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.scenarioListControlPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Scenarios.LibTree scenarioList;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel scenarioListControlPanel;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAddLibrary;
        private Scenarios.ScenarioDetails scenarioDetails1;
        private System.Windows.Forms.FolderBrowserDialog folderDialog;
    }
}