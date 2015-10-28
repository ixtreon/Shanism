namespace AbilityIDE.ScenarioViews
{
    partial class ModelsView
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
            this.modelTree = new System.Windows.Forms.TreeView();
            this.pModelsView = new System.Windows.Forms.SplitContainer();
            this.pButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pModelsView)).BeginInit();
            this.pModelsView.Panel1.SuspendLayout();
            this.pModelsView.SuspendLayout();
            this.pButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // modelTree
            // 
            this.modelTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modelTree.Location = new System.Drawing.Point(0, 26);
            this.modelTree.Margin = new System.Windows.Forms.Padding(0);
            this.modelTree.Name = "modelTree";
            this.modelTree.Size = new System.Drawing.Size(135, 295);
            this.modelTree.TabIndex = 0;
            this.modelTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.modelTree_AfterSelect);
            // 
            // pModelsView
            // 
            this.pModelsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pModelsView.Location = new System.Drawing.Point(0, 0);
            this.pModelsView.Name = "pModelsView";
            // 
            // pModelsView.Panel1
            // 
            this.pModelsView.Panel1.Controls.Add(this.pButtons);
            this.pModelsView.Panel1.Controls.Add(this.modelTree);
            this.pModelsView.Size = new System.Drawing.Size(405, 321);
            this.pModelsView.SplitterDistance = 135;
            this.pModelsView.TabIndex = 1;
            // 
            // pButtons
            // 
            this.pButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pButtons.ColumnCount = 3;
            this.pButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.pButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.pButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.pButtons.Controls.Add(this.btnAdd2, 0, 0);
            this.pButtons.Controls.Add(this.btnRemove, 0, 0);
            this.pButtons.Controls.Add(this.btnAdd, 0, 0);
            this.pButtons.Location = new System.Drawing.Point(0, 0);
            this.pButtons.Margin = new System.Windows.Forms.Padding(0);
            this.pButtons.Name = "pButtons";
            this.pButtons.RowCount = 1;
            this.pButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pButtons.Size = new System.Drawing.Size(135, 26);
            this.pButtons.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.Location = new System.Drawing.Point(0, 0);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(44, 26);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemove.Location = new System.Drawing.Point(44, 0);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(0);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(44, 26);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "Rem";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnAdd2
            // 
            this.btnAdd2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd2.Location = new System.Drawing.Point(88, 0);
            this.btnAdd2.Margin = new System.Windows.Forms.Padding(0);
            this.btnAdd2.Name = "btnAdd2";
            this.btnAdd2.Size = new System.Drawing.Size(47, 26);
            this.btnAdd2.TabIndex = 3;
            this.btnAdd2.Text = "Add2";
            this.btnAdd2.UseVisualStyleBackColor = true;
            // 
            // ModelsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pModelsView);
            this.Name = "ModelsView";
            this.Size = new System.Drawing.Size(405, 321);
            this.pModelsView.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pModelsView)).EndInit();
            this.pModelsView.ResumeLayout(false);
            this.pButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView modelTree;
        private System.Windows.Forms.SplitContainer pModelsView;
        private System.Windows.Forms.TableLayoutPanel pButtons;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnAdd2;
        private System.Windows.Forms.Button btnRemove;
    }
}
