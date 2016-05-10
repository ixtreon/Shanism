namespace ShanoRPGWin.UI
{
    partial class NewHeroPanel
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
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.btnNapred = new System.Windows.Forms.Button();
            this.lblSkinColor = new System.Windows.Forms.Label();
            this.lblStaffColor = new System.Windows.Forms.Label();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.lblMainColor = new System.Windows.Forms.Label();
            this.lblSecondColor = new System.Windows.Forms.Label();
            this.secondColorPicker = new ShanoRPGWin.UI.ManyColorPicker();
            this.mainColorPicker = new ShanoRPGWin.UI.ManyColorPicker();
            this.staffPicker = new ShanoRPGWin.UI.ColorPicker();
            this.skinPicker = new ShanoRPGWin.UI.ColorPicker();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(282, 3);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(195, 20);
            this.txtName.TabIndex = 0;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(169, 6);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(76, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Vylshebno Ime";
            // 
            // btnNapred
            // 
            this.btnNapred.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNapred.Location = new System.Drawing.Point(172, 374);
            this.btnNapred.Name = "btnNapred";
            this.btnNapred.Size = new System.Drawing.Size(305, 23);
            this.btnNapred.TabIndex = 2;
            this.btnNapred.Text = "Ok";
            this.btnNapred.UseVisualStyleBackColor = true;
            // 
            // lblSkinColor
            // 
            this.lblSkinColor.AutoSize = true;
            this.lblSkinColor.Location = new System.Drawing.Point(169, 71);
            this.lblSkinColor.Name = "lblSkinColor";
            this.lblSkinColor.Size = new System.Drawing.Size(55, 13);
            this.lblSkinColor.TabIndex = 5;
            this.lblSkinColor.Text = "Skin Color";
            // 
            // lblStaffColor
            // 
            this.lblStaffColor.AutoSize = true;
            this.lblStaffColor.Location = new System.Drawing.Point(169, 95);
            this.lblStaffColor.Name = "lblStaffColor";
            this.lblStaffColor.Size = new System.Drawing.Size(56, 13);
            this.lblStaffColor.TabIndex = 7;
            this.lblStaffColor.Text = "Staff Color";
            // 
            // picBox
            // 
            this.picBox.Location = new System.Drawing.Point(3, 3);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(160, 394);
            this.picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBox.TabIndex = 3;
            this.picBox.TabStop = false;
            // 
            // lblMainColor
            // 
            this.lblMainColor.AutoSize = true;
            this.lblMainColor.Location = new System.Drawing.Point(169, 120);
            this.lblMainColor.Name = "lblMainColor";
            this.lblMainColor.Size = new System.Drawing.Size(57, 13);
            this.lblMainColor.TabIndex = 9;
            this.lblMainColor.Text = "Main Color";
            // 
            // lblSecondColor
            // 
            this.lblSecondColor.AutoSize = true;
            this.lblSecondColor.Location = new System.Drawing.Point(169, 150);
            this.lblSecondColor.Name = "lblSecondColor";
            this.lblSecondColor.Size = new System.Drawing.Size(85, 13);
            this.lblSecondColor.TabIndex = 11;
            this.lblSecondColor.Text = "Secondary Color";
            // 
            // secondColorPicker
            // 
            this.secondColorPicker.Location = new System.Drawing.Point(282, 143);
            this.secondColorPicker.Margin = new System.Windows.Forms.Padding(0);
            this.secondColorPicker.MaximumSize = new System.Drawing.Size(9999, 24);
            this.secondColorPicker.MinimumSize = new System.Drawing.Size(0, 24);
            this.secondColorPicker.Name = "secondColorPicker";
            this.secondColorPicker.Size = new System.Drawing.Size(55, 24);
            this.secondColorPicker.TabIndex = 10;
            this.secondColorPicker.ColorChanged += new System.EventHandler(this.secondColorPicker_ColorChanged);
            // 
            // mainColorPicker
            // 
            this.mainColorPicker.Location = new System.Drawing.Point(282, 113);
            this.mainColorPicker.Margin = new System.Windows.Forms.Padding(0);
            this.mainColorPicker.MaximumSize = new System.Drawing.Size(9999, 24);
            this.mainColorPicker.MinimumSize = new System.Drawing.Size(0, 24);
            this.mainColorPicker.Name = "mainColorPicker";
            this.mainColorPicker.Size = new System.Drawing.Size(55, 24);
            this.mainColorPicker.TabIndex = 8;
            this.mainColorPicker.ColorChanged += new System.EventHandler(this.mainColorPicker_ColorChanged);
            // 
            // staffPicker
            // 
            this.staffPicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.staffPicker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.staffPicker.Location = new System.Drawing.Point(282, 90);
            this.staffPicker.Name = "staffPicker";
            this.staffPicker.Size = new System.Drawing.Size(195, 20);
            this.staffPicker.TabIndex = 6;
            this.staffPicker.ColorSelected += new ShanoRPGWin.UI.ColorPicker.ColorEventHandler(this.staffPicker_ColorSelected);
            // 
            // skinPicker
            // 
            this.skinPicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skinPicker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.skinPicker.Location = new System.Drawing.Point(282, 64);
            this.skinPicker.Name = "skinPicker";
            this.skinPicker.Size = new System.Drawing.Size(195, 20);
            this.skinPicker.TabIndex = 4;
            this.skinPicker.ColorSelected += new ShanoRPGWin.UI.ColorPicker.ColorEventHandler(this.skinPicker_ColorSelected);
            // 
            // NewPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblSecondColor);
            this.Controls.Add(this.secondColorPicker);
            this.Controls.Add(this.lblMainColor);
            this.Controls.Add(this.mainColorPicker);
            this.Controls.Add(this.lblStaffColor);
            this.Controls.Add(this.staffPicker);
            this.Controls.Add(this.lblSkinColor);
            this.Controls.Add(this.skinPicker);
            this.Controls.Add(this.picBox);
            this.Controls.Add(this.btnNapred);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.MaximumSize = new System.Drawing.Size(5000, 400);
            this.MinimumSize = new System.Drawing.Size(480, 400);
            this.Name = "NewPanel";
            this.Size = new System.Drawing.Size(480, 400);
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnNapred;
        private System.Windows.Forms.PictureBox picBox;
        private ShanoRPGWin.UI.ColorPicker skinPicker;
        private System.Windows.Forms.Label lblSkinColor;
        private System.Windows.Forms.Label lblStaffColor;
        private ShanoRPGWin.UI.ColorPicker staffPicker;
        private ShanoRPGWin.UI.ManyColorPicker mainColorPicker;
        private System.Windows.Forms.Label lblMainColor;
        private System.Windows.Forms.Label lblSecondColor;
        private ShanoRPGWin.UI.ManyColorPicker secondColorPicker;
    }
}
