using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ShanoRPGWin.Properties;
using ShanoRPGWin.UI;
using IO;
using Client;
using Local;
using ScenarioLib;
using Network.Client;

namespace ShanoRPGWin
{
    public partial class LauncherForm : Form
    {
        ScenarioDirForm scenarioPicker = new ScenarioDirForm();

        ScenarioConfig ChosenScenario;

        public LauncherForm()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {

            //Restore the last IP and port values. 
            loadSettings();

            Size = MinimumSize;

            await scenarioPicker.LoadScenarios();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            //Start the game when the "Play" button is clicked. 
            var playerName = txtPlayerName.Text;
            var ipAddress = txtRemoteIp.Text;

            //save some settings
            Settings.Default.PlayerName = playerName;
            Settings.Default.IsRemoteGame = !btnLocalGame.Checked;
            Settings.Default.RemoteIp = ipAddress;
            Settings.Default.Save();

            if (btnLocalGame.Checked)
                StartLocalGame(playerName);
            else
                StartRemoteGame(playerName, ipAddress);

            Application.ExitThread();

        }

        //Starts connecting to the specified server. 
        private void StartRemoteGame(string playerName, string ipAddress)
        {
            var netClient = new NClient(ipAddress, playerName);
            var clientGame = ShanoGame.CreateClient(playerName);

            clientGame.SetServer(netClient);
            netClient.SetClient(clientGame.Engine);

            clientGame.Run();
        }

        void StartLocalGame(string playerName)
        {
            // get the selected map seed, scenario
            var mapSeed = (int)nbMapSeed.Value;
            var scenarioPath = Settings.Default.CurrentScenario;

            // start the local game
            new Thread(() =>
            {
                var theGame = new LocalShano(playerName, mapSeed, scenarioPath);

                if (chkLocalNetworked.Checked)
                    theGame.ShanoEngine.OpenToNetwork();
            }).Start();
        }

        private void txtPlayerName_TextChanged(object sender, EventArgs e)
        {
            validatePlayButton();
        }

        private void txtRemoteIp_TextChanged(object sender, EventArgs e)
        {
            validatePlayButton();
        }

        #region Panel event handlers
        private void btnLocalGame_CheckedChanged(object sender, EventArgs e)
        {
            pLocalGame.Visible = btnLocalGame.Checked;
            validatePlayButton();
        }

        private void btnRemoteGame_CheckedChanged(object sender, EventArgs e)
        {
            pRemoteGame.Visible = btnRemoteGame.Checked;
            validatePlayButton();
        }
        #endregion

        private void Form1_Resize(object sender, EventArgs e)
        {
            //resize the gamesettings panel
            var left = lblBasic.Left + pSettings.Left;
            lblBasic.Width = ClientSize.Width - 2 * left;
        }
        

        private void btnScenarioDirs_Click(object sender, EventArgs e)
        {
            var result = scenarioPicker.ShowDialog(this);
            if (result == DialogResult.OK)
                setCurrentScenario(scenarioPicker.ChosenScenario);
        }

        /// <summary>
        /// Loads the app settings to the form
        /// </summary>
        void loadSettings()
        {
            //player name
            txtPlayerName.Text = Settings.Default.PlayerName;

            //remote or local
            if (Settings.Default.IsRemoteGame)
                btnRemoteGame.Checked = true;
            else
                btnLocalGame.Checked = true;

            //chosen scenario
            scenarioPicker.ScenariosLoaded += () =>
            {
                var scPath = Settings.Default.CurrentScenario;
                if (string.IsNullOrEmpty(scPath))
                    return;

                var sc = scenarioPicker.FindScenario(scPath);
                if (sc == null)
                    return;

                setCurrentScenario(sc);
                btnPlay.Enabled = true;
            };

            //remote ip
            txtRemoteIp.Text = Settings.Default.RemoteIp;
        }

        void setCurrentScenario(ScenarioConfig scenario)
        {
            ChosenScenario = scenario;

            Settings.Default.CurrentScenario = scenario?.BaseDirectory;
            Settings.Default.Save();

            if(scenario != null)
            {
                lblChosenScenario.Text = scenario.Name;
                toolTip1.SetToolTip(lblChosenScenario, scenario.BaseDirectory);
            }
            else
            {
                lblChosenScenario.Text = "<none>";
                toolTip1.SetToolTip(lblChosenScenario, string.Empty);
            }

            validatePlayButton();
        }

        void validatePlayButton()
        {
            var hasName = !string.IsNullOrEmpty(txtPlayerName.Text);

            var hasLocalScenario = ChosenScenario != null;
            var hasRemoteServer = !string.IsNullOrEmpty(txtRemoteIp.Text);
            var hasDataToPlay = btnLocalGame.Checked ? hasLocalScenario : hasRemoteServer;

            var isValid = hasName && hasDataToPlay;

            btnPlay.Enabled = isValid;
        }
    }
}
