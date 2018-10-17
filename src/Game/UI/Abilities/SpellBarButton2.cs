using Shanism.Client.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Shanism.Client.UI
{
    /// <summary>
    /// A selectable, keybindable spell button as placed in a bar. 
    /// </summary>
    class SpellBarButton2 : SpellButton
    {

        static new Vector2 DefaultSize { get; } = new Vector2(0.1f);

        public Keybind CurrentKeybind { get; private set; }

        string keyText;

        public SpellBarButton2()
        {
            StickyToggle = true;
            Size = DefaultSize;
        }

        public void SetKeybind(Keybind kb)
        {
            if(kb == CurrentKeybind)
                return;

            CurrentKeybind = kb;
            keyText = CurrentKeybind.ToShortString();
        }

        protected override void OnDropEnd(DragDropArgs e)
        {
            base.OnDropEnd(e);
            if ((e.Source is SpellButton other) && other.Ability != null)
            {
                var temp = Ability;
                Ability = other.Ability;
                other.Ability = temp;
            }
        }



        public override void Draw(Canvas g)
        {
            base.Draw(g);
            g.DrawString(TextFont, keyText, Vector2.Zero, UiColors.Text);
        }
    }
}
