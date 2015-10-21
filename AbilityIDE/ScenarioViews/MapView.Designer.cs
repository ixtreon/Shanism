namespace AbilityIDE.ScenarioViews
{
    partial class MapView
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
            this.chkInfinite = new System.Windows.Forms.CheckBox();
            this.shanoMap1 = new AbilityIDE.ScenarioViews.Maps.ShanoMap();
            this.lblSize = new System.Windows.Forms.Label();
            this.nWidth = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nHeight = new System.Windows.Forms.NumericUpDown();
            this.btnResizeMap = new System.Windows.Forms.Button();
            this.pFiniteSettings = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pInfiniteSettings = new System.Windows.Forms.GroupBox();
            this.chkFixedSeed = new System.Windows.Forms.CheckBox();
            this.txtFixedSeed = new System.Windows.Forms.NumericUpDown();
            this.mapSplitter = new System.Windows.Forms.SplitContainer();
            this.pMapSettings = new System.Windows.Forms.Panel();
            this.btnMinMap = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMaxMap = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nHeight)).BeginInit();
            this.pFiniteSettings.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pInfiniteSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFixedSeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapSplitter)).BeginInit();
            this.mapSplitter.Panel1.SuspendLayout();
            this.mapSplitter.Panel2.SuspendLayout();
            this.mapSplitter.SuspendLayout();
            this.pMapSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkInfinite
            // 
            this.chkInfinite.AutoSize = true;
            this.chkInfinite.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInfinite.Location = new System.Drawing.Point(12, 12);
            this.chkInfinite.Margin = new System.Windows.Forms.Padding(12);
            this.chkInfinite.Name = "chkInfinite";
            this.chkInfinite.Size = new System.Drawing.Size(64, 20);
            this.chkInfinite.TabIndex = 0;
            this.chkInfinite.Text = "Infinite";
            this.chkInfinite.UseVisualStyleBackColor = true;
            this.chkInfinite.CheckedChanged += new System.EventHandler(this.chkInfinite_CheckedChanged);
            // 
            // shanoMap1
            // 
            this.shanoMap1.Brush = IO.Common.TerrainType.Dirt;
            this.shanoMap1.BrushSize = 1;
            this.shanoMap1.Cursor = System.Windows.Forms.Cursors.Cross;
            this.shanoMap1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shanoMap1.Location = new System.Drawing.Point(0, 0);
            this.shanoMap1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.shanoMap1.Name = "shanoMap1";
            this.shanoMap1.Size = new System.Drawing.Size(355, 430);
            this.shanoMap1.TabIndex = 0;
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSize.Location = new System.Drawing.Point(15, 20);
            this.lblSize.Margin = new System.Windows.Forms.Padding(12, 3, 3, 0);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(37, 16);
            this.lblSize.TabIndex = 2;
            this.lblSize.Text = "Size:";
            // 
            // nWidth
            // 
            this.nWidth.Location = new System.Drawing.Point(58, 20);
            this.nWidth.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.nWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nWidth.Name = "nWidth";
            this.nWidth.Size = new System.Drawing.Size(50, 20);
            this.nWidth.TabIndex = 3;
            this.nWidth.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.nWidth.ValueChanged += new System.EventHandler(this.WidthHeight_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(113, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "x";
            // 
            // nHeight
            // 
            this.nHeight.Location = new System.Drawing.Point(133, 20);
            this.nHeight.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.nHeight.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.nHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nHeight.Name = "nHeight";
            this.nHeight.Size = new System.Drawing.Size(50, 20);
            this.nHeight.TabIndex = 5;
            this.nHeight.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.nHeight.ValueChanged += new System.EventHandler(this.WidthHeight_ValueChanged);
            // 
            // btnResizeMap
            // 
            this.btnResizeMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResizeMap.Location = new System.Drawing.Point(15, 46);
            this.btnResizeMap.Margin = new System.Windows.Forms.Padding(12, 3, 12, 0);
            this.btnResizeMap.Name = "btnResizeMap";
            this.btnResizeMap.Size = new System.Drawing.Size(168, 26);
            this.btnResizeMap.TabIndex = 8;
            this.btnResizeMap.Text = "Resize";
            this.btnResizeMap.UseVisualStyleBackColor = true;
            this.btnResizeMap.Visible = false;
            this.btnResizeMap.Click += new System.EventHandler(this.btnResizeMap_Click);
            // 
            // pFiniteSettings
            // 
            this.pFiniteSettings.AutoSize = true;
            this.pFiniteSettings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pFiniteSettings.Controls.Add(this.btnResizeMap);
            this.pFiniteSettings.Controls.Add(this.nWidth);
            this.pFiniteSettings.Controls.Add(this.nHeight);
            this.pFiniteSettings.Controls.Add(this.label1);
            this.pFiniteSettings.Controls.Add(this.lblSize);
            this.pFiniteSettings.Location = new System.Drawing.Point(12, 47);
            this.pFiniteSettings.Name = "pFiniteSettings";
            this.pFiniteSettings.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.pFiniteSettings.Size = new System.Drawing.Size(198, 85);
            this.pFiniteSettings.TabIndex = 10;
            this.pFiniteSettings.TabStop = false;
            this.pFiniteSettings.Text = "Finite Map Settings";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pInfiniteSettings);
            this.panel1.Controls.Add(this.chkInfinite);
            this.panel1.Controls.Add(this.pFiniteSettings);
            this.panel1.Location = new System.Drawing.Point(1, 25);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(222, 356);
            this.panel1.TabIndex = 0;
            // 
            // pInfiniteSettings
            // 
            this.pInfiniteSettings.AutoSize = true;
            this.pInfiniteSettings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pInfiniteSettings.Controls.Add(this.chkFixedSeed);
            this.pInfiniteSettings.Controls.Add(this.txtFixedSeed);
            this.pInfiniteSettings.Location = new System.Drawing.Point(12, 148);
            this.pInfiniteSettings.Name = "pInfiniteSettings";
            this.pInfiniteSettings.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.pInfiniteSettings.Size = new System.Drawing.Size(198, 56);
            this.pInfiniteSettings.TabIndex = 11;
            this.pInfiniteSettings.TabStop = false;
            this.pInfiniteSettings.Text = "Infinite Map Settings";
            // 
            // chkFixedSeed
            // 
            this.chkFixedSeed.AutoSize = true;
            this.chkFixedSeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFixedSeed.Location = new System.Drawing.Point(15, 20);
            this.chkFixedSeed.Margin = new System.Windows.Forms.Padding(12, 12, 12, 3);
            this.chkFixedSeed.Name = "chkFixedSeed";
            this.chkFixedSeed.Size = new System.Drawing.Size(96, 20);
            this.chkFixedSeed.TabIndex = 6;
            this.chkFixedSeed.Text = "Fixed Seed";
            this.chkFixedSeed.UseVisualStyleBackColor = true;
            this.chkFixedSeed.CheckedChanged += new System.EventHandler(this.chkFixedSeed_CheckedChanged);
            // 
            // txtFixedSeed
            // 
            this.txtFixedSeed.Location = new System.Drawing.Point(133, 20);
            this.txtFixedSeed.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.txtFixedSeed.Maximum = new decimal(new int[] {
            2000000000,
            0,
            0,
            0});
            this.txtFixedSeed.Name = "txtFixedSeed";
            this.txtFixedSeed.Size = new System.Drawing.Size(50, 20);
            this.txtFixedSeed.TabIndex = 5;
            // 
            // mapSplitter
            // 
            this.mapSplitter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mapSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mapSplitter.IsSplitterFixed = true;
            this.mapSplitter.Location = new System.Drawing.Point(0, 0);
            this.mapSplitter.Name = "mapSplitter";
            // 
            // mapSplitter.Panel1
            // 
            this.mapSplitter.Panel1.Controls.Add(this.pMapSettings);
            this.mapSplitter.Panel1.Controls.Add(this.panel1);
            this.mapSplitter.Panel1MinSize = 222;
            // 
            // mapSplitter.Panel2
            // 
            this.mapSplitter.Panel2.Controls.Add(this.btnMaxMap);
            this.mapSplitter.Panel2.Controls.Add(this.shanoMap1);
            this.mapSplitter.Size = new System.Drawing.Size(580, 432);
            this.mapSplitter.SplitterDistance = 222;
            this.mapSplitter.SplitterWidth = 1;
            this.mapSplitter.TabIndex = 1;
            // 
            // pMapSettings
            // 
            this.pMapSettings.BackColor = System.Drawing.SystemColors.Control;
            this.pMapSettings.Controls.Add(this.btnMinMap);
            this.pMapSettings.Controls.Add(this.label2);
            this.pMapSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pMapSettings.Location = new System.Drawing.Point(0, 0);
            this.pMapSettings.Margin = new System.Windows.Forms.Padding(0);
            this.pMapSettings.Name = "pMapSettings";
            this.pMapSettings.Size = new System.Drawing.Size(220, 25);
            this.pMapSettings.TabIndex = 11;
            // 
            // btnMinMap
            // 
            this.btnMinMap.BackColor = System.Drawing.SystemColors.Control;
            this.btnMinMap.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMinMap.FlatAppearance.BorderSize = 0;
            this.btnMinMap.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnMinMap.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDark;
            this.btnMinMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinMap.Location = new System.Drawing.Point(195, 0);
            this.btnMinMap.Name = "btnMinMap";
            this.btnMinMap.Size = new System.Drawing.Size(25, 25);
            this.btnMinMap.TabIndex = 1;
            this.btnMinMap.Text = "▂";
            this.toolTip1.SetToolTip(this.btnMinMap, "Hide Map Settings");
            this.btnMinMap.UseVisualStyleBackColor = false;
            this.btnMinMap.Click += new System.EventHandler(this.btnMinMap_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(195, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Map Settings";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnMaxMap
            // 
            this.btnMaxMap.BackColor = System.Drawing.SystemColors.Control;
            this.btnMaxMap.FlatAppearance.BorderSize = 0;
            this.btnMaxMap.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnMaxMap.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDark;
            this.btnMaxMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaxMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMaxMap.Location = new System.Drawing.Point(0, -1);
            this.btnMaxMap.Name = "btnMaxMap";
            this.btnMaxMap.Size = new System.Drawing.Size(25, 25);
            this.btnMaxMap.TabIndex = 2;
            this.btnMaxMap.Text = "▞";
            this.toolTip1.SetToolTip(this.btnMaxMap, "Restore Map Settings");
            this.btnMaxMap.UseVisualStyleBackColor = false;
            this.btnMaxMap.Visible = false;
            this.btnMaxMap.Click += new System.EventHandler(this.btnMaxMap_Click);
            // 
            // MapView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.mapSplitter);
            this.MinimumSize = new System.Drawing.Size(310, 240);
            this.Name = "MapView";
            this.Size = new System.Drawing.Size(580, 432);
            ((System.ComponentModel.ISupportInitialize)(this.nWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nHeight)).EndInit();
            this.pFiniteSettings.ResumeLayout(false);
            this.pFiniteSettings.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pInfiniteSettings.ResumeLayout(false);
            this.pInfiniteSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFixedSeed)).EndInit();
            this.mapSplitter.Panel1.ResumeLayout(false);
            this.mapSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mapSplitter)).EndInit();
            this.mapSplitter.ResumeLayout(false);
            this.pMapSettings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkInfinite;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.NumericUpDown nWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nHeight;
        private Maps.ShanoMap shanoMap1;
        private System.Windows.Forms.Button btnResizeMap;
        private System.Windows.Forms.GroupBox pFiniteSettings;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer mapSplitter;
        private System.Windows.Forms.Panel pMapSettings;
        private System.Windows.Forms.Button btnMinMap;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMaxMap;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox pInfiniteSettings;
        private System.Windows.Forms.CheckBox chkFixedSeed;
        private System.Windows.Forms.NumericUpDown txtFixedSeed;
    }
}
