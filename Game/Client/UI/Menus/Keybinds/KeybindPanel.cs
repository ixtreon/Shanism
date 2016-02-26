using Client.Input;
using IO;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace Client.UI.Menus.Keybinds
{
    /// <summary>
    /// Lists all standard keybinds (exluding actionbars). 
    /// </summary>
    class KeybindPanel : Control
    {
        readonly List<KeyBoxLabel> keyBoxes = new List<KeyBoxLabel>();

        public KeybindPanel()
        {
            BackColor = new Color();
        }

        public void InitKeybindLabels()
        {
            var allKeyButtons = Enum<GameAction>.Values
                .Where(a => a != GameAction.ToggleMenus)
                .Where(a => a < GameAction.ActionBar)
                .Select(a => new KeyBoxLabel(a))
                .ToList();

            var position = new Vector(Padding);

            foreach(var btn in allKeyButtons)
            {
                btn.Location = position;
                Add(btn);

                position += new Vector(0, btn.Size.Y + Padding);
                if(position.Y + btn.Size.Y + Padding > Size.Y)
                {
                    position = new Vector(position.X + btn.Size.X + Padding * 2, Padding);
                }
            }

        }

    }
}
