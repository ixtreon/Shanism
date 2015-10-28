namespace AbilityIDE.ScenarioViews
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
            this.lTextures = new System.Windows.Forms.CheckedListBox();
            this.pSplitter = new System.Windows.Forms.SplitContainer();
            this.pDetailSplitter = new System.Windows.Forms.SplitContainer();
            this.pTexPreview = new System.Windows.Forms.PictureBox();
            this.pTexProps = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.pSplitter)).BeginInit();
            this.pSplitter.Panel1.SuspendLayout();
            this.pSplitter.Panel2.SuspendLayout();
            this.pSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pDetailSplitter)).BeginInit();
            this.pDetailSplitter.Panel1.SuspendLayout();
            this.pDetailSplitter.Panel2.SuspendLayout();
            this.pDetailSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pTexPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // lTextures
            // 
            this.lTextures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTextures.FormattingEnabled = true;
            this.lTextures.IntegralHeight = false;
            this.lTextures.Items.AddRange(new object[] {
            "edno",
            "dve",
            "tri"});
            this.lTextures.Location = new System.Drawing.Point(0, 0);
            this.lTextures.Name = "lTextures";
            this.lTextures.Size = new System.Drawing.Size(144, 352);
            this.lTextures.TabIndex = 0;
            this.lTextures.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lTextures_ItemCheck);
            this.lTextures.SelectedIndexChanged += new System.EventHandler(this.lTextures_SelectedIndexChanged);
            // 
            // pSplitter
            // 
            this.pSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSplitter.Location = new System.Drawing.Point(0, 0);
            this.pSplitter.Name = "pSplitter";
            // 
            // pSplitter.Panel1
            // 
            this.pSplitter.Panel1.Controls.Add(this.lTextures);
            // 
            // pSplitter.Panel2
            // 
            this.pSplitter.Panel2.Controls.Add(this.pDetailSplitter);
            this.pSplitter.Size = new System.Drawing.Size(434, 352);
            this.pSplitter.SplitterDistance = 144;
            this.pSplitter.TabIndex = 1;
            // 
            // pDetailSplitter
            // 
            this.pDetailSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pDetailSplitter.Location = new System.Drawing.Point(0, 0);
            this.pDetailSplitter.Name = "pDetailSplitter";
            this.pDetailSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // pDetailSplitter.Panel1
            // 
            this.pDetailSplitter.Panel1.Controls.Add(this.pTexPreview);
            // 
            // pDetailSplitter.Panel2
            // 
            this.pDetailSplitter.Panel2.Controls.Add(this.pTexProps);
            this.pDetailSplitter.Size = new System.Drawing.Size(286, 352);
            this.pDetailSplitter.SplitterDistance = 202;
            this.pDetailSplitter.TabIndex = 0;
            // 
            // pTexPreview
            // 
            this.pTexPreview.BackColor = System.Drawing.Color.Black;
            this.pTexPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pTexPreview.Location = new System.Drawing.Point(0, 0);
            this.pTexPreview.Name = "pTexPreview";
            this.pTexPreview.Size = new System.Drawing.Size(286, 202);
            this.pTexPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pTexPreview.TabIndex = 0;
            this.pTexPreview.TabStop = false;
            this.pTexPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pTexPreview_Paint);
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
            this.pTexProps.Size = new System.Drawing.Size(286, 146);
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
            this.Size = new System.Drawing.Size(434, 352);
            this.pSplitter.Panel1.ResumeLayout(false);
            this.pSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pSplitter)).EndInit();
            this.pSplitter.ResumeLayout(false);
            this.pDetailSplitter.Panel1.ResumeLayout(false);
            this.pDetailSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pDetailSplitter)).EndInit();
            this.pDetailSplitter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pTexPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox lTextures;
        private System.Windows.Forms.SplitContainer pSplitter;
        private System.Windows.Forms.SplitContainer pDetailSplitter;
        private System.Windows.Forms.PictureBox pTexPreview;
        private System.Windows.Forms.PropertyGrid pTexProps;
    }
}
