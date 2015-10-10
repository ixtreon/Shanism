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
using ShanoRpgWin;
using ShanoRPGWin.Properties;
using ShanoRPGWin.UI;
using IO;
using Client;
using Local;

namespace ShanoRPGWin
{
    public partial class Form1 : Form
    {
        ScenarioDirForm scenarioPicker = new ScenarioDirForm();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Restore the last IP and port values. 
            txtRemoteIp.Text = Settings.Default.RemoteIp;
            txtPlayerName.Text = Settings.Default.PlayerName;

            if (Settings.Default.IsRemoteGame)
                btnRemoteGame.Checked = true;
            else
                btnLocalGame.Checked = true;


            Size = MinimumSize;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            //Start the game when the "Play" button is clicked. 
            var playerName = txtPlayerName.Text;
            var ipAddress = txtRemoteIp.Text;

            Settings.Default.RemoteIp = ipAddress;
            Settings.Default.PlayerName = playerName;
            Settings.Default.IsRemoteGame = false;

            if (btnLocalGame.Checked)
                StartLocalGame(playerName);
            else
                StartRemoteGame(playerName, ipAddress);

            Settings.Default.Save();

            Application.ExitThread();

        }

        //Starts connecting to the specified server. 
        private void StartRemoteGame(string playerName, string ipAddress)
        {
            var netClient = new Network.LClient(ipAddress, playerName);
            var client = new MainGame(playerName);
            client.SetReceptor(netClient);
            netClient.SetClient(client);

            client.Running = true;
        }

        private void StartLocalGame(string playerName)
        {
            // get the selected map seed, scenario
            int mapSeed = (int)nbMapSeed.Value;
            var scenarioPath = @"..\..\..\Scenarios\DefaultScenario";

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
            scenarioPicker.ShowDialog(this);
        }
    }
}
