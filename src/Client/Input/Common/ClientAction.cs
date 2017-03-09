using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Input
{
    /// <summary>
    /// The user actions that can be performed inside the client. 
    /// Includes in-game actions such as movement and casting, 
    /// but also all UI actions such as chatting and opening menus. 
    /// </summary>
    public enum ClientAction
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
        
        
        /*
         * Action Bars: only first one needs to be defined,
         * all others are arrayed after the first one.
         */

        ActionBar_0_0,
    }
}
