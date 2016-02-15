using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Input
{
    /// <summary>
    /// Contains all keys used in the game. 
    /// </summary>
    enum GameAction
    {
        ToggleMainMenu = 1,
        ToggleSpellBook,
        ToggleCharacterMenu,

        ShowHealthBars,

        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,

        Chat,

        /// <summary>
        /// Reloads the user interface. Useful for debugging. 
        /// </summary>
        ReloadUi,

        //should be last!
        ActionBar,

    }
}
