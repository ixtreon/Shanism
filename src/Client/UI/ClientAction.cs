
namespace Shanism.Client
{
    /// <summary>
    /// The user actions that can be performed inside the client. 
    /// Includes in-game actions such as movement and casting, 
    /// but also all UI actions such as chatting and opening menus. 
    /// </summary>
    public enum ClientAction
    {
        /* Menus */
        /// <summary>
        /// Close is always escape.
        /// </summary>
        HideMenus = 1,
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

        
        /*
         * Action Bars: only first one needs to be defined,
         * all others are arrayed after the first one.
         */

        ActionBar_0_0,
    }
}
