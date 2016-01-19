using Client.Input;
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
            TitleText = "Character";
            Action = GameAction.ToggleCharacterMenu;


            Add(lblCharName);
            lblCharName.Location = new Vector(Padding, TitleHeight + Padding);

            VisibleChanged += CharacterMenu_VisibleChanged;
        }

        private void CharacterMenu_VisibleChanged(Control obj)
        {
            Visible &= (Target != null);
        }

        protected override void OnUpdate(int msElapsed)
        {
            if (Target == null)
                return;

            //lblCharName.Text = Target?.Owner.Name ?? string.Empty;
            //lblCharName.Left = (Size.X - lblCharName.Size.X) / 2;

            base.OnUpdate(msElapsed);
        }
    }
}
