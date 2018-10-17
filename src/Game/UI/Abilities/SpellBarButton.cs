using Shanism.Client.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Shanism.Client.Game;

namespace Shanism.Client.UI
{
    /// <summary>
    /// A selectable, keybindable spell button as placed in a bar. 
    /// </summary>
    class SpellBarButton : SpellButton
    {
        /// <summary>
        /// Gets the currently selected spell button. 
        /// </summary>
        public static SpellBarButton CurrentSpellButton { get; private set; }

        static SpellBarButton prevSpellButton;

        public static void DeselectCurrent()
        {
            if (prevSpellButton != null)
                prevSpellButton.IsSelected = true;
            else
                CurrentSpellButton.IsSelected = false;
        }
        

        public int BarId { get; }
        public int ButtonId { get; }

        public Keybind CurrentKeybind { get; private set; }

        string keybindString;

        public override bool IsSelected
        {
            get => base.IsSelected; 
            set
            {
                if(value == base.IsSelected)
                    return;

                base.IsSelected = value;

                if(value)
                {
                    if(CurrentSpellButton != null)
                        CurrentSpellButton.IsSelected = false;

                    prevSpellButton = CurrentSpellButton;
                    CurrentSpellButton = this;
                }
                else if(CurrentSpellButton == this)
                    CurrentSpellButton = null;
            }
        }


        public SpellBarButton(int barId, int buttonId)
        {
            BarId = barId;
            ButtonId = buttonId;

            StickyToggle = true;
        }

        protected override void OnDropEnd(DragDropArgs e)
        {
            base.OnDropEnd(e);

            //see if dropping from another spell button
            var srcButton = e.Source as SpellButton;
            if(srcButton?.Ability != null)
            {
                var oldAb = Ability;
                Ability = srcButton.Ability;

                //swap/remove if dragging from a bar button
                if(srcButton is SpellBarButton)
                    srcButton.Ability = oldAb;
            }
        }



        public override void Update(int msElapsed)
        {
            var kb = Settings.Current.Keybinds[BarId, ButtonId];
            if(kb != CurrentKeybind)
            {
                CurrentKeybind = kb;
                keybindString = CurrentKeybind.ToShortString();
            }
            base.Update(msElapsed);
        }

        public override void Draw(Canvas c)
        {
            base.Draw(c);

            c.DrawString(TextFont, keybindString, Vector2.Zero, UiColors.Text);
        }
    }
}
