using System.ComponentModel;
using System.Windows.Forms;

namespace ShanoEditor.Views.Maps
{
    partial class PropPanel
    {
        private Label lblCategory;
        private Panel panel1;
        private RadioButton btnDoodad;
        private RadioButton btnEffect;
        private Label label1;
        private Content.AnimationTree animTree;
        private ToolTip toolTip1;
        private Label label2;
        private Panel panel2;
        private Button btnColor;
        private Label label4;
        private NumericUpDown btnSize;
        private Label label3;
        private ColorDialog colorDialog;
        private IContainer components;

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
            this.lblCategory = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDoodad = new System.Windows.Forms.RadioButton();
            this.btnEffect = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnColor = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSize = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.animTree = new ShanoEditor.Views.Content.AnimationTree();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSize)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCategory.Location = new System.Drawing.Point(12, 12);
            this.lblCategory.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(103, 18);
            this.lblCategory.TabIndex = 4;
            this.lblCategory.Text = "Object Type:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnDoodad);
            this.panel1.Controls.Add(this.btnEffect);
            this.panel1.Location = new System.Drawing.Point(3, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(298, 57);
            this.panel1.TabIndex = 5;
            // 
            // btnDoodad
            // 
            this.btnDoodad.AutoSize = true;
            this.btnDoodad.Checked = true;
            this.btnDoodad.Location = new System.Drawing.Point(18, 6);
            this.btnDoodad.Margin = new System.Windows.Forms.Padding(18, 6, 3, 3);
            this.btnDoodad.Name = "btnDoodad";
            this.btnDoodad.Size = new System.Drawing.Size(63, 17);
            this.btnDoodad.TabIndex = 6;
            this.btnDoodad.TabStop = true;
            this.btnDoodad.Text = "Doodad";
            this.toolTip1.SetToolTip(this.btnDoodad, "Doodads are just like effects, but have pathing. ");
            this.btnDoodad.UseVisualStyleBackColor = true;
            this.btnDoodad.CheckedChanged += new System.EventHandler(this.btnDoodad_CheckedChanged);
            // 
            // btnEffect
            // 
            this.btnEffect.AutoSize = true;
            this.btnEffect.Location = new System.Drawing.Point(18, 32);
            this.btnEffect.Margin = new System.Windows.Forms.Padding(18, 6, 3, 3);
            this.btnEffect.Name = "btnEffect";
            this.btnEffect.Size = new System.Drawing.Size(53, 17);
            this.btnEffect.TabIndex = 7;
            this.btnEffect.Text = "Effect";
            this.toolTip1.SetToolTip(this.btnEffect, "Effects are just like doodads, but do not have pathing. ");
            this.btnEffect.UseVisualStyleBackColor = true;
            this.btnEffect.CheckedChanged += new System.EventHandler(this.btnEffect_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 210);
            this.label1.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "Animation:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 108);
            this.label2.Margin = new System.Windows.Forms.Padding(12, 12, 3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 18);
            this.label2.TabIndex = 9;
            this.label2.Text = "Properties";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.btnColor);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.btnSize);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(3, 132);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(298, 63);
            this.panel2.TabIndex = 8;
            // 
            // btnColor
            // 
            this.btnColor.BackColor = System.Drawing.Color.White;
            this.btnColor.Location = new System.Drawing.Point(58, 32);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(58, 20);
            this.btnColor.TabIndex = 3;
            this.btnColor.UseVisualStyleBackColor = false;
            this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 36);
            this.label4.Margin = new System.Windows.Forms.Padding(18, 6, 3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Color:";
            // 
            // btnSize
            // 
            this.btnSize.DecimalPlaces = 2;
            this.btnSize.Location = new System.Drawing.Point(58, 6);
            this.btnSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.btnSize.Name = "btnSize";
            this.btnSize.Size = new System.Drawing.Size(58, 20);
            this.btnSize.TabIndex = 1;
            this.btnSize.Value = new decimal(new int[] {
            25,
            0,
            0,
            65536});
            this.btnSize.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 8);
            this.label3.Margin = new System.Windows.Forms.Padding(18, 6, 3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Size:";
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
            this.animTree.Location = new System.Drawing.Point(3, 234);
            this.animTree.Name = "animTree";
            this.animTree.PathSeparator = "/";
            this.animTree.ReadOnly = true;
            this.animTree.SelectedImageIndex = 0;
            this.animTree.Size = new System.Drawing.Size(298, 363);
            this.animTree.TabIndex = 8;
            // 
            // PropPanel
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.animTree);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblCategory);
            this.Name = "PropPanel";
            this.Size = new System.Drawing.Size(304, 600);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
