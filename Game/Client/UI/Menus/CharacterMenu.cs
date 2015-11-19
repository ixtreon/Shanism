using Client.Controls;
using Client.UI.Common;
using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using IO.Common;
using IO.Objects;

namespace Client.UI.Menus
{
    class CharacterMenu : Window
    {
        public IHero Target { get; set; }

        Label lblCharName = new Label();

        public CharacterMenu()
             : base(AnchorMode.Left)
        {
            this.Title = "Character";
            this.Key = Keybind.Character;
            this.Visible = false;


            Add(lblCharName);
            lblCharName.Location = new Vector(Padding, TitleHeight + Padding);

            VisibleChanged += CharacterMenu_VisibleChanged;
        }

        private void CharacterMenu_VisibleChanged(Control obj)
        {
            Visible &= (Target != null);
        }

        public override void Update(int msElapsed)
        {
            if (Target == null)
                return;

            //lblCharName.Text = Target?.Owner.Name ?? string.Empty;
            //lblCharName.Left = (Size.X - lblCharName.Size.X) / 2;

            base.Update(msElapsed);
        }
    }
}
