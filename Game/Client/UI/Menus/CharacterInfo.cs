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

namespace Client.UI.Menus
{
    class CharacterInfo : Window
    {
        public IHero Target { get; set; }

        Label lblCharName = new Label();

        public CharacterInfo()
            : base(WindowAlign.Left)
        {
            this.Title = "Character";
            this.Key = Keybind.Character;
            this.Visible = false;


            Add(lblCharName);
            lblCharName.RelativePosition = new Vector(Anchor, TitleHeight + Anchor).ToVector2();
        }

        public override void Update(int msElapsed)
        {
            if (Target == null)
                Visible = false;

            //lblCharName.Text = Target?.Owner.Name ?? string.Empty;
            lblCharName.Left = (Size.X - lblCharName.Size.X) / 2;

            base.Update(msElapsed);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (Target == null)
                return;

            base.Draw(sb);


            var name = Target.Name;



        }
    }
}
