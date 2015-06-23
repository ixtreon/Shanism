using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine.Objects.Game;

namespace ShanoRPGWin.UI
{
    public partial class HeroSelectPanel : UserControl
    {
        public readonly Hero Hero;
        public HeroSelectPanel()
        {
            InitializeComponent();

        }
        public HeroSelectPanel(Hero h)
        {
            this.Hero = h;
            InitializeComponent();
            this.ResizeRedraw = true;
        }

        private bool selected;
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                this.selected = value;
                Invalidate();
            }
        }

        public Font NameFont = new Font(FontFamily.GenericSansSerif, 12f);
        public Font LevelFont = new Font(FontFamily.GenericSansSerif, 10f);

        private void HeroSelectPanel_Paint(object sender, PaintEventArgs e)
        {

            var imgPos = new Point(3, 3);
            var imgSize = (Size)(new Point(Height, Height) - (Size)imgPos - (Size)imgPos);

            e.Graphics.FillRectangle(Brushes.GreenYellow, new Rectangle(imgPos, imgSize));

            if (Hero != null)
            {
                var nameSize = e.Graphics.MeasureString(Hero.Name, NameFont);
                var namePos = imgPos + new Size(imgSize.Width + 6, 3);

                e.Graphics.DrawString(Hero.Name, NameFont, Brushes.Black, namePos);

                var sLvl = "Level " + Hero.Level.ToString();
                var lvlSize = e.Graphics.MeasureString(sLvl, LevelFont);
                var lvlPos = imgPos + imgSize + new Size(6, -(int)lvlSize.Height - 3);

                e.Graphics.DrawString(sLvl, LevelFont, Brushes.Black, lvlPos);

                if (selected)
                {
                    var p = new Pen(Color.DarkBlue, 5);
                    e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
                }
            }
        }
    }
}
