namespace ShanoEditor.ScenarioViews
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
            this.pSplitter = new System.Windows.Forms.SplitContainer();
            this.texTree = new System.Windows.Forms.TreeView();
            this.pDetailSplitter = new System.Windows.Forms.SplitContainer();
            this.texView = new Models.TextureBox();
            this.pTexProps = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.pSplitter)).BeginInit();
            this.pSplitter.Panel1.SuspendLayout();
            this.pSplitter.Panel2.SuspendLayout();
            this.pSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pDetailSplitter)).BeginInit();
            this.pDetailSplitter.Panel1.SuspendLayout();
            this.pDetailSplitter.Panel2.SuspendLayout();
            this.pDetailSplitter.SuspendLayout();
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
            this.pSplitter.Panel1.Controls.Add(this.texTree);
            // 
            // pSplitter.Panel2
            // 
            this.pSplitter.Panel2.Controls.Add(this.pDetailSplitter);
            this.pSplitter.Size = new System.Drawing.Size(522, 397);
            this.pSplitter.SplitterDistance = 191;
            this.pSplitter.TabIndex = 1;
            // 
            // texTree
            // 
            this.texTree.CheckBoxes = true;
            this.texTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texTree.Location = new System.Drawing.Point(0, 0);
            this.texTree.Margin = new System.Windows.Forms.Padding(0);
            this.texTree.Name = "texTree";
            this.texTree.Size = new System.Drawing.Size(191, 397);
            this.texTree.TabIndex = 1;
            this.texTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.texTree_AfterCheck);
            this.texTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.texTree_AfterSelect);
            // 
            // pDetailSplitter
            // 
            this.pDetailSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pDetailSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.pDetailSplitter.Location = new System.Drawing.Point(0, 0);
            this.pDetailSplitter.Name = "pDetailSplitter";
            this.pDetailSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // pDetailSplitter.Panel1
            // 
            this.pDetailSplitter.Panel1.Controls.Add(this.texView);
            // 
            // pDetailSplitter.Panel2
            // 
            this.pDetailSplitter.Panel2.Controls.Add(this.pTexProps);
            this.pDetailSplitter.Size = new System.Drawing.Size(327, 397);
            this.pDetailSplitter.SplitterDistance = 247;
            this.pDetailSplitter.TabIndex = 0;
            // 
            // texView
            // 
            this.texView.CanSelectLogical = false;
            this.texView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texView.Location = new System.Drawing.Point(0, 0);
            this.texView.Name = "texView";
            this.texView.Size = new System.Drawing.Size(327, 247);
            this.texView.TabIndex = 1;
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
            this.pTexProps.Size = new System.Drawing.Size(327, 146);
            this.pTexProps.TabIndex = 0;
            this.pTexProps.ToolbarVisible = false;
            this.pTexProps.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pTexProps_PropertyValueChanged);
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
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer pSplitter;
        private System.Windows.Forms.SplitContainer pDetailSplitter;
        private System.Windows.Forms.PropertyGrid pTexProps;
        private Models.TextureBox texView;
        private System.Windows.Forms.TreeView texTree;
    }
}
