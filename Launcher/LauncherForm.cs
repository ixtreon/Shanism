using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ShanoRPGWin.UI;
using Shanism.Common;
using Shanism.Client;
using Shanism.Local;
using Shanism.ScenarioLib;
using Shanism.Network.Client;
using Shanism.Launcher.Properties;

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
            Size = MinimumSize;

            //restore user settings
            txtPlayerName.Text = Settings.Default.PlayerName;
            txtRemoteIp.Text = Settings.Default.RemoteIp;
            if (Settings.Default.IsRemoteGame)
                isRemoteGame.Checked = true;
            else
                isLocalGame.Checked = true;

            //reload scenarios
            await scenarioPicker.LoadScenarios();

            //restore last scenario
            var scPath = Settings.Default.CurrentScenario;
            if (string.IsNullOrEmpty(scPath))
                return;

            var sc = scenarioPicker.FindScenario(scPath);
            if (sc != null)
            {
                setCurrentScenario(sc);
                validatePlayButton();
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            //Start the game when the "Play" button is clicked. 
            var playerName = txtPlayerName.Text;
            var ipAddress = txtRemoteIp.Text;

            //save some settings
            Settings.Default.PlayerName = playerName;
            Settings.Default.IsRemoteGame = !isLocalGame.Checked;
            Settings.Default.RemoteIp = ipAddress;
            Settings.Default.Save();

            if (isLocalGame.Checked)
                StartLocalGame(playerName);
            else
                StartRemoteGame(playerName, ipAddress);

            Application.ExitThread();

        }

        //Starts connecting to the specified server. 
        void StartRemoteGame(string playerName, string ipAddress)
        {
            var clientGame = ClientFactory.CreateGame(playerName);
            var netClient = new NClient(ipAddress);

            clientGame.GameLoaded += () =>
            {
                IReceptor receptor;
                if (!clientGame.Engine.TryConnect(netClient, out receptor))
                    throw new Exception("Unable to connect to the local server!");
                
                netClient.StartPlaying(receptor);
            };

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
                    theGame.OpenToNetwork();
            }).Start();
        }

        void playerName_TextChanged(object sender, EventArgs e)
        {
            validatePlayButton();
        }

        void remoteIp_TextChanged(object sender, EventArgs e)
        {
            validatePlayButton();
        }

        #region Panel event handlers
        void isLocalGame_CheckedChanged(object sender, EventArgs e)
        {
            pLocalGame.Visible = isLocalGame.Checked;

            isLocalGame.TabStop = !isLocalGame.Checked;
            isRemoteGame.TabStop = !isRemoteGame.Checked;
            validatePlayButton();
        }

        void isRemoteGame_CheckedChanged(object sender, EventArgs e)
        {
            pRemoteGame.Visible = isRemoteGame.Checked;

            isLocalGame.TabStop = !isLocalGame.Checked;
            isRemoteGame.TabStop = !isRemoteGame.Checked;
            validatePlayButton();
        }
        #endregion

        void Form1_Resize(object sender, EventArgs e)
        {
            //resize the gamesettings panel
            var left = lblBasic.Left + pSettings.Left;
            lblBasic.Width = ClientSize.Width - 2 * left;
        }
        

        void btnScenarioDirs_Click(object sender, EventArgs e)
        {
            var result = scenarioPicker.ShowDialog(this);
            if (result == DialogResult.OK)
                setCurrentScenario(scenarioPicker.ChosenScenario);
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
            var hasDataToPlay = isLocalGame.Checked ? hasLocalScenario : hasRemoteServer;

            var isValid = hasName && hasDataToPlay;

            btnPlay.Enabled = isValid;
        }
    }
}
