namespace ShanoRPGWin
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnBack = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.pAdvanced = new System.Windows.Forms.FlowLayoutPanel();
            this.lblAdvanced = new System.Windows.Forms.Label();
            this.btnLocalGame = new System.Windows.Forms.RadioButton();
            this.pLocalGame = new System.Windows.Forms.Panel();
            this.nbMapSeed = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cbLocalNetworked = new System.Windows.Forms.CheckBox();
            this.btnRemoteGame = new System.Windows.Forms.RadioButton();
            this.pRemoteGame = new System.Windows.Forms.Panel();
            this.nbPort = new System.Windows.Forms.NumericUpDown();
            this.txtRemoteIp = new System.Windows.Forms.TextBox();
            this.lblRemoteIp = new System.Windows.Forms.Label();
            this.txtRemotePort = new System.Windows.Forms.Label();
            this.heroList = new ShanoRPGWin.UI.HeroListPanel();
            this.pAdvanced.SuspendLayout();
            this.pLocalGame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbMapSeed)).BeginInit();
            this.pRemoteGame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbPort)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.Location = new System.Drawing.Point(492, 511);
            this.btnBack.Margin = new System.Windows.Forms.Padding(6);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(272, 34);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "Exit";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.Enabled = false;
            this.btnPlay.Location = new System.Drawing.Point(492, 15);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(6);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(272, 34);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // pAdvanced
            // 
            this.pAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pAdvanced.AutoSize = true;
            this.pAdvanced.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pAdvanced.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pAdvanced.Controls.Add(this.lblAdvanced);
            this.pAdvanced.Controls.Add(this.btnLocalGame);
            this.pAdvanced.Controls.Add(this.pLocalGame);
            this.pAdvanced.Controls.Add(this.btnRemoteGame);
            this.pAdvanced.Controls.Add(this.pRemoteGame);
            this.pAdvanced.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pAdvanced.Location = new System.Drawing.Point(492, 58);
            this.pAdvanced.Name = "pAdvanced";
            this.pAdvanced.Size = new System.Drawing.Size(272, 246);
            this.pAdvanced.TabIndex = 5;
            // 
            // lblAdvanced
            // 
            this.lblAdvanced.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblAdvanced.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblAdvanced.Location = new System.Drawing.Point(3, 3);
            this.lblAdvanced.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.lblAdvanced.Name = "lblAdvanced";
            this.lblAdvanced.Size = new System.Drawing.Size(266, 23);
            this.lblAdvanced.TabIndex = 0;
            this.lblAdvanced.Text = "Advanced";
            this.lblAdvanced.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLocalGame
            // 
            this.btnLocalGame.AutoSize = true;
            this.btnLocalGame.Checked = true;
            this.btnLocalGame.Location = new System.Drawing.Point(18, 32);
            this.btnLocalGame.Margin = new System.Windows.Forms.Padding(18, 6, 6, 6);
            this.btnLocalGame.Name = "btnLocalGame";
            this.btnLocalGame.Size = new System.Drawing.Size(82, 17);
            this.btnLocalGame.TabIndex = 0;
            this.btnLocalGame.TabStop = true;
            this.btnLocalGame.Text = "Local Game";
            this.btnLocalGame.UseVisualStyleBackColor = true;
            this.btnLocalGame.CheckedChanged += new System.EventHandler(this.btnLocalGame_CheckedChanged);
            // 
            // pLocalGame
            // 
            this.pLocalGame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pLocalGame.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pLocalGame.BackColor = System.Drawing.SystemColors.Control;
            this.pLocalGame.Controls.Add(this.nbMapSeed);
            this.pLocalGame.Controls.Add(this.label1);
            this.pLocalGame.Controls.Add(this.cbLocalNetworked);
            this.pLocalGame.Location = new System.Drawing.Point(3, 58);
            this.pLocalGame.Name = "pLocalGame";
            this.pLocalGame.Size = new System.Drawing.Size(266, 77);
            this.pLocalGame.TabIndex = 1;
            // 
            // nbMapSeed
            // 
            this.nbMapSeed.InterceptArrowKeys = false;
            this.nbMapSeed.Location = new System.Drawing.Point(95, 45);
            this.nbMapSeed.Maximum = new decimal(new int[] {
            2000000000,
            0,
            0,
            0});
            this.nbMapSeed.Name = "nbMapSeed";
            this.nbMapSeed.Size = new System.Drawing.Size(116, 20);
            this.nbMapSeed.TabIndex = 6;
            this.nbMapSeed.Value = new decimal(new int[] {
            123,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 47);
            this.label1.Margin = new System.Windows.Forms.Padding(6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Map Seed:";
            // 
            // cbLocalNetworked
            // 
            this.cbLocalNetworked.AutoSize = true;
            this.cbLocalNetworked.Location = new System.Drawing.Point(30, 12);
            this.cbLocalNetworked.Margin = new System.Windows.Forms.Padding(30, 12, 3, 12);
            this.cbLocalNetworked.Name = "cbLocalNetworked";
            this.cbLocalNetworked.Size = new System.Drawing.Size(111, 17);
            this.cbLocalNetworked.TabIndex = 2;
            this.cbLocalNetworked.Text = "Open to network. ";
            this.cbLocalNetworked.UseVisualStyleBackColor = true;
            // 
            // btnRemoteGame
            // 
            this.btnRemoteGame.AutoSize = true;
            this.btnRemoteGame.Location = new System.Drawing.Point(18, 144);
            this.btnRemoteGame.Margin = new System.Windows.Forms.Padding(18, 6, 6, 6);
            this.btnRemoteGame.Name = "btnRemoteGame";
            this.btnRemoteGame.Size = new System.Drawing.Size(78, 17);
            this.btnRemoteGame.TabIndex = 1;
            this.btnRemoteGame.Text = "Online Play";
            this.btnRemoteGame.UseVisualStyleBackColor = true;
            this.btnRemoteGame.CheckedChanged += new System.EventHandler(this.btnRemoteGame_CheckedChanged);
            // 
            // pRemoteGame
            // 
            this.pRemoteGame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pRemoteGame.BackColor = System.Drawing.SystemColors.Control;
            this.pRemoteGame.Controls.Add(this.nbPort);
            this.pRemoteGame.Controls.Add(this.txtRemoteIp);
            this.pRemoteGame.Controls.Add(this.lblRemoteIp);
            this.pRemoteGame.Controls.Add(this.txtRemotePort);
            this.pRemoteGame.Location = new System.Drawing.Point(3, 170);
            this.pRemoteGame.Name = "pRemoteGame";
            this.pRemoteGame.Size = new System.Drawing.Size(266, 73);
            this.pRemoteGame.TabIndex = 7;
            this.pRemoteGame.Visible = false;
            // 
            // nbPort
            // 
            this.nbPort.InterceptArrowKeys = false;
            this.nbPort.Location = new System.Drawing.Point(63, 40);
            this.nbPort.Maximum = new decimal(new int[] {
            65000,
            0,
            0,
            0});
            this.nbPort.Name = "nbPort";
            this.nbPort.Size = new System.Drawing.Size(96, 20);
            this.nbPort.TabIndex = 7;
            this.nbPort.Value = new decimal(new int[] {
            18881,
            0,
            0,
            0});
            // 
            // txtRemoteIp
            // 
            this.txtRemoteIp.Location = new System.Drawing.Point(63, 14);
            this.txtRemoteIp.Name = "txtRemoteIp";
            this.txtRemoteIp.Size = new System.Drawing.Size(96, 20);
            this.txtRemoteIp.TabIndex = 6;
            // 
            // lblRemoteIp
            // 
            this.lblRemoteIp.AutoSize = true;
            this.lblRemoteIp.Location = new System.Drawing.Point(25, 17);
            this.lblRemoteIp.Margin = new System.Windows.Forms.Padding(30, 3, 3, 6);
            this.lblRemoteIp.Name = "lblRemoteIp";
            this.lblRemoteIp.Size = new System.Drawing.Size(23, 13);
            this.lblRemoteIp.TabIndex = 5;
            this.lblRemoteIp.Text = "IP: ";
            // 
            // txtRemotePort
            // 
            this.txtRemotePort.AutoSize = true;
            this.txtRemotePort.Location = new System.Drawing.Point(25, 42);
            this.txtRemotePort.Margin = new System.Windows.Forms.Padding(30, 6, 3, 3);
            this.txtRemotePort.Name = "txtRemotePort";
            this.txtRemotePort.Size = new System.Drawing.Size(32, 13);
            this.txtRemotePort.TabIndex = 3;
            this.txtRemotePort.Text = "Port: ";
            // 
            // heroList
            // 
            this.heroList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.heroList.Location = new System.Drawing.Point(12, 12);
            this.heroList.Name = "heroList";
            this.heroList.Size = new System.Drawing.Size(471, 533);
            this.heroList.TabIndex = 0;
            this.heroList.SelectedHeroChanged += new System.Action(this.heroListPanel1_SelectedHeroChanged);
            this.heroList.ForceStartGame += new System.Action(this.heroList_ForceStartGame);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 560);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.pAdvanced);
            this.Controls.Add(this.heroList);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pAdvanced.ResumeLayout(false);
            this.pAdvanced.PerformLayout();
            this.pLocalGame.ResumeLayout(false);
            this.pLocalGame.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbMapSeed)).EndInit();
            this.pRemoteGame.ResumeLayout(false);
            this.pRemoteGame.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.HeroListPanel heroList;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.FlowLayoutPanel pAdvanced;
        private System.Windows.Forms.Label lblAdvanced;
        private System.Windows.Forms.Panel pLocalGame;
        private System.Windows.Forms.CheckBox cbLocalNetworked;
        private System.Windows.Forms.RadioButton btnRemoteGame;
        private System.Windows.Forms.RadioButton btnLocalGame;
        private System.Windows.Forms.Label txtRemotePort;
        private System.Windows.Forms.TextBox txtRemoteIp;
        private System.Windows.Forms.Label lblRemoteIp;
        private System.Windows.Forms.Panel pRemoteGame;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nbMapSeed;
        private System.Windows.Forms.NumericUpDown nbPort;
    }
}

