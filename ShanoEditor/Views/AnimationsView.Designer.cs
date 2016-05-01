namespace ShanoEditor.Views
{
    partial class AnimationsView
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
            this.pModelsView = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnReload = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnStripAnimAdd = new System.Windows.Forms.ToolStripButton();
            this.btnStripAnimRenamee = new System.Windows.Forms.ToolStripButton();
            this.btnStripAnimDelete = new System.Windows.Forms.ToolStripButton();
            this.animTree = new ShanoEditor.Views.Content.AnimationTree();
            this.animView = new ShanoEditor.Views.Models.AnimationView();
            this.lblDescription = new System.Windows.Forms.Label();
            this.rootRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.animRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnAnimRename = new System.Windows.Forms.ToolStripMenuItem();
            this.setAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.walkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.castToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.standToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAnimDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pModelsView)).BeginInit();
            this.pModelsView.Panel1.SuspendLayout();
            this.pModelsView.Panel2.SuspendLayout();
            this.pModelsView.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.animRightClick.SuspendLayout();
            this.SuspendLayout();
            // 
            // pModelsView
            // 
            this.pModelsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pModelsView.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.pModelsView.Location = new System.Drawing.Point(0, 0);
            this.pModelsView.Name = "pModelsView";
            // 
            // pModelsView.Panel1
            // 
            this.pModelsView.Panel1.Controls.Add(this.toolStrip1);
            this.pModelsView.Panel1.Controls.Add(this.animTree);
            // 
            // pModelsView.Panel2
            // 
            this.pModelsView.Panel2.Controls.Add(this.animView);
            this.pModelsView.Panel2.Controls.Add(this.lblDescription);
            this.pModelsView.Size = new System.Drawing.Size(1036, 508);
            this.pModelsView.SplitterDistance = 170;
            this.pModelsView.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnReload,
            this.toolStripSeparator1,
            this.btnStripAnimAdd,
            this.btnStripAnimRenamee,
            this.btnStripAnimDelete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(170, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
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
            // btnStripAnimAdd
            // 
            this.btnStripAnimAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStripAnimAdd.Image = global::ShanoEditor.Properties.Resources.ActionEvent;
            this.btnStripAnimAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStripAnimAdd.Name = "btnStripAnimAdd";
            this.btnStripAnimAdd.Size = new System.Drawing.Size(23, 22);
            this.btnStripAnimAdd.Text = "toolStripButton1";
            this.btnStripAnimAdd.Click += new System.EventHandler(this.btnStripAnimAdd_Click);
            // 
            // btnStripAnimRenamee
            // 
            this.btnStripAnimRenamee.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStripAnimRenamee.Image = global::ShanoEditor.Properties.Resources.ActionRename;
            this.btnStripAnimRenamee.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStripAnimRenamee.Name = "btnStripAnimRenamee";
            this.btnStripAnimRenamee.Size = new System.Drawing.Size(23, 22);
            this.btnStripAnimRenamee.Text = "toolStripButton2";
            this.btnStripAnimRenamee.Click += new System.EventHandler(this.btnStripAnimRename_Click);
            // 
            // btnStripAnimDelete
            // 
            this.btnStripAnimDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStripAnimDelete.Image = global::ShanoEditor.Properties.Resources.ActionCancel;
            this.btnStripAnimDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStripAnimDelete.Name = "btnStripAnimDelete";
            this.btnStripAnimDelete.Size = new System.Drawing.Size(23, 22);
            this.btnStripAnimDelete.Text = "toolStripButton3";
            this.btnStripAnimDelete.Click += new System.EventHandler(this.btnStripAnimDelete_Click);
            // 
            // animTree
            // 
            this.animTree.AllowDrop = true;
            this.animTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.animTree.HideSelection = false;
            this.animTree.ImageIndex = 0;
            this.animTree.LabelEdit = true;
            this.animTree.Location = new System.Drawing.Point(0, 25);
            this.animTree.Margin = new System.Windows.Forms.Padding(0);
            this.animTree.Name = "animTree";
            this.animTree.PathSeparator = "/";
            this.animTree.ReadOnly = false;
            this.animTree.SelectedImageIndex = 0;
            this.animTree.Size = new System.Drawing.Size(170, 483);
            this.animTree.TabIndex = 2;
            // 
            // animView
            // 
            this.animView.Location = new System.Drawing.Point(15, 79);
            this.animView.Name = "animView";
            this.animView.Size = new System.Drawing.Size(797, 380);
            this.animView.TabIndex = 0;
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(12, 26);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(12);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(838, 52);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Use the buttons in the top left corner to create models, modify their animations " +
    "and preview them. ";
            // 
            // rootRightClick
            // 
            this.rootRightClick.Name = "rootRightClick";
            this.rootRightClick.Size = new System.Drawing.Size(61, 4);
            // 
            // animRightClick
            // 
            this.animRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAnimRename,
            this.setAsToolStripMenuItem,
            this.btnAnimDelete});
            this.animRightClick.Name = "animRightClick";
            this.animRightClick.Size = new System.Drawing.Size(135, 70);
            // 
            // btnAnimRename
            // 
            this.btnAnimRename.Image = global::ShanoEditor.Properties.Resources.ActionRename;
            this.btnAnimRename.Name = "btnAnimRename";
            this.btnAnimRename.Size = new System.Drawing.Size(134, 22);
            this.btnAnimRename.Text = "Rename";
            this.btnAnimRename.Click += new System.EventHandler(this.btnStripAnimRename_Click);
            // 
            // setAsToolStripMenuItem
            // 
            this.setAsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.walkToolStripMenuItem,
            this.attackToolStripMenuItem,
            this.castToolStripMenuItem,
            this.standToolStripMenuItem});
            this.setAsToolStripMenuItem.Image = global::ShanoEditor.Properties.Resources.ActionRename;
            this.setAsToolStripMenuItem.Name = "setAsToolStripMenuItem";
            this.setAsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.setAsToolStripMenuItem.Text = "Rename To";
            // 
            // walkToolStripMenuItem
            // 
            this.walkToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.walkToolStripMenuItem.Name = "walkToolStripMenuItem";
            this.walkToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.walkToolStripMenuItem.Text = "walk";
            // 
            // attackToolStripMenuItem
            // 
            this.attackToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.attackToolStripMenuItem.Name = "attackToolStripMenuItem";
            this.attackToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.attackToolStripMenuItem.Text = "attack";
            // 
            // castToolStripMenuItem
            // 
            this.castToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.castToolStripMenuItem.Name = "castToolStripMenuItem";
            this.castToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.castToolStripMenuItem.Text = "cast";
            // 
            // standToolStripMenuItem
            // 
            this.standToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standToolStripMenuItem.Name = "standToolStripMenuItem";
            this.standToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.standToolStripMenuItem.Text = "stand";
            // 
            // btnAnimDelete
            // 
            this.btnAnimDelete.Image = global::ShanoEditor.Properties.Resources.ActionCancel;
            this.btnAnimDelete.Name = "btnAnimDelete";
            this.btnAnimDelete.Size = new System.Drawing.Size(134, 22);
            this.btnAnimDelete.Text = "Delete";
            this.btnAnimDelete.Click += new System.EventHandler(this.btnStripAnimDelete_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 250;
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 250;
            this.toolTip1.ReshowDelay = 50;
            // 
            // AnimationsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pModelsView);
            this.Name = "AnimationsView";
            this.Size = new System.Drawing.Size(1036, 508);
            this.pModelsView.Panel1.ResumeLayout(false);
            this.pModelsView.Panel1.PerformLayout();
            this.pModelsView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pModelsView)).EndInit();
            this.pModelsView.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.animRightClick.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer pModelsView;
        private System.Windows.Forms.ContextMenuStrip rootRightClick;
        private System.Windows.Forms.ContextMenuStrip animRightClick;
        private System.Windows.Forms.ToolStripMenuItem btnAnimRename;
        private System.Windows.Forms.ToolStripMenuItem btnAnimDelete;
        private System.Windows.Forms.ToolStripMenuItem setAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem walkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem attackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem castToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem standToolStripMenuItem;
        private Models.AnimationView animView;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblDescription;
        private Content.AnimationTree animTree;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnReload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnStripAnimAdd;
        private System.Windows.Forms.ToolStripButton btnStripAnimRenamee;
        private System.Windows.Forms.ToolStripButton btnStripAnimDelete;
    }
}
