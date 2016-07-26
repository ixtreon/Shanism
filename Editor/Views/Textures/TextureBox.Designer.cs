using System;
using Shanism.Common.Content;

namespace Shanism.Editor.Views.Models
{
    partial class TextureBox
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
            this.texStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.singleStaticAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aDynamicAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.multipleStaticAnimationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.texStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // texStrip
            // 
            this.texStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem});
            this.texStrip.Name = "texStrip";
            this.texStrip.Size = new System.Drawing.Size(153, 48);
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.singleStaticAnimationToolStripMenuItem,
            this.aDynamicAnimationToolStripMenuItem,
            this.multipleStaticAnimationsToolStripMenuItem});
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.createToolStripMenuItem.Text = "Create...";
            // 
            // singleStaticAnimationToolStripMenuItem
            // 
            this.singleStaticAnimationToolStripMenuItem.Name = "singleStaticAnimationToolStripMenuItem";
            this.singleStaticAnimationToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.singleStaticAnimationToolStripMenuItem.Text = "A static animation";
            this.singleStaticAnimationToolStripMenuItem.Click += new System.EventHandler(this.aStaticAnimationToolStripMenuItem_Click);
            // 
            // aDynamicAnimationToolStripMenuItem
            // 
            this.aDynamicAnimationToolStripMenuItem.Name = "aDynamicAnimationToolStripMenuItem";
            this.aDynamicAnimationToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.aDynamicAnimationToolStripMenuItem.Text = "A dynamic animation";
            this.aDynamicAnimationToolStripMenuItem.Click += new System.EventHandler(this.aDynamicAnimationToolStripMenuItem_Click);
            // 
            // multipleStaticAnimationsToolStripMenuItem
            // 
            this.multipleStaticAnimationsToolStripMenuItem.Name = "multipleStaticAnimationsToolStripMenuItem";
            this.multipleStaticAnimationsToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.multipleStaticAnimationsToolStripMenuItem.Text = "Multiple static animations";
            this.multipleStaticAnimationsToolStripMenuItem.Click += new System.EventHandler(this.multipleStaticAnimationsToolStripMenuItem_Click);
            // 
            // TextureBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "TextureBox";
            this.texStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip texStrip;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem singleStaticAnimationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aDynamicAnimationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem multipleStaticAnimationsToolStripMenuItem;
    }
}
