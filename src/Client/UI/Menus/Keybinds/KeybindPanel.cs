using Shanism.Client.Input;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shanism.Client.UI.Menus.Keybinds
{
    /// <summary>
    /// Lists all standard keybinds (exluding actionbars). 
    /// </summary>
    class KeybindPanel : FlowPanel
    {
        public KeybindPanel()
        {
            BackColor = new Color();

            Direction = FlowDirection.Vertical;
            var controls = Enum<ClientAction>.Values
                .OrderBy(a => a.ToString())
                .Where(a => a != ClientAction.ToggleMenus)
                .Where(a => a < ClientAction.ActionBar_0_0)
                .Select(a => new KeyBoxLabel(a));

            AddRange(controls);
        }

    }
}
