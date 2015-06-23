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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Restore the last IP and port values. 
            restoreSettings();
            Size = MinimumSize;
//#if DEBUG
//            heroList.SelectFirstHero();
//            forceStartGame();
//#endif
        }

        private void restoreSettings()
        {
            txtRemoteIp.Text = Settings.Default.RemoteIp;
            txtPlayerName.Text = Settings.Default.PlayerName;
            if (Settings.Default.IsRemoteGame)
                btnRemoteGame.Checked = true;
            else
                btnLocalGame.Checked = true;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            //Start the game when the "Play" button is clicked. 
            var playerName = txtPlayerName.Text;
            if (btnLocalGame.Checked)
            {
                Settings.Default.PlayerName = playerName;
                Settings.Default.IsRemoteGame = false;

                StartLocalGame(playerName);
            }
            else
            {
                var ipAddress = txtRemoteIp.Text;

                Settings.Default.RemoteIp = ipAddress;
                Settings.Default.PlayerName = playerName;
                Settings.Default.IsRemoteGame = true;

                StartRemoteGame(playerName, ipAddress);
            }
            Settings.Default.Save();

            Application.ExitThread();

        }

        //Starts connecting to the specified server. 
        private void StartRemoteGame(string playerName, string ipAddress)
        {
            //create the NetworkServer object
            var serv = new Network.LClient(ipAddress, playerName);

            var client = new MainGame(playerName, serv.Update);

            client.Server = serv;

            //start the client
            client.Running = true;

        }

        private void StartLocalGame(string playerName)
        {

            // get the selected map seed
            int mapSeed = (int)nbMapSeed.Value;

            // start the local game
            new Thread(() =>
            {
                var theGame = new LocalShano(playerName, mapSeed);

                //if (chkLocalNetworked.Checked)
                //    theGame.OpenToNetwork((int)nbPort.Value);
            }).Start();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void forceStartGame()
        {
            btnPlay_Click(null, null);
        }

        private void btnLocalGame_CheckedChanged(object sender, EventArgs e)
        {
            pLocalGame.Visible = btnLocalGame.Checked;
        }

        private void btnRemoteGame_CheckedChanged(object sender, EventArgs e)
        {
            pRemoteGame.Visible = btnRemoteGame.Checked;
        }

        private void resizeSettings()
        {
            var left = lblBasic.Left + pSettings.Left;
            lblBasic.Width = ClientSize.Width - 2 * left;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            btnPlay.Enabled = txtPlayerName.Text != null;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            resizeSettings();
        }
    }
}
