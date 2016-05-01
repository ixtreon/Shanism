namespace ShanoEditor.Views
{
    partial class TexturesView
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
            this.pSplitter = new System.Windows.Forms.SplitContainer();
            this.texBrowser = new ShanoEditor.Views.Content.TextureBrowser();
            this.pDetailSplitter = new System.Windows.Forms.SplitContainer();
            this.pTexProps = new System.Windows.Forms.PropertyGrid();
            this.texView = new ShanoEditor.Views.Models.TextureBox();
            this.texStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.singleStaticAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aDynamicAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.multipleStaticAnimationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pSplitter)).BeginInit();
            this.pSplitter.Panel1.SuspendLayout();
            this.pSplitter.Panel2.SuspendLayout();
            this.pSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pDetailSplitter)).BeginInit();
            this.pDetailSplitter.Panel1.SuspendLayout();
            this.pDetailSplitter.Panel2.SuspendLayout();
            this.pDetailSplitter.SuspendLayout();
            this.texStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pSplitter
            // 
            this.pSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.pSplitter.Location = new System.Drawing.Point(0, 0);
            this.pSplitter.Name = "pSplitter";
            // 
            // pSplitter.Panel1
            // 
            this.pSplitter.Panel1.Controls.Add(this.texBrowser);
            // 
            // pSplitter.Panel2
            // 
            this.pSplitter.Panel2.Controls.Add(this.pDetailSplitter);
            this.pSplitter.Size = new System.Drawing.Size(522, 397);
            this.pSplitter.SplitterDistance = 191;
            this.pSplitter.TabIndex = 1;
            // 
            // texBrowser
            // 
            this.texBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texBrowser.Location = new System.Drawing.Point(0, 0);
            this.texBrowser.Name = "texBrowser";
            this.texBrowser.Size = new System.Drawing.Size(191, 397);
            this.texBrowser.TabIndex = 0;
            // 
            // pDetailSplitter
            // 
            this.pDetailSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pDetailSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.pDetailSplitter.Location = new System.Drawing.Point(0, 0);
            this.pDetailSplitter.Name = "pDetailSplitter";
            // 
            // pDetailSplitter.Panel1
            // 
            this.pDetailSplitter.Panel1.Controls.Add(this.pTexProps);
            // 
            // pDetailSplitter.Panel2
            // 
            this.pDetailSplitter.Panel2.Controls.Add(this.texView);
            this.pDetailSplitter.Size = new System.Drawing.Size(327, 397);
            this.pDetailSplitter.SplitterDistance = 203;
            this.pDetailSplitter.TabIndex = 0;
            // 
            // pTexProps
            // 
            this.pTexProps.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pTexProps.CommandsVisibleIfAvailable = false;
            this.pTexProps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pTexProps.HelpVisible = false;
            this.pTexProps.Location = new System.Drawing.Point(0, 0);
            this.pTexProps.Margin = new System.Windows.Forms.Padding(0);
            this.pTexProps.Name = "pTexProps";
            this.pTexProps.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.pTexProps.Size = new System.Drawing.Size(203, 397);
            this.pTexProps.TabIndex = 0;
            this.pTexProps.ToolbarVisible = false;
            this.pTexProps.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pTexProps_PropertyValueChanged);
            // 
            // texView
            // 
            this.texView.CanSelectLogical = true;
            this.texView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texView.Location = new System.Drawing.Point(0, 0);
            this.texView.Name = "texView";
            this.texView.Size = new System.Drawing.Size(120, 397);
            this.texView.StickySelection = false;
            this.texView.TabIndex = 1;
            this.texView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.texView_MouseClick);
            // 
            // texStrip
            // 
            this.texStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem});
            this.texStrip.Name = "texStrip";
            this.texStrip.Size = new System.Drawing.Size(153, 48);
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.singleStaticAnimationToolStripMenuItem,
            this.aDynamicAnimationToolStripMenuItem,
            this.multipleStaticAnimationsToolStripMenuItem});
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.createToolStripMenuItem.Text = "Create...";
            // 
            // singleStaticAnimationToolStripMenuItem
            // 
            this.singleStaticAnimationToolStripMenuItem.Name = "singleStaticAnimationToolStripMenuItem";
            this.singleStaticAnimationToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.singleStaticAnimationToolStripMenuItem.Text = "A static animation";
            this.singleStaticAnimationToolStripMenuItem.Click += new System.EventHandler(this.aStaticAnimationToolStripMenuItem_Click);
            // 
            // aDynamicAnimationToolStripMenuItem
            // 
            this.aDynamicAnimationToolStripMenuItem.Name = "aDynamicAnimationToolStripMenuItem";
            this.aDynamicAnimationToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.aDynamicAnimationToolStripMenuItem.Text = "A dynamic animation";
            this.aDynamicAnimationToolStripMenuItem.Click += new System.EventHandler(this.aDynamicAnimationToolStripMenuItem_Click);
            // 
            // multipleStaticAnimationsToolStripMenuItem
            // 
            this.multipleStaticAnimationsToolStripMenuItem.Name = "multipleStaticAnimationsToolStripMenuItem";
            this.multipleStaticAnimationsToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.multipleStaticAnimationsToolStripMenuItem.Text = "Multiple static animations";
            this.multipleStaticAnimationsToolStripMenuItem.Click += new System.EventHandler(this.multipleStaticAnimationsToolStripMenuItem_Click);
            // 
            // TexturesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pSplitter);
            this.Name = "TexturesView";
            this.Size = new System.Drawing.Size(522, 397);
            this.pSplitter.Panel1.ResumeLayout(false);
            this.pSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pSplitter)).EndInit();
            this.pSplitter.ResumeLayout(false);
            this.pDetailSplitter.Panel1.ResumeLayout(false);
            this.pDetailSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pDetailSplitter)).EndInit();
            this.pDetailSplitter.ResumeLayout(false);
            this.texStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer pSplitter;
        private System.Windows.Forms.SplitContainer pDetailSplitter;
        private System.Windows.Forms.PropertyGrid pTexProps;
        private Models.TextureBox texView;
        private Content.TextureBrowser texBrowser;
        private System.Windows.Forms.ToolStripMenuItem singleStaticAnimationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aDynamicAnimationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem multipleStaticAnimationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip texStrip;
    }
}
