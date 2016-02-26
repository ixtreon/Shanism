using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Input
{
    /// <summary>
    /// The enumeration of user actions that can be performed in-game. 
    /// Includes both actions sent to the server (such as moving, casting, etc)
    /// and actions relating to the game interface. 
    /// </summary>
    enum GameAction
    {
        /* Menus */
        ToggleMenus = 1,
        ToggleAbilityMenu,
        ToggleCharacterMenu,


        /* Game Settings */

        ShowHealthBars,


        /* Hero Actions */
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,

        Chat,


        /* Debugging */

        /// <summary>
        /// Reloads the user interface. Useful for debugging. 
        /// </summary>
        ReloadUi,

        ToggleDebugInfo,
        
        
        /* Action Bars */

        ActionBar,

    }
}
