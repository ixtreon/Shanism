using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Objects;
using ShanoRpgWin;
using Engine.Objects.Game;
using IO.Common;

namespace ShanoRPGWin.UI
{
    public partial class HeroListPanel : UserControl
    {
        /// <summary>
        /// Gets the selected hero. 
        /// </summary>
        public HeroSelectPanel SelectedHero { get; private set; }

        public event Action SelectedHeroChanged;
        public event Action ForceStartGame;

        public HeroListPanel()
        {
            InitializeComponent();

            ReloadHeroes();
        }

        public void ReloadHeroes()
        {
            if (!Program.DesignMode)
            {
                pHeroes.Controls.Clear();
                LocalHeroes.LoadHeroes();
                if (LocalHeroes.Heroes.Any())
                {
                    foreach (var h in LocalHeroes.Heroes)
                    {
                        AddHeroPanel(h);
                    }
                }
            }
        }

        public void SelectFirstHero()
        {
            pan_MouseClick(pHeroes.Controls[0], null);
        }

        public void AddHeroPanel(Hero h)
        {
            var pan = new HeroSelectPanel(h)
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left,
            };


            pan.MouseClick += pan_MouseClick;
            pan.MouseDoubleClick += pan_MouseDoubleClick;
            this.pHeroes.Controls.Add(pan);

            foreach (var lastRow in pHeroes.RowStyles.Cast<RowStyle>())
            {
                lastRow.Height = 64;
                lastRow.SizeType = SizeType.Absolute;
            }
        }

        void pan_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(SelectedHero != null && ForceStartGame != null)
                ForceStartGame();
        }

        void pan_MouseClick(object sender, MouseEventArgs e)
        {
            var ctrl = sender as HeroSelectPanel;

            if (SelectedHero != ctrl)
            {
                if (SelectedHero != null)
                    SelectedHero.Selected = false;

                if(ctrl != null)
                    ctrl.Selected = true;

                btnDelete.Enabled = (ctrl != null);

                SelectedHero = ctrl;
                if (SelectedHeroChanged != null)
                    SelectedHeroChanged();
            }

            base.OnMouseClick(e);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            //var name = "<enter hero name>";
            //if (InputBox.Show(ref name) == DialogResult.OK)
            //{
            //    if (LocalHeroes.Heroes.Any(h => h.Name == name))
            //    {
            //        MessageBox.Show("A hero with the same name already exists!");
            //        return;
            //    }

            //    if (name.Any(c => !char.IsLetter(c)))
            //    {
            //        MessageBox.Show("The hero name must contain only letters. ");
            //        return;
            //    }

            //    //var theHero = new Hero(Vector.Zero)
            //    //{
            //    //    Name = name,
            //    //};
            //    //theHero.Save();
            //}
            //ReloadHeroes();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Are you sure you want to delete the hero '" + SelectedHero.Hero.Name + "' ?", "Confirm Delete", MessageBoxButtons.YesNo))
                File.Delete(SelectedHero.Hero.GetDirectory());
        }
    }
}
