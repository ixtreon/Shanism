namespace AbilityIDE
{
    partial class ShanoEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShanoEditor));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.scenarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dasToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cCodeEditor = new System.Windows.Forms.RichTextBox();
            this.pSplitCode = new System.Windows.Forms.SplitContainer();
            this.treeCode = new AbilityIDE.ScenarioTreeView();
            this.stripCode = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.openDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpScenario = new System.Windows.Forms.TabPage();
            this.tpContent = new System.Windows.Forms.TabPage();
            this.tpCode = new System.Windows.Forms.TabPage();
            this.pSplitContent = new System.Windows.Forms.SplitContainer();
            this.treeContent = new AbilityIDE.ScenarioTreeView();
            this.stripContent = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeScenario = new AbilityIDE.ScenarioTreeView();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pSplitCode)).BeginInit();
            this.pSplitCode.Panel1.SuspendLayout();
            this.pSplitCode.Panel2.SuspendLayout();
            this.pSplitCode.SuspendLayout();
            this.stripCode.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpScenario.SuspendLayout();
            this.tpContent.SuspendLayout();
            this.tpCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pSplitContent)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.pSplitContent.SuspendLayout();
            this.stripContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scenarioToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1107, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // scenarioToolStripMenuItem
            // 
            this.scenarioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.recentToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.scenarioToolStripMenuItem.Name = "scenarioToolStripMenuItem";
            this.scenarioToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.scenarioToolStripMenuItem.Text = "&Scenario";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // recentToolStripMenuItem
            // 
            this.recentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dasToolStripMenuItem,
            this.dasToolStripMenuItem1});
            this.recentToolStripMenuItem.Name = "recentToolStripMenuItem";
            this.recentToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.recentToolStripMenuItem.Text = "Recent";
            // 
            // dasToolStripMenuItem
            // 
            this.dasToolStripMenuItem.Name = "dasToolStripMenuItem";
            this.dasToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.dasToolStripMenuItem.Text = "das";
            // 
            // dasToolStripMenuItem1
            // 
            this.dasToolStripMenuItem1.Name = "dasToolStripMenuItem1";
            this.dasToolStripMenuItem1.Size = new System.Drawing.Size(92, 22);
            this.dasToolStripMenuItem1.Text = "das";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(143, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator3});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(141, 6);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customizeToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // customizeToolStripMenuItem
            // 
            this.customizeToolStripMenuItem.Enabled = false;
            this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
            this.customizeToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.customizeToolStripMenuItem.Text = "&Customize";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Enabled = false;
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.contentsToolStripMenuItem.Text = "&Contents";
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.Enabled = false;
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.indexToolStripMenuItem.Text = "&Index";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Enabled = false;
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.searchToolStripMenuItem.Text = "&Search";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(119, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            // 
            // cCodeEditor
            // 
            this.cCodeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cCodeEditor.Location = new System.Drawing.Point(0, 0);
            this.cCodeEditor.Name = "cCodeEditor";
            this.cCodeEditor.Size = new System.Drawing.Size(725, 669);
            this.cCodeEditor.TabIndex = 1;
            this.cCodeEditor.Text = "";
            // 
            // pSplitCode
            // 
            this.pSplitCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSplitCode.Location = new System.Drawing.Point(3, 3);
            this.pSplitCode.Name = "pSplitCode";
            // 
            // pSplitCode.Panel1
            // 
            this.pSplitCode.Panel1.Controls.Add(this.treeCode);
            // 
            // pSplitCode.Panel2
            // 
            this.pSplitCode.Panel2.Controls.Add(this.stripCode);
            this.pSplitCode.Panel2.Controls.Add(this.cCodeEditor);
            this.pSplitCode.Size = new System.Drawing.Size(1093, 669);
            this.pSplitCode.SplitterDistance = 364;
            this.pSplitCode.TabIndex = 3;
            // 
            // treeCode
            // 
            this.treeCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeCode.Location = new System.Drawing.Point(0, 0);
            this.treeCode.Name = "treeCode";
            this.treeCode.Size = new System.Drawing.Size(364, 669);
            this.treeCode.TabIndex = 2;
            this.treeCode.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.scenarioView1_AfterSelect);
            // 
            // stripCode
            // 
            this.stripCode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.stripCode.Location = new System.Drawing.Point(0, 0);
            this.stripCode.Name = "stripCode";
            this.stripCode.Size = new System.Drawing.Size(725, 25);
            this.stripCode.TabIndex = 2;
            this.stripCode.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "WTF WE";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpScenario);
            this.tabControl1.Controls.Add(this.tpContent);
            this.tabControl1.Controls.Add(this.tpCode);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1107, 701);
            this.tabControl1.TabIndex = 4;
            // 
            // tpScenario
            // 
            this.tpScenario.BackColor = System.Drawing.SystemColors.Control;
            this.tpScenario.Controls.Add(this.splitContainer1);
            this.tpScenario.Location = new System.Drawing.Point(4, 22);
            this.tpScenario.Name = "tpScenario";
            this.tpScenario.Padding = new System.Windows.Forms.Padding(3);
            this.tpScenario.Size = new System.Drawing.Size(1099, 675);
            this.tpScenario.TabIndex = 2;
            this.tpScenario.Text = "Scenario";
            // 
            // tpContent
            // 
            this.tpContent.BackColor = System.Drawing.SystemColors.Control;
            this.tpContent.Controls.Add(this.pSplitContent);
            this.tpContent.Location = new System.Drawing.Point(4, 22);
            this.tpContent.Name = "tpContent";
            this.tpContent.Padding = new System.Windows.Forms.Padding(3);
            this.tpContent.Size = new System.Drawing.Size(1099, 675);
            this.tpContent.TabIndex = 1;
            this.tpContent.Text = "Content";
            // 
            // tpCode
            // 
            this.tpCode.BackColor = System.Drawing.SystemColors.Control;
            this.tpCode.Controls.Add(this.pSplitCode);
            this.tpCode.Location = new System.Drawing.Point(4, 22);
            this.tpCode.Name = "tpCode";
            this.tpCode.Padding = new System.Windows.Forms.Padding(3);
            this.tpCode.Size = new System.Drawing.Size(1099, 675);
            this.tpCode.TabIndex = 0;
            this.tpCode.Text = "Code";
            // 
            // pSplitContent
            // 
            this.pSplitContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSplitContent.Location = new System.Drawing.Point(3, 3);
            this.pSplitContent.Name = "pSplitContent";
            // 
            // pSplitContent.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeContent);
            // 
            // pSplitContent.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.stripContent);
            this.pSplitContent.Size = new System.Drawing.Size(1093, 669);
            this.pSplitContent.SplitterDistance = 364;
            this.pSplitContent.TabIndex = 4;
            // 
            // treeContent
            // 
            this.treeContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeContent.Location = new System.Drawing.Point(0, 0);
            this.treeContent.Name = "treeContent";
            this.treeContent.Size = new System.Drawing.Size(364, 669);
            this.treeContent.TabIndex = 2;
            // 
            // stripContent
            // 
            this.stripContent.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2});
            this.stripContent.Location = new System.Drawing.Point(0, 0);
            this.stripContent.Name = "stripContent";
            this.stripContent.Size = new System.Drawing.Size(725, 25);
            this.stripContent.TabIndex = 2;
            this.stripContent.Text = "toolStrip2";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "WTF WE";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeScenario);
            this.splitContainer1.Size = new System.Drawing.Size(1093, 669);
            this.splitContainer1.SplitterDistance = 364;
            this.splitContainer1.TabIndex = 4;
            // 
            // treeScenario
            // 
            this.treeScenario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeScenario.Location = new System.Drawing.Point(0, 0);
            this.treeScenario.Name = "treeScenario";
            this.treeScenario.Size = new System.Drawing.Size(364, 669);
            this.treeScenario.TabIndex = 2;
            // 
            // ShanoEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1107, 725);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ShanoEditor";
            this.Text = "ShanoEditor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pSplitCode.Panel1.ResumeLayout(false);
            this.pSplitCode.Panel2.ResumeLayout(false);
            this.pSplitCode.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pSplitCode)).EndInit();
            this.pSplitCode.ResumeLayout(false);
            this.stripCode.ResumeLayout(false);
            this.stripCode.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tpScenario.ResumeLayout(false);
            this.tpContent.ResumeLayout(false);
            this.tpCode.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pSplitContent)).EndInit();
            this.pSplitContent.ResumeLayout(false);
            this.stripContent.ResumeLayout(false);
            this.stripContent.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem scenarioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.RichTextBox cCodeEditor;
        private ScenarioTreeView treeCode;
        private System.Windows.Forms.SplitContainer pSplitCode;
        private System.Windows.Forms.FolderBrowserDialog openDialog;
        private System.Windows.Forms.ToolStrip stripCode;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem recentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dasToolStripMenuItem1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpScenario;
        private System.Windows.Forms.TabPage tpContent;
        private System.Windows.Forms.TabPage tpCode;
        private System.Windows.Forms.SplitContainer pSplitContent;
        private ScenarioTreeView treeContent;
        private System.Windows.Forms.ToolStrip stripContent;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ScenarioTreeView treeScenario;
    }
}

