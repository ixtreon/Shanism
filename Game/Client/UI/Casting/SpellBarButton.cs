using Shanism.Client.Input;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Shanism.Common;

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

        public static SpellBarButton PreviousSpellButton { get; private set; }


        public readonly int BarId;
        public readonly int ButtonId;

        public Keybind CurrentKeybind { get; private set; }

        public override bool IsSelected
        {
            get { return base.IsSelected; }
            set
            {
                if (value == base.IsSelected)
                    return;

                base.IsSelected = value;

                if (value)
                {
                    PreviousSpellButton = CurrentSpellButton;

                    if (CurrentSpellButton != null)
                        CurrentSpellButton.IsSelected = false;

                    CurrentSpellButton = this;
                }
                else if (CurrentSpellButton == this)
                    CurrentSpellButton = null;
            }
        }


        public SpellBarButton(int barId, int buttonId)
        {
            BarId = barId;
            ButtonId = buttonId;
            CanSelect = true;

            Selected += onSelected;
            //Deselected += onDeselected;
            OnDrop += onDrop;
        }

        void onDrop(Control src)
        {
            //see if dropping from another spell button
            var srcButton = src as SpellButton;
            if (srcButton?.Ability != null)
            {
                var oldAb = Ability;
                Ability = srcButton.Ability;

                //swap/remove if dragging from a bar button
                if (srcButton is SpellBarButton)
                    srcButton.Ability = oldAb;
            }
        }

        void onSelected(Button obj)
        {
            //if (CurrentSpellButton != this && CurrentSpellButton != null)
            //    CurrentSpellButton.IsSelected = false;

            //PreviousSpellButton = CurrentSpellButton;
            //CurrentSpellButton = this;

        }

        public override void OnDraw(Graphics g)
        {
            base.OnDraw(g);

            var str = "";
            if (CurrentKeybind.Key != Keys.None)
                str = CurrentKeybind.ToShortString() ?? "?";

            //draw keybind
            g.DrawString(Font, str, Color.White, new Vector(), 0, 0);
        }

        protected override void OnUpdate(int msElapsed)
        {
            CurrentKeybind = Settings.Current.Keybinds[BarId, ButtonId];
            base.OnUpdate(msElapsed);
        }
    }
}
