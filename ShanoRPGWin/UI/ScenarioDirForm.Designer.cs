namespace ShanoRPGWin.UI
{
    partial class ScenarioDirForm
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
            this.scenarioList = new ShanoRPGWin.UI.Scenarios.ScenarioList();
            this.Library = new ShanoRPGWin.UI.Scenarios.ScenarioLibrary();
            this.SuspendLayout();
            // 
            // scenarioList
            // 
            this.scenarioList.Library = this.Library;
            this.scenarioList.Location = new System.Drawing.Point(12, 12);
            this.scenarioList.Name = "scenarioList";
            this.scenarioList.Size = new System.Drawing.Size(175, 275);
            this.scenarioList.TabIndex = 0;
            // 
            // Library
            // 
            this.Library.AutoSave = true;
            // 
            // ScenarioDirForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 299);
            this.Controls.Add(this.scenarioList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ScenarioDirForm";
            this.Text = "ScenarioDirForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Scenarios.ScenarioList scenarioList;
        public Scenarios.ScenarioLibrary Library;
    }
}