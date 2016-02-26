using System;
using System.Collections.Generic;

namespace ShanoEditor.Views.Maps
{
    partial class GameObjectList
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
            this.cbCategory = new System.Windows.Forms.ComboBox();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.cbOwner = new System.Windows.Forms.ComboBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblOwner = new System.Windows.Forms.Label();
            this.pOwner = new System.Windows.Forms.Panel();
            this.pOwner.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbCategory
            // 
            this.cbCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCategory.FormattingEnabled = true;
            this.cbCategory.Location = new System.Drawing.Point(12, 42);
            this.cbCategory.Margin = new System.Windows.Forms.Padding(12, 12, 12, 0);
            this.cbCategory.Name = "cbCategory";
            this.cbCategory.Size = new System.Drawing.Size(274, 24);
            this.cbCategory.TabIndex = 0;
            this.cbCategory.SelectedIndexChanged += new System.EventHandler(this.onCategoryChanged);
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainPanel.Location = new System.Drawing.Point(12, 164);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(12);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(274, 200);
            this.mainPanel.TabIndex = 1;
            // 
            // cbOwner
            // 
            this.cbOwner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbOwner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOwner.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbOwner.FormattingEnabled = true;
            this.cbOwner.Items.AddRange(new object[] {
            "Neutral Aggressive",
            "Neutral Passive"});
            this.cbOwner.Location = new System.Drawing.Point(9, 41);
            this.cbOwner.Margin = new System.Windows.Forms.Padding(12, 12, 12, 0);
            this.cbOwner.Name = "cbOwner";
            this.cbOwner.Size = new System.Drawing.Size(274, 24);
            this.cbOwner.TabIndex = 2;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCategory.Location = new System.Drawing.Point(9, 12);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(81, 18);
            this.lblCategory.TabIndex = 3;
            this.lblCategory.Text = "Category:";
            // 
            // lblOwner
            // 
            this.lblOwner.AutoSize = true;
            this.lblOwner.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOwner.Location = new System.Drawing.Point(6, 11);
            this.lblOwner.Name = "lblOwner";
            this.lblOwner.Size = new System.Drawing.Size(62, 18);
            this.lblOwner.TabIndex = 4;
            this.lblOwner.Text = "Owner:";
            // 
            // pOwner
            // 
            this.pOwner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pOwner.Controls.Add(this.lblOwner);
            this.pOwner.Controls.Add(this.cbOwner);
            this.pOwner.Location = new System.Drawing.Point(3, 70);
            this.pOwner.Name = "pOwner";
            this.pOwner.Size = new System.Drawing.Size(292, 79);
            this.pOwner.TabIndex = 0;
            this.pOwner.VisibleChanged += new System.EventHandler(this.pOwner_VisibleChanged);
            // 
            // GameObjectList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.pOwner);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.cbCategory);
            this.Name = "GameObjectList";
            this.Size = new System.Drawing.Size(298, 376);
            this.pOwner.ResumeLayout(false);
            this.pOwner.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbCategory;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.ComboBox cbOwner;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblOwner;
        private System.Windows.Forms.Panel pOwner;
    }
}
