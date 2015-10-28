using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controls
{
    /// <summary>
    /// Contains all keys used in the game. 
    /// </summary>
    enum Keybind
    {
        [DefaultKey(Keys.Escape)]
        MainMenu = 0,

        [DefaultKey(Keys.P)]
        SpellBook,

        [DefaultKey(Keys.OemTilde, ModifierKeys.Control)]
        ShowHealthBars,

        [DefaultKey(Keys.A)]
        MoveLeft,
        [DefaultKey(Keys.D)]
        MoveRight,
        [DefaultKey(Keys.W)]
        MoveUp,
        [DefaultKey(Keys.S)]
        MoveDown,
        [DefaultKey(Keys.Enter)]
        Chat,
        [DefaultKey(Keys.C)]
        Character,
    }
}
