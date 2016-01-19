using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanoEditor.Views.Maps
{
    partial class TerrainList
    {
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pTerrain = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pSizes = new System.Windows.Forms.FlowLayoutPanel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // pTerrain
            // 
            this.pTerrain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pTerrain.Location = new System.Drawing.Point(12, 166);
            this.pTerrain.Margin = new System.Windows.Forms.Padding(12, 6, 12, 12);
            this.pTerrain.Name = "pTerrain";
            this.pTerrain.Size = new System.Drawing.Size(235, 239);
            this.pTerrain.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 136);
            this.label1.Margin = new System.Windows.Forms.Padding(18, 12, 6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Brush Type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(18, 12, 6, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Brush Size:";
            // 
            // pSizes
            // 
            this.pSizes.Location = new System.Drawing.Point(12, 42);
            this.pSizes.Margin = new System.Windows.Forms.Padding(12, 6, 12, 6);
            this.pSizes.MinimumSize = new System.Drawing.Size(50, 38);
            this.pSizes.Name = "pSizes";
            this.pSizes.Size = new System.Drawing.Size(163, 76);
            this.pSizes.TabIndex = 1;
            // 
            // TerrainList
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Controls.Add(this.pSizes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pTerrain);
            this.Name = "TerrainList";
            this.Size = new System.Drawing.Size(259, 417);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel pSizes;
        private System.Windows.Forms.FlowLayoutPanel pTerrain;
        private System.Windows.Forms.ToolTip toolTip;
        private System.ComponentModel.IContainer components;
    }
}
