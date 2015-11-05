using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbilityIDE.ScenarioViews.Models
{
    partial class AnimationView : UserControl
    {
        private Label label3;
        private Label label1;
        private Label label7;
        private Label label6;
        private Label label5;
        private NumericUpDown nSpanH;
        private NumericUpDown nSpanW;
        private Label label4;
        private NumericUpDown nSpanY;
        private NumericUpDown nSpanX;
        private ComboBox btnTextures;
        private NumericUpDown nPeriod;
        private Label label8;
        private SplitContainer pMainSplit;

        private void InitializeComponent()
        {
            this.pMainSplit = new System.Windows.Forms.SplitContainer();
            this.nPeriod = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nSpanH = new System.Windows.Forms.NumericUpDown();
            this.nSpanW = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nSpanY = new System.Windows.Forms.NumericUpDown();
            this.nSpanX = new System.Windows.Forms.NumericUpDown();
            this.btnTextures = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.texView = new AbilityIDE.ScenarioViews.Models.TextureView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.pMainSplit)).BeginInit();
            this.pMainSplit.Panel1.SuspendLayout();
            this.pMainSplit.Panel2.SuspendLayout();
            this.pMainSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpanH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpanW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpanY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpanX)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
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
            this.pMainSplit.Panel1.Controls.Add(this.nPeriod);
            this.pMainSplit.Panel1.Controls.Add(this.label7);
            this.pMainSplit.Panel1.Controls.Add(this.label6);
            this.pMainSplit.Panel1.Controls.Add(this.label5);
            this.pMainSplit.Panel1.Controls.Add(this.nSpanH);
            this.pMainSplit.Panel1.Controls.Add(this.nSpanW);
            this.pMainSplit.Panel1.Controls.Add(this.label4);
            this.pMainSplit.Panel1.Controls.Add(this.nSpanY);
            this.pMainSplit.Panel1.Controls.Add(this.nSpanX);
            this.pMainSplit.Panel1.Controls.Add(this.btnTextures);
            this.pMainSplit.Panel1.Controls.Add(this.label8);
            this.pMainSplit.Panel1.Controls.Add(this.label3);
            this.pMainSplit.Panel1.Controls.Add(this.label1);
            this.pMainSplit.Panel1MinSize = 180;
            // 
            // pMainSplit.Panel2
            // 
            this.pMainSplit.Panel2.Controls.Add(this.tabControl1);
            this.pMainSplit.Size = new System.Drawing.Size(608, 395);
            this.pMainSplit.SplitterDistance = 202;
            this.pMainSplit.TabIndex = 2;
            // 
            // nPeriod
            // 
            this.nPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nPeriod.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nPeriod.Location = new System.Drawing.Point(116, 78);
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
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 181);
            this.label7.Margin = new System.Windows.Forms.Padding(24, 6, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Height:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 158);
            this.label6.Margin = new System.Windows.Forms.Padding(24, 6, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Width:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 135);
            this.label5.Margin = new System.Windows.Forms.Padding(24, 6, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Start Y:";
            // 
            // nSpanH
            // 
            this.nSpanH.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nSpanH.Location = new System.Drawing.Point(116, 179);
            this.nSpanH.Name = "nSpanH";
            this.nSpanH.Size = new System.Drawing.Size(71, 20);
            this.nSpanH.TabIndex = 5;
            this.nSpanH.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nSpanH.ValueChanged += new System.EventHandler(this.nSpan_ValueChanged);
            // 
            // nSpanW
            // 
            this.nSpanW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nSpanW.Location = new System.Drawing.Point(116, 156);
            this.nSpanW.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.nSpanW.Name = "nSpanW";
            this.nSpanW.Size = new System.Drawing.Size(71, 20);
            this.nSpanW.TabIndex = 5;
            this.nSpanW.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nSpanW.ValueChanged += new System.EventHandler(this.nSpan_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 112);
            this.label4.Margin = new System.Windows.Forms.Padding(24, 6, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Start X:";
            // 
            // nSpanY
            // 
            this.nSpanY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nSpanY.Location = new System.Drawing.Point(116, 133);
            this.nSpanY.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.nSpanY.Name = "nSpanY";
            this.nSpanY.Size = new System.Drawing.Size(71, 20);
            this.nSpanY.TabIndex = 5;
            this.nSpanY.ValueChanged += new System.EventHandler(this.nSpan_ValueChanged);
            // 
            // nSpanX
            // 
            this.nSpanX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nSpanX.Location = new System.Drawing.Point(116, 110);
            this.nSpanX.Margin = new System.Windows.Forms.Padding(3, 9, 3, 0);
            this.nSpanX.Name = "nSpanX";
            this.nSpanX.Size = new System.Drawing.Size(71, 20);
            this.nSpanX.TabIndex = 5;
            this.nSpanX.ValueChanged += new System.EventHandler(this.nSpan_ValueChanged);
            // 
            // btnTextures
            // 
            this.btnTextures.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTextures.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.btnTextures.FormattingEnabled = true;
            this.btnTextures.Location = new System.Drawing.Point(64, 45);
            this.btnTextures.Name = "btnTextures";
            this.btnTextures.Size = new System.Drawing.Size(123, 21);
            this.btnTextures.TabIndex = 4;
            this.btnTextures.SelectedIndexChanged += new System.EventHandler(this.btnTextures_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 80);
            this.label8.Margin = new System.Windows.Forms.Padding(12, 6, 3, 0);
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
            this.label1.Size = new System.Drawing.Size(202, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Animation Properties";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(402, 395);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.texView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(394, 369);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Texture View";
            // 
            // texView
            // 
            this.texView.CanSelectLogical = true;
            this.texView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.texView.Location = new System.Drawing.Point(3, 3);
            this.texView.Name = "texView";
            this.texView.Size = new System.Drawing.Size(388, 363);
            this.texView.TabIndex = 0;
            this.texView.SelectionChanged += new System.Action(this.texView_SelectionChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Black;
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(394, 369);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Animation View";
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
            ((System.ComponentModel.ISupportInitialize)(this.nSpanH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpanW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpanY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpanX)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextureView texView;
    }
}
