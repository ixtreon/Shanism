using System;
using System.Collections.Generic;

namespace ShanoEditor.Views
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
            this.lblSize = new System.Windows.Forms.Label();
            this.nWidth = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nHeight = new System.Windows.Forms.NumericUpDown();
            this.btnResizeMap = new System.Windows.Forms.Button();
            this.pFiniteSettings = new System.Windows.Forms.GroupBox();
            this.btnCancelMapResize = new System.Windows.Forms.Button();
            this.pMapSettings = new System.Windows.Forms.Panel();
            this.pInfiniteSettings = new System.Windows.Forms.GroupBox();
            this.chkFixedSeed = new System.Windows.Forms.CheckBox();
            this.txtFixedSeed = new System.Windows.Forms.NumericUpDown();
            this.mapSplitter = new System.Windows.Forms.SplitContainer();
            this.pTerrain = new ShanoEditor.Views.Maps.TerrainList();
            this.pObjects = new ShanoEditor.Views.Maps.GameObjectList();
            this.listPanel1 = new ShanoEditor.Views.ListPanel();
            this.btnMaxTools = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nHeight)).BeginInit();
            this.pFiniteSettings.SuspendLayout();
            this.pMapSettings.SuspendLayout();
            this.pInfiniteSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFixedSeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapSplitter)).BeginInit();
            this.mapSplitter.Panel1.SuspendLayout();
            this.mapSplitter.Panel2.SuspendLayout();
            this.mapSplitter.SuspendLayout();
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
            this.toolTip1.SetToolTip(this.chkInfinite, "Controls whether this scenario uses an \r\ninfinite, procedural map (not yet suppor" +
        "ted),\r\nor a finite, fixed map. ");
            this.chkInfinite.UseVisualStyleBackColor = true;
            this.chkInfinite.CheckedChanged += new System.EventHandler(this.chkInfinite_CheckedChanged);
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
            this.nWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nWidth.Location = new System.Drawing.Point(60, 20);
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
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(115, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "x";
            // 
            // nHeight
            // 
            this.nHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nHeight.Location = new System.Drawing.Point(135, 20);
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
            this.btnResizeMap.Location = new System.Drawing.Point(15, 47);
            this.btnResizeMap.Margin = new System.Windows.Forms.Padding(12, 3, 0, 0);
            this.btnResizeMap.Name = "btnResizeMap";
            this.btnResizeMap.Size = new System.Drawing.Size(141, 26);
            this.btnResizeMap.TabIndex = 8;
            this.btnResizeMap.Text = "Resize";
            this.toolTip1.SetToolTip(this.btnResizeMap, "Applies the changes to the  map size. ");
            this.btnResizeMap.UseVisualStyleBackColor = true;
            this.btnResizeMap.Click += new System.EventHandler(this.btnResizeMap_Click);
            // 
            // pFiniteSettings
            // 
            this.pFiniteSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pFiniteSettings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pFiniteSettings.Controls.Add(this.btnCancelMapResize);
            this.pFiniteSettings.Controls.Add(this.btnResizeMap);
            this.pFiniteSettings.Controls.Add(this.nWidth);
            this.pFiniteSettings.Controls.Add(this.nHeight);
            this.pFiniteSettings.Controls.Add(this.label1);
            this.pFiniteSettings.Controls.Add(this.lblSize);
            this.pFiniteSettings.Location = new System.Drawing.Point(12, 47);
            this.pFiniteSettings.Margin = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.pFiniteSettings.Name = "pFiniteSettings";
            this.pFiniteSettings.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.pFiniteSettings.Size = new System.Drawing.Size(200, 89);
            this.pFiniteSettings.TabIndex = 10;
            this.pFiniteSettings.TabStop = false;
            this.pFiniteSettings.Text = "Finite Map Settings";
            // 
            // btnCancelMapResize
            // 
            this.btnCancelMapResize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelMapResize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelMapResize.Location = new System.Drawing.Point(159, 47);
            this.btnCancelMapResize.Name = "btnCancelMapResize";
            this.btnCancelMapResize.Size = new System.Drawing.Size(26, 26);
            this.btnCancelMapResize.TabIndex = 9;
            this.btnCancelMapResize.Text = "✕";
            this.toolTip1.SetToolTip(this.btnCancelMapResize, "Cancels any changes to the map size. ");
            this.btnCancelMapResize.UseVisualStyleBackColor = true;
            this.btnCancelMapResize.Click += new System.EventHandler(this.btnCancelMapResize_Click);
            // 
            // pMapSettings
            // 
            this.pMapSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pMapSettings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pMapSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.pMapSettings.Controls.Add(this.chkInfinite);
            this.pMapSettings.Controls.Add(this.pInfiniteSettings);
            this.pMapSettings.Controls.Add(this.pFiniteSettings);
            this.pMapSettings.Location = new System.Drawing.Point(3, 96);
            this.pMapSettings.Name = "pMapSettings";
            this.pMapSettings.Size = new System.Drawing.Size(224, 210);
            this.pMapSettings.TabIndex = 0;
            // 
            // pInfiniteSettings
            // 
            this.pInfiniteSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pInfiniteSettings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pInfiniteSettings.Controls.Add(this.chkFixedSeed);
            this.pInfiniteSettings.Controls.Add(this.txtFixedSeed);
            this.pInfiniteSettings.Location = new System.Drawing.Point(12, 151);
            this.pInfiniteSettings.Margin = new System.Windows.Forms.Padding(12);
            this.pInfiniteSettings.Name = "pInfiniteSettings";
            this.pInfiniteSettings.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.pInfiniteSettings.Size = new System.Drawing.Size(200, 57);
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
            this.txtFixedSeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFixedSeed.Location = new System.Drawing.Point(135, 21);
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
            this.mapSplitter.Location = new System.Drawing.Point(0, 0);
            this.mapSplitter.Name = "mapSplitter";
            // 
            // mapSplitter.Panel1
            // 
            this.mapSplitter.Panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mapSplitter.Panel1.Controls.Add(this.pTerrain);
            this.mapSplitter.Panel1.Controls.Add(this.pObjects);
            this.mapSplitter.Panel1.Controls.Add(this.pMapSettings);
            this.mapSplitter.Panel1.Controls.Add(this.listPanel1);
            this.mapSplitter.Panel1MinSize = 222;
            // 
            // mapSplitter.Panel2
            // 
            this.mapSplitter.Panel2.Controls.Add(this.btnMaxTools);
            this.mapSplitter.Size = new System.Drawing.Size(580, 677);
            this.mapSplitter.SplitterDistance = 232;
            this.mapSplitter.TabIndex = 1;
            // 
            // pTerrain
            // 
            this.pTerrain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.pTerrain.Location = new System.Drawing.Point(7, 312);
            this.pTerrain.Name = "pTerrain";
            this.pTerrain.Size = new System.Drawing.Size(220, 296);
            this.pTerrain.TabIndex = 17;
            this.pTerrain.BrushTypeSelected += new System.Action<IO.Common.TerrainType>(this.pTerrain_BrushTypeSelected);
            this.pTerrain.BrushSizeSelected += new System.Action<int>(this.pTerrain_BrushSizeSelected);
            this.pTerrain.VisibleChanged += new System.EventHandler(this.pTerrain_VisibleChanged);
            // 
            // pObjects
            // 
            this.pObjects.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.pObjects.Location = new System.Drawing.Point(7, 339);
            this.pObjects.Name = "pObjects";
            this.pObjects.Size = new System.Drawing.Size(208, 314);
            this.pObjects.TabIndex = 0;
            this.pObjects.ObjectSelected += new System.Action<IO.Objects.IGameObject>(this.pObjects_ObjectSelected);
            this.pObjects.VisibleChanged += new System.EventHandler(this.pObjects_VisibleChanged);
            // 
            // listPanel1
            // 
            this.listPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.listPanel1.Location = new System.Drawing.Point(0, -1);
            this.listPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.listPanel1.Name = "listPanel1";
            this.listPanel1.Size = new System.Drawing.Size(227, 94);
            this.listPanel1.TabIndex = 16;
            // 
            // btnMaxTools
            // 
            this.btnMaxTools.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnMaxTools.FlatAppearance.BorderSize = 0;
            this.btnMaxTools.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnMaxTools.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnMaxTools.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaxTools.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMaxTools.Location = new System.Drawing.Point(1, 1);
            this.btnMaxTools.Margin = new System.Windows.Forms.Padding(1, 1, 3, 3);
            this.btnMaxTools.Name = "btnMaxTools";
            this.btnMaxTools.Size = new System.Drawing.Size(24, 24);
            this.btnMaxTools.TabIndex = 2;
            this.btnMaxTools.Text = "◀";
            this.toolTip1.SetToolTip(this.btnMaxTools, "Show/Hide side panel. ");
            this.btnMaxTools.UseVisualStyleBackColor = false;
            this.btnMaxTools.Click += new System.EventHandler(this.btnMaxTools_Click);
            // 
            // MapView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mapSplitter);
            this.MinimumSize = new System.Drawing.Size(310, 240);
            this.Name = "MapView";
            this.Size = new System.Drawing.Size(580, 677);
            ((System.ComponentModel.ISupportInitialize)(this.nWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nHeight)).EndInit();
            this.pFiniteSettings.ResumeLayout(false);
            this.pFiniteSettings.PerformLayout();
            this.pMapSettings.ResumeLayout(false);
            this.pMapSettings.PerformLayout();
            this.pInfiniteSettings.ResumeLayout(false);
            this.pInfiniteSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFixedSeed)).EndInit();
            this.mapSplitter.Panel1.ResumeLayout(false);
            this.mapSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mapSplitter)).EndInit();
            this.mapSplitter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkInfinite;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.NumericUpDown nWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nHeight;
        private System.Windows.Forms.Button btnResizeMap;
        private System.Windows.Forms.GroupBox pFiniteSettings;
        private System.Windows.Forms.Panel pMapSettings;
        private System.Windows.Forms.SplitContainer mapSplitter;
        private System.Windows.Forms.Button btnMaxTools;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox pInfiniteSettings;
        private System.Windows.Forms.CheckBox chkFixedSeed;
        private System.Windows.Forms.NumericUpDown txtFixedSeed;
        private System.Windows.Forms.Button btnCancelMapResize;
        private Maps.GameObjectList pObjects;
        private ShanoEditor.Views.ListPanel listPanel1;
        private Maps.TerrainList pTerrain;
    }
}
