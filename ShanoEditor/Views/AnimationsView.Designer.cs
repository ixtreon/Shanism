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
            this.animTree = new ShanoEditor.Views.Content.AnimationTree();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddAnim = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
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
            this.flowLayoutPanel1.SuspendLayout();
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
            this.pModelsView.Panel1.Controls.Add(this.animTree);
            this.pModelsView.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // pModelsView.Panel2
            // 
            this.pModelsView.Panel2.Controls.Add(this.animView);
            this.pModelsView.Panel2.Controls.Add(this.lblDescription);
            this.pModelsView.Size = new System.Drawing.Size(1036, 508);
            this.pModelsView.SplitterDistance = 170;
            this.pModelsView.TabIndex = 1;
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
            this.animTree.Location = new System.Drawing.Point(0, 26);
            this.animTree.Margin = new System.Windows.Forms.Padding(0);
            this.animTree.Name = "animTree";
            this.animTree.PathSeparator = "/";
            this.animTree.SelectedImageIndex = 0;
            this.animTree.Size = new System.Drawing.Size(170, 482);
            this.animTree.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.btnAddAnim);
            this.flowLayoutPanel1.Controls.Add(this.btnRename);
            this.flowLayoutPanel1.Controls.Add(this.btnDelete);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(170, 26);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // btnAddAnim
            // 
            this.btnAddAnim.Image = global::ShanoEditor.Properties.Resources.ActionEvent;
            this.btnAddAnim.Location = new System.Drawing.Point(0, 0);
            this.btnAddAnim.Margin = new System.Windows.Forms.Padding(0);
            this.btnAddAnim.Name = "btnAddAnim";
            this.btnAddAnim.Size = new System.Drawing.Size(26, 26);
            this.btnAddAnim.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnAddAnim, "Add an animation");
            this.btnAddAnim.UseVisualStyleBackColor = true;
            this.btnAddAnim.Click += new System.EventHandler(this.btnAddAnim_Click);
            // 
            // btnRename
            // 
            this.btnRename.Image = global::ShanoEditor.Properties.Resources.ActionRename;
            this.btnRename.Location = new System.Drawing.Point(26, 0);
            this.btnRename.Margin = new System.Windows.Forms.Padding(0);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(26, 26);
            this.btnRename.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnRename, "Rename");
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::ShanoEditor.Properties.Resources.ActionCancel;
            this.btnDelete.Location = new System.Drawing.Point(52, 0);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(26, 26);
            this.btnDelete.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnDelete, "Delete");
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
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
            this.rootRightClick.Size = new System.Drawing.Size(153, 26);
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
            this.btnAnimRename.Click += new System.EventHandler(this.btnRename_Click);
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
            this.btnAnimDelete.Click += new System.EventHandler(this.btnDelete_Click);
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
            this.pModelsView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pModelsView)).EndInit();
            this.pModelsView.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
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
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnAddAnim;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblDescription;
        private Content.AnimationTree animTree;
    }
}
