namespace Shanism.Editor.Views
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
            this.split2 = new System.Windows.Forms.SplitContainer();
            this.texBrowser = new Shanism.Editor.Views.Content.TextureTree();
            this.propGrid = new Shanism.Editor.Util.ShanoGrid();
            this.texBox = new Shanism.Editor.Views.Models.TextureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pSplitter)).BeginInit();
            this.pSplitter.Panel1.SuspendLayout();
            this.pSplitter.Panel2.SuspendLayout();
            this.pSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split2)).BeginInit();
            this.split2.Panel1.SuspendLayout();
            this.split2.Panel2.SuspendLayout();
            this.split2.SuspendLayout();
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
            this.pSplitter.Panel1.Controls.Add(this.split2);
            // 
            // pSplitter.Panel2
            // 
            this.pSplitter.Panel2.Controls.Add(this.texBox);
            this.pSplitter.Size = new System.Drawing.Size(628, 499);
            this.pSplitter.SplitterDistance = 200;
            this.pSplitter.TabIndex = 1;
            // 
            // split2
            // 
            this.split2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.split2.Location = new System.Drawing.Point(0, 0);
            this.split2.Name = "split2";
            this.split2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split2.Panel1
            // 
            this.split2.Panel1.Controls.Add(this.texBrowser);
            // 
            // split2.Panel2
            // 
            this.split2.Panel2.Controls.Add(this.propGrid);
            this.split2.Size = new System.Drawing.Size(200, 499);
            this.split2.SplitterDistance = 249;
            this.split2.TabIndex = 0;
            // 
            // texBrowser
            // 
            this.texBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texBrowser.Location = new System.Drawing.Point(0, 0);
            this.texBrowser.Name = "texBrowser";
            this.texBrowser.Size = new System.Drawing.Size(200, 249);
            this.texBrowser.TabIndex = 0;
            // 
            // propGrid
            // 
            this.propGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propGrid.Location = new System.Drawing.Point(0, 0);
            this.propGrid.Name = "propGrid";
            this.propGrid.Size = new System.Drawing.Size(200, 246);
            this.propGrid.TabIndex = 0;
            this.propGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propGrid_PropertyValueChanged);
            // 
            // texBox
            // 
            this.texBox.CanSelectLogical = true;
            this.texBox.ContextMenuEnabled = true;
            this.texBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texBox.Location = new System.Drawing.Point(0, 0);
            this.texBox.Name = "texBox";
            this.texBox.Size = new System.Drawing.Size(424, 499);
            this.texBox.StickySelection = false;
            this.texBox.TabIndex = 0;
            // 
            // TexturesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pSplitter);
            this.Name = "TexturesView";
            this.Size = new System.Drawing.Size(628, 499);
            this.pSplitter.Panel1.ResumeLayout(false);
            this.pSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pSplitter)).EndInit();
            this.pSplitter.ResumeLayout(false);
            this.split2.Panel1.ResumeLayout(false);
            this.split2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split2)).EndInit();
            this.split2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer pSplitter;
        private Content.TextureTree texBrowser;
        private System.Windows.Forms.SplitContainer split2;
        private Util.ShanoGrid propGrid;
        private Models.TextureBox texBox;
    }
}
