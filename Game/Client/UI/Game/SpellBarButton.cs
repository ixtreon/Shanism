using Client.Input;
using IO.Common;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Color = Microsoft.Xna.Framework.Color;

namespace Client.UI
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


        readonly int BarId;
        readonly int ButtonId;


        public SpellBarButton(int barId, int buttonId)
        {
            BarId = barId;
            ButtonId = buttonId;
            CanSelect = true;

            Selected += onSelected;
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

        void onSelected(Common.Button obj)
        {
            if (CurrentSpellButton != this && CurrentSpellButton != null)
                CurrentSpellButton.IsSelected = false;

            CurrentSpellButton = this;
        }

        public override void OnDraw(Graphics g)
        {
            base.OnDraw(g);

            var currentKey = ShanoSettings.Current.Keybinds.TryGet(BarId, ButtonId) ?? Keys.None;
            var str = "";
            if (currentKey != Keys.None)
                str = currentKey.ToShortString() ?? "?";
            //draw keybind

            g.DrawString(Font, str, Color.White, new Vector(), 0, 0);
        }

        protected override void OnUpdate(int msElapsed)
        {
            var currentKey = ShanoSettings.Current.Keybinds.TryGet(BarId, ButtonId);
            if (currentKey != null && KeyboardInfo.IsActivated(currentKey.Value))
                IsSelected = true;
        }
    }
}
