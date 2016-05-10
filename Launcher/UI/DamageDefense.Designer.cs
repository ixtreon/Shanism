namespace ShanoRPGWin.UI
{
    partial class DamageDefenseMoveSpeed
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
            this.lblMoveNum = new System.Windows.Forms.Label();
            this.lblMove = new System.Windows.Forms.Label();
            this.lblDmgNum = new System.Windows.Forms.Label();
            this.lblDmg = new System.Windows.Forms.Label();
            this.lblDefNum = new System.Windows.Forms.Label();
            this.lblDef = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMoveNum
            // 
            this.lblMoveNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMoveNum.AutoSize = true;
            this.lblMoveNum.Location = new System.Drawing.Point(84, 58);
            this.lblMoveNum.Margin = new System.Windows.Forms.Padding(12);
            this.lblMoveNum.Name = "lblMoveNum";
            this.lblMoveNum.Size = new System.Drawing.Size(13, 13);
            this.lblMoveNum.TabIndex = 8;
            this.lblMoveNum.Text = "3";
            // 
            // lblMove
            // 
            this.lblMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMove.AutoSize = true;
            this.lblMove.Location = new System.Drawing.Point(12, 58);
            this.lblMove.Margin = new System.Windows.Forms.Padding(12);
            this.lblMove.Name = "lblMove";
            this.lblMove.Size = new System.Drawing.Size(68, 13);
            this.lblMove.TabIndex = 7;
            this.lblMove.Text = "MoveSpeed:";
            // 
            // lblDmgNum
            // 
            this.lblDmgNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDmgNum.AutoSize = true;
            this.lblDmgNum.Location = new System.Drawing.Point(84, 35);
            this.lblDmgNum.Margin = new System.Windows.Forms.Padding(12);
            this.lblDmgNum.Name = "lblDmgNum";
            this.lblDmgNum.Size = new System.Drawing.Size(34, 13);
            this.lblDmgNum.TabIndex = 10;
            this.lblDmgNum.Text = "10-25";
            // 
            // lblDmg
            // 
            this.lblDmg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDmg.AutoSize = true;
            this.lblDmg.Location = new System.Drawing.Point(12, 12);
            this.lblDmg.Margin = new System.Windows.Forms.Padding(12);
            this.lblDmg.Name = "lblDmg";
            this.lblDmg.Size = new System.Drawing.Size(47, 13);
            this.lblDmg.TabIndex = 9;
            this.lblDmg.Text = "Damage";
            // 
            // lblDefNum
            // 
            this.lblDefNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDefNum.AutoSize = true;
            this.lblDefNum.Location = new System.Drawing.Point(84, 12);
            this.lblDefNum.Margin = new System.Windows.Forms.Padding(12);
            this.lblDefNum.Name = "lblDefNum";
            this.lblDefNum.Size = new System.Drawing.Size(19, 13);
            this.lblDefNum.TabIndex = 12;
            this.lblDefNum.Text = "10";
            // 
            // lblDef
            // 
            this.lblDef.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDef.AutoSize = true;
            this.lblDef.Location = new System.Drawing.Point(12, 35);
            this.lblDef.Margin = new System.Windows.Forms.Padding(12);
            this.lblDef.Name = "lblDef";
            this.lblDef.Size = new System.Drawing.Size(50, 13);
            this.lblDef.TabIndex = 11;
            this.lblDef.Text = "Defense:";
            // 
            // DamageDefenseMoveSpeed
            // 
            this.Controls.Add(this.lblDefNum);
            this.Controls.Add(this.lblDef);
            this.Controls.Add(this.lblDmgNum);
            this.Controls.Add(this.lblDmg);
            this.Controls.Add(this.lblMoveNum);
            this.Controls.Add(this.lblMove);
            this.MaximumSize = new System.Drawing.Size(10000, 146);
            this.MinimumSize = new System.Drawing.Size(140, 88);
            this.Name = "DamageDefenseMoveSpeed";
            this.Size = new System.Drawing.Size(140, 88);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDamage;
        private System.Windows.Forms.Label lblDefense;
        private System.Windows.Forms.Label lblDamageNum;
        private System.Windows.Forms.Label lblDefenseNum;
        private System.Windows.Forms.Label lblMoveSpeed;
        private System.Windows.Forms.Label lblMoveSpeedNum;
        private System.Windows.Forms.Label lblMoveNum;
        private System.Windows.Forms.Label lblMove;
        private System.Windows.Forms.Label lblDmgNum;
        private System.Windows.Forms.Label lblDmg;
        private System.Windows.Forms.Label lblDefNum;
        private System.Windows.Forms.Label lblDef;
    }
}
