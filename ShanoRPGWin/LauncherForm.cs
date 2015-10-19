using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine.Objects;
using ShanoRPGWin.Properties;
using ShanoRPGWin.UI;
using IO;
using Client;
using Local;
using ScenarioLib;

namespace ShanoRPGWin
{
    public partial class LauncherForm : Form
    {
        ScenarioDirForm scenarioPicker = new ScenarioDirForm();

        public LauncherForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Restore the last IP and port values. 
            loadSettings();

            Size = MinimumSize;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            //Start the game when the "Play" button is clicked. 
            var playerName = txtPlayerName.Text;
            var ipAddress = txtRemoteIp.Text;

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
            var netClient = new Network.LClient(ipAddress, playerName);
            var client = ShanoGame.Create(playerName);

            client.SetReceptor(netClient);
            netClient.SetClient(client);

            client.Run();
        }

        private void StartLocalGame(string playerName)
        {
            // get the selected map seed, scenario
            int mapSeed = (int)nbMapSeed.Value;
            var scenarioPath = Settings.Default.CurrentScenario;

            // start the local game
            new Thread(() =>
            {
                var theGame = new LocalShano(playerName, mapSeed, scenarioPath);

                if (chkLocalNetworked.Checked)
                    theGame.ShanoEngine.OpenToNetwork();
            }).Start();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLocalGame_CheckedChanged(object sender, EventArgs e)
        {
            pLocalGame.Visible = btnLocalGame.Checked;
        }

        private void btnRemoteGame_CheckedChanged(object sender, EventArgs e)
        {
            pRemoteGame.Visible = btnRemoteGame.Checked;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            btnPlay.Enabled = txtPlayerName.Text != null;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            resizeSettings();
        }

        private void resizeSettings()
        {
            var left = lblBasic.Left + pSettings.Left;
            lblBasic.Width = ClientSize.Width - 2 * left;
        }

        private void forceStartGame()
        {
            btnPlay_Click(null, null);
        }

        private void btnScenarioDirs_Click(object sender, EventArgs e)
        {
            var result = scenarioPicker.ShowDialog(this);
            if (result == DialogResult.OK)
                setCurrentScenario(scenarioPicker.ChosenScenario);
        }

        #region App Settings dataz
        void loadSettings()
        {
            //player
            txtPlayerName.Text = Settings.Default.PlayerName;

            //remote or local
            if (Settings.Default.IsRemoteGame)
                btnRemoteGame.Checked = true;
            else
                btnLocalGame.Checked = true;

            //chosen scenario
            var scPath = Settings.Default.CurrentScenario;
            if(!string.IsNullOrEmpty(scPath))
            {
                var sc = scenarioPicker.FindScenario(scPath);
                if(sc != null)
                    lblChosenScenario.Text = sc.Name;
            }

            //open to network
            txtRemoteIp.Text = Settings.Default.RemoteIp;

        }

        void setCurrentScenario(ScenarioBase scenario)
        {
            Settings.Default.CurrentScenario = scenario?.ScenarioDirectory;
            Settings.Default.Save();

            lblChosenScenario.Text = scenario?.Name ?? "<none>";
        }

        #endregion
    }
}
