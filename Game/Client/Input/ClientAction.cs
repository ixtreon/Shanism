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
    enum ClientAction
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

        ActionBar_0_0,
        //ActionBar_0_1,
        //ActionBar_0_2,
        //ActionBar_0_3,
        //ActionBar_0_4,
        //ActionBar_0_5,
        //ActionBar_0_6,
        //ActionBar_0_7,
        //ActionBar_0_8,
        //ActionBar_0_9,

        //ActionBar_1_0 = ActionBar_0_0 + AbilityGameAction.MaxButtonsPerBar,
        //ActionBar_1_1,
        //ActionBar_1_2,
        //ActionBar_1_3,
        //ActionBar_1_4,
        //ActionBar_1_5,
        //ActionBar_1_6,
        //ActionBar_1_7,
        //ActionBar_1_8,
        //ActionBar_1_9,
    }

    //static class AbilityGameAction
    //{
    //    public const int MaxButtonsPerBar = 100;

    //    public static ClientAction FromId(int barId, int keyId) => 
    //        ClientAction.ActionBar_0_0 + barId * MaxButtonsPerBar + keyId;

    //    public static bool IsBarAction(this ClientAction act) =>
    //        act >= ClientAction.ActionBar_0_0;

    //    /// <summary>
    //    /// Gets the Bar Id of a <see cref="ClientAction"/>. 
    //    /// </summary>
    //    /// <param name="act"></param>
    //    /// <returns></returns>
    //    public static int GetBarId(this ClientAction act) =>
    //        (act - ClientAction.ActionBar_0_0) / MaxButtonsPerBar;

    //    public static int GetButtonId(this ClientAction act) =>
    //        (act - ClientAction.ActionBar_0_0) % MaxButtonsPerBar;
    //}
}
