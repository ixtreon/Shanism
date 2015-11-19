using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoEditor.ScenarioViews.Models
{
    partial class AnimationView : ScenarioView
    {
        private Label label3;
        private Label label1;
        private Label label5;
        private ComboBox btnTextures;
        private NumericUpDown nPeriod;
        private Label label8;
        private SplitContainer pMainSplit;

        private void InitializeComponent()
        {
            this.pMainSplit = new System.Windows.Forms.SplitContainer();
            this.txtSpan = new System.Windows.Forms.TextBox();
            this.chkLoop = new System.Windows.Forms.CheckBox();
            this.chkDynamic = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nPeriod = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.btnTextures = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textureBox = new ShanoEditor.ScenarioViews.Models.TextureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pMainSplit)).BeginInit();
            this.pMainSplit.Panel1.SuspendLayout();
            this.pMainSplit.Panel2.SuspendLayout();
            this.pMainSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pMainSplit
            // 
            this.pMainSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMainSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.pMainSplit.Location = new System.Drawing.Point(0, 0);
            this.pMainSplit.Margin = new System.Windows.Forms.Padding(0);
            this.pMainSplit.Name = "pMainSplit";
            // 
            // pMainSplit.Panel1
            // 
            this.pMainSplit.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pMainSplit.Panel1.Controls.Add(this.txtSpan);
            this.pMainSplit.Panel1.Controls.Add(this.chkLoop);
            this.pMainSplit.Panel1.Controls.Add(this.chkDynamic);
            this.pMainSplit.Panel1.Controls.Add(this.label2);
            this.pMainSplit.Panel1.Controls.Add(this.nPeriod);
            this.pMainSplit.Panel1.Controls.Add(this.label5);
            this.pMainSplit.Panel1.Controls.Add(this.btnTextures);
            this.pMainSplit.Panel1.Controls.Add(this.label8);
            this.pMainSplit.Panel1.Controls.Add(this.label3);
            this.pMainSplit.Panel1.Controls.Add(this.label1);
            this.pMainSplit.Panel1MinSize = 180;
            // 
            // pMainSplit.Panel2
            // 
            this.pMainSplit.Panel2.Controls.Add(this.splitContainer1);
            this.pMainSplit.Size = new System.Drawing.Size(608, 395);
            this.pMainSplit.SplitterDistance = 213;
            this.pMainSplit.TabIndex = 2;
            // 
            // txtSpan
            // 
            this.txtSpan.Location = new System.Drawing.Point(98, 155);
            this.txtSpan.Name = "txtSpan";
            this.txtSpan.Size = new System.Drawing.Size(100, 20);
            this.txtSpan.TabIndex = 11;
            this.txtSpan.Text = "[ 99, 99, 99, 99 ]";
            this.txtSpan.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chkLoop
            // 
            this.chkLoop.AutoSize = true;
            this.chkLoop.Location = new System.Drawing.Point(33, 132);
            this.chkLoop.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.Size = new System.Drawing.Size(64, 17);
            this.chkLoop.TabIndex = 10;
            this.chkLoop.Text = "Looping";
            this.chkLoop.UseVisualStyleBackColor = true;
            this.chkLoop.CheckedChanged += new System.EventHandler(this.chkLoop_CheckedChanged);
            // 
            // chkDynamic
            // 
            this.chkDynamic.AutoSize = true;
            this.chkDynamic.Location = new System.Drawing.Point(12, 85);
            this.chkDynamic.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.chkDynamic.Name = "chkDynamic";
            this.chkDynamic.Size = new System.Drawing.Size(67, 17);
            this.chkDynamic.TabIndex = 9;
            this.chkDynamic.Text = "Dynamic";
            this.chkDynamic.UseVisualStyleBackColor = true;
            this.chkDynamic.CheckedChanged += new System.EventHandler(this.chkDynamic_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 158);
            this.label2.Margin = new System.Windows.Forms.Padding(12, 6, 3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Span:";
            // 
            // nPeriod
            // 
            this.nPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nPeriod.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nPeriod.Location = new System.Drawing.Point(127, 109);
            this.nPeriod.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.nPeriod.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nPeriod.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nPeriod.Name = "nPeriod";
            this.nPeriod.Size = new System.Drawing.Size(71, 20);
            this.nPeriod.TabIndex = 7;
            this.nPeriod.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nPeriod.ValueChanged += new System.EventHandler(this.nPeriod_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 184);
            this.label5.Margin = new System.Windows.Forms.Padding(24, 6, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(141, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "(select tiles in the right pane)";
            // 
            // btnTextures
            // 
            this.btnTextures.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTextures.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.btnTextures.FormattingEnabled = true;
            this.btnTextures.Location = new System.Drawing.Point(64, 45);
            this.btnTextures.Name = "btnTextures";
            this.btnTextures.Size = new System.Drawing.Size(134, 21);
            this.btnTextures.TabIndex = 4;
            this.btnTextures.SelectedIndexChanged += new System.EventHandler(this.btnTextures_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(30, 111);
            this.label8.Margin = new System.Windows.Forms.Padding(30, 6, 3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Period:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 48);
            this.label3.Margin = new System.Windows.Forms.Padding(12, 6, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Texture:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Animation Properties";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textureBox);
            this.splitContainer1.Size = new System.Drawing.Size(391, 395);
            this.splitContainer1.SplitterDistance = 226;
            this.splitContainer1.TabIndex = 1;
            // 
            // textureBox
            // 
            this.textureBox.CanSelectLogical = true;
            this.textureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureBox.Location = new System.Drawing.Point(0, 0);
            this.textureBox.Name = "textureBox";
            this.textureBox.Size = new System.Drawing.Size(391, 226);
            this.textureBox.TabIndex = 0;
            this.textureBox.SelectionChanged += new System.Action(this.textureBox_SelectionChanged);
            // 
            // AnimationView
            // 
            this.Controls.Add(this.pMainSplit);
            this.Name = "AnimationView";
            this.Size = new System.Drawing.Size(608, 395);
            this.pMainSplit.Panel1.ResumeLayout(false);
            this.pMainSplit.Panel1.PerformLayout();
            this.pMainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pMainSplit)).EndInit();
            this.pMainSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nPeriod)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private TextureBox textureBox;
        private CheckBox chkLoop;
        private CheckBox chkDynamic;
        private Label label2;
        private SplitContainer splitContainer1;
        private TextBox txtSpan;
    }
}
