using Shanism.Client.UI.Containers;
using Shanism.Common;
using System.Linq;


namespace Shanism.Client.UI.Menus.Keybinds
{
    /// <summary>
    /// Lists all standard keybinds (exluding actionbars). 
    /// </summary>
    class KeybindPanel : ListPanel
    {
        public KeybindPanel()
        {
            Direction = Direction.TopDown;
            var controls = Enum<ClientAction>.Values
                .OrderBy(a => a.ToString())
                .Where(a => a != ClientAction.HideMenus)
                .Where(a => a < ClientAction.ActionBar_0_0)
                .Select(a => new KeyBoxLabel(a));

            AddRange(controls);
        }

    }
}
