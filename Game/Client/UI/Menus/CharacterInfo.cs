using Client.Controls;
using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.Menus
{
    class CharacterInfo : Window
    {
        public IHero Target { get; set; }

        public CharacterInfo()
            : base(WindowAlign.Left)
        {
            this.Title = "Character";
            this.Key = Keybind.Character;
            this.Visible = false;
        }

        public override void Update(int msElapsed)
        {
            if (Target == null)
                Visible = false;

            base.Update(msElapsed);
        }
    }
}
