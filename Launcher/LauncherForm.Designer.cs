namespace ShanoRPGWin
{
    partial class LauncherForm
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
            this.components = new System.ComponentModel.Container();
            this.btnPlay = new System.Windows.Forms.Button();
            this.pSettings = new System.Windows.Forms.FlowLayoutPanel();
            this.lblBasic = new System.Windows.Forms.Label();
            this.isLocalGame = new System.Windows.Forms.RadioButton();
            this.pLocalGame = new System.Windows.Forms.Panel();
            this.lblChosenScenario = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nbMapSeed = new System.Windows.Forms.NumericUpDown();
            this.btnScenarioDirs = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chkLocalNetworked = new System.Windows.Forms.CheckBox();
            this.isRemoteGame = new System.Windows.Forms.RadioButton();
            this.pRemoteGame = new System.Windows.Forms.Panel();
            this.txtRemoteIp = new System.Windows.Forms.TextBox();
            this.lblRemoteIp = new System.Windows.Forms.Label();
            this.lblPlayerName = new System.Windows.Forms.Label();
            this.txtPlayerName = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pSettings.SuspendLayout();
            this.pLocalGame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbMapSeed)).BeginInit();
            this.pRemoteGame.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.Enabled = false;
            this.btnPlay.Location = new System.Drawing.Point(19, 53);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(6);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(350, 34);
            this.btnPlay.TabIndex = 1;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // pSettings
            // 
            this.pSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pSettings.AutoSize = true;
            this.pSettings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pSettings.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pSettings.Controls.Add(this.lblBasic);
            this.pSettings.Controls.Add(this.isLocalGame);
            this.pSettings.Controls.Add(this.pLocalGame);
            this.pSettings.Controls.Add(this.isRemoteGame);
            this.pSettings.Controls.Add(this.pRemoteGame);
            this.pSettings.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pSettings.Location = new System.Drawing.Point(15, 96);
            this.pSettings.Name = "pSettings";
            this.pSettings.Size = new System.Drawing.Size(358, 240);
            this.pSettings.TabIndex = 2;
            // 
            // lblBasic
            // 
            this.lblBasic.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblBasic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBasic.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblBasic.Location = new System.Drawing.Point(3, 3);
            this.lblBasic.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.lblBasic.Name = "lblBasic";
            this.lblBasic.Size = new System.Drawing.Size(350, 23);
            this.lblBasic.TabIndex = 8;
            this.lblBasic.Text = "Game Settings";
            this.lblBasic.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // isLocalGame
            // 
            this.isLocalGame.AutoSize = true;
            this.isLocalGame.Checked = true;
            this.isLocalGame.Location = new System.Drawing.Point(12, 32);
            this.isLocalGame.Margin = new System.Windows.Forms.Padding(12, 6, 6, 6);
            this.isLocalGame.Name = "isLocalGame";
            this.isLocalGame.Size = new System.Drawing.Size(82, 17);
            this.isLocalGame.TabIndex = 2;
            this.isLocalGame.TabStop = true;
            this.isLocalGame.Text = "Local Game";
            this.isLocalGame.UseVisualStyleBackColor = true;
            this.isLocalGame.CheckedChanged += new System.EventHandler(this.isLocalGame_CheckedChanged);
            // 
            // pLocalGame
            // 
            this.pLocalGame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pLocalGame.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pLocalGame.BackColor = System.Drawing.SystemColors.Control;
            this.pLocalGame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pLocalGame.Controls.Add(this.lblChosenScenario);
            this.pLocalGame.Controls.Add(this.label2);
            this.pLocalGame.Controls.Add(this.nbMapSeed);
            this.pLocalGame.Controls.Add(this.btnScenarioDirs);
            this.pLocalGame.Controls.Add(this.label1);
            this.pLocalGame.Controls.Add(this.chkLocalNetworked);
            this.pLocalGame.Location = new System.Drawing.Point(0, 55);
            this.pLocalGame.Margin = new System.Windows.Forms.Padding(0);
            this.pLocalGame.Name = "pLocalGame";
            this.pLocalGame.Size = new System.Drawing.Size(356, 77);
            this.pLocalGame.TabIndex = 3;
            // 
            // lblChosenScenario
            // 
            this.lblChosenScenario.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChosenScenario.Location = new System.Drawing.Point(85, 12);
            this.lblChosenScenario.Name = "lblChosenScenario";
            this.lblChosenScenario.Size = new System.Drawing.Size(210, 21);
            this.lblChosenScenario.TabIndex = 12;
            this.lblChosenScenario.Text = "<none>";
            this.lblChosenScenario.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Scenario:";
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
            this.nbMapSeed.Size = new System.Drawing.Size(96, 20);
            this.nbMapSeed.TabIndex = 4;
            this.nbMapSeed.Value = new decimal(new int[] {
            123,
            0,
            0,
            0});
            // 
            // btnScenarioDirs
            // 
            this.btnScenarioDirs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScenarioDirs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScenarioDirs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScenarioDirs.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnScenarioDirs.Location = new System.Drawing.Point(298, 12);
            this.btnScenarioDirs.Margin = new System.Windows.Forms.Padding(0);
            this.btnScenarioDirs.Name = "btnScenarioDirs";
            this.btnScenarioDirs.Size = new System.Drawing.Size(47, 21);
            this.btnScenarioDirs.TabIndex = 3;
            this.btnScenarioDirs.Text = "•••";
            this.btnScenarioDirs.UseVisualStyleBackColor = true;
            this.btnScenarioDirs.Click += new System.EventHandler(this.btnScenarioDirs_Click);
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
            // chkLocalNetworked
            // 
            this.chkLocalNetworked.AutoSize = true;
            this.chkLocalNetworked.Location = new System.Drawing.Point(234, 46);
            this.chkLocalNetworked.Margin = new System.Windows.Forms.Padding(30, 12, 3, 12);
            this.chkLocalNetworked.Name = "chkLocalNetworked";
            this.chkLocalNetworked.Size = new System.Drawing.Size(111, 17);
            this.chkLocalNetworked.TabIndex = 5;
            this.chkLocalNetworked.Text = "Open to network. ";
            this.toolTip1.SetToolTip(this.chkLocalNetworked, "Make the server available to play online. ");
            this.chkLocalNetworked.UseVisualStyleBackColor = true;
            // 
            // isRemoteGame
            // 
            this.isRemoteGame.AutoSize = true;
            this.isRemoteGame.Location = new System.Drawing.Point(12, 138);
            this.isRemoteGame.Margin = new System.Windows.Forms.Padding(12, 6, 6, 6);
            this.isRemoteGame.Name = "isRemoteGame";
            this.isRemoteGame.Size = new System.Drawing.Size(78, 17);
            this.isRemoteGame.TabIndex = 6;
            this.isRemoteGame.Text = "Online Play";
            this.isRemoteGame.UseVisualStyleBackColor = true;
            this.isRemoteGame.CheckedChanged += new System.EventHandler(this.isRemoteGame_CheckedChanged);
            // 
            // pRemoteGame
            // 
            this.pRemoteGame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pRemoteGame.BackColor = System.Drawing.SystemColors.Control;
            this.pRemoteGame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pRemoteGame.Controls.Add(this.txtRemoteIp);
            this.pRemoteGame.Controls.Add(this.lblRemoteIp);
            this.pRemoteGame.Location = new System.Drawing.Point(0, 161);
            this.pRemoteGame.Margin = new System.Windows.Forms.Padding(0);
            this.pRemoteGame.Name = "pRemoteGame";
            this.pRemoteGame.Size = new System.Drawing.Size(356, 77);
            this.pRemoteGame.TabIndex = 7;
            this.pRemoteGame.Visible = false;
            // 
            // txtRemoteIp
            // 
            this.txtRemoteIp.Location = new System.Drawing.Point(107, 14);
            this.txtRemoteIp.Name = "txtRemoteIp";
            this.txtRemoteIp.Size = new System.Drawing.Size(96, 20);
            this.txtRemoteIp.TabIndex = 7;
            this.txtRemoteIp.TextChanged += new System.EventHandler(this.remoteIp_TextChanged);
            // 
            // lblRemoteIp
            // 
            this.lblRemoteIp.AutoSize = true;
            this.lblRemoteIp.Location = new System.Drawing.Point(25, 17);
            this.lblRemoteIp.Margin = new System.Windows.Forms.Padding(30, 3, 3, 6);
            this.lblRemoteIp.Name = "lblRemoteIp";
            this.lblRemoteIp.Size = new System.Drawing.Size(76, 13);
            this.lblRemoteIp.TabIndex = 5;
            this.lblRemoteIp.Text = "Host Address: ";
            // 
            // lblPlayerName
            // 
            this.lblPlayerName.AutoSize = true;
            this.lblPlayerName.Location = new System.Drawing.Point(18, 28);
            this.lblPlayerName.Name = "lblPlayerName";
            this.lblPlayerName.Size = new System.Drawing.Size(70, 13);
            this.lblPlayerName.TabIndex = 7;
            this.lblPlayerName.Text = "Player Name:";
            // 
            // txtPlayerName
            // 
            this.txtPlayerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPlayerName.Location = new System.Drawing.Point(95, 24);
            this.txtPlayerName.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.txtPlayerName.Name = "txtPlayerName";
            this.txtPlayerName.Size = new System.Drawing.Size(274, 21);
            this.txtPlayerName.TabIndex = 0;
            this.txtPlayerName.Text = "Pesho";
            this.txtPlayerName.TextChanged += new System.EventHandler(this.playerName_TextChanged);
            // 
            // LauncherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 364);
            this.Controls.Add(this.txtPlayerName);
            this.Controls.Add(this.lblPlayerName);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.pSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(10000, 3150);
            this.MinimumSize = new System.Drawing.Size(400, 315);
            this.Name = "LauncherForm";
            this.Text = "ShanoLauncher";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.pSettings.ResumeLayout(false);
            this.pSettings.PerformLayout();
            this.pLocalGame.ResumeLayout(false);
            this.pLocalGame.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbMapSeed)).EndInit();
            this.pRemoteGame.ResumeLayout(false);
            this.pRemoteGame.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.FlowLayoutPanel pSettings;
        private System.Windows.Forms.Label lblBasic;
        private System.Windows.Forms.RadioButton isLocalGame;
        private System.Windows.Forms.Panel pLocalGame;
        private System.Windows.Forms.NumericUpDown nbMapSeed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkLocalNetworked;
        private System.Windows.Forms.RadioButton isRemoteGame;
        private System.Windows.Forms.Panel pRemoteGame;
        private System.Windows.Forms.TextBox txtRemoteIp;
        private System.Windows.Forms.Label lblRemoteIp;
        private System.Windows.Forms.Label lblPlayerName;
        private System.Windows.Forms.TextBox txtPlayerName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnScenarioDirs;
        private System.Windows.Forms.Label lblChosenScenario;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

