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
using Local;
using ShanoRpgWin;
using ShanoRPGWin.Properties;
using ShanoRPGWin.UI;

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
            nbPort.Value = Settings.Default.LastRemotePort;
            txtRemoteIp.Text = Settings.Default.LastRemoteIp;
        }

        private void heroListPanel1_SelectedHeroChanged()
        {
            btnPlay.Enabled = heroList.SelectedHero != null;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
        }

        static Random rnd = new Random();

        private void btnPlay_Click(object sender, EventArgs e)
        {

            if (heroList.SelectedHero == null)
                return;

            var h = heroList.SelectedHero.Hero;

            int mapSeed = (int)nbMapSeed.Value;

            new Thread(() =>
            {
                LocalShano theGame = new LocalShano(mapSeed, h);
                if (cbLocalNetworked.Checked)
                    theGame.OpenToNetwork((int)nbPort.Value);
            }).Start();

            if (btnRemoteGame.Checked)
            {
                Settings.Default.LastRemotePort = (int)nbPort.Value;
                Settings.Default.LastRemoteIp = txtRemoteIp.Text;
                Settings.Default.Save();
            }

            Application.ExitThread();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void heroList_ForceStartGame()
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
    }
}
