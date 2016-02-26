namespace ShanoEditor.Views.Content
{
    partial class TextureBrowser
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
            this.pControls = new System.Windows.Forms.ToolStrip();
            this.btnReload = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.texTree = new ShanoEditor.Views.Content.TextureTree();
            this.pControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // pControls
            // 
            this.pControls.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnReload,
            this.toolStripSeparator1});
            this.pControls.Location = new System.Drawing.Point(0, 0);
            this.pControls.Name = "pControls";
            this.pControls.Size = new System.Drawing.Size(241, 25);
            this.pControls.TabIndex = 3;
            this.pControls.Text = "toolStrip1";
            // 
            // btnReload
            // 
            this.btnReload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnReload.Image = global::ShanoEditor.Properties.Resources.refresh_16xLG;
            this.btnReload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(23, 22);
            this.btnReload.Text = "toolStripButton1";
            this.btnReload.ToolTipText = "Reload Textures";
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // texTree
            // 
            this.texTree.AllowDrop = true;
            this.texTree.CheckBoxes = true;
            this.texTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texTree.HideSelection = false;
            this.texTree.ImageIndex = 0;
            this.texTree.Location = new System.Drawing.Point(0, 25);
            this.texTree.Margin = new System.Windows.Forms.Padding(0);
            this.texTree.Name = "texTree";
            this.texTree.SelectedImageIndex = 0;
            this.texTree.Size = new System.Drawing.Size(241, 355);
            this.texTree.TabIndex = 4;
            // 
            // TextureBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.texTree);
            this.Controls.Add(this.pControls);
            this.Name = "TextureBrowser";
            this.Size = new System.Drawing.Size(241, 380);
            this.pControls.ResumeLayout(false);
            this.pControls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip pControls;
        private System.Windows.Forms.ToolStripButton btnReload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private TextureTree texTree;
    }
}
