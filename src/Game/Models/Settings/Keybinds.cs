using Shanism.Client.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Shanism.Client
{
    class KeybindSettings
    {
        readonly Dictionary<ClientAction, Keybind> keyMap = new Dictionary<ClientAction, Keybind>();

        [JsonIgnore]
        public IReadOnlyDictionary<ClientAction, Keybind> BoundActions => keyMap;

        [JsonConstructor]
        KeybindSettings() { }

        public KeybindSettings(bool init)
        {
            if (!init) return;

            this[ClientAction.HideMenus] = new Keybind(Keys.Escape);
            this[ClientAction.ToggleCharacterMenu] = new Keybind(Keys.C);
            this[ClientAction.ToggleAbilityMenu] = new Keybind(Keys.B);

            this[ClientAction.ShowHealthBars] = new Keybind(Keys.Tab);

            this[ClientAction.ReloadUi] = new Keybind(ModifierKeys.Control | ModifierKeys.Shift, Keys.R);

            this[ClientAction.MoveUp] = new Keybind(Keys.W);
            this[ClientAction.MoveDown] = new Keybind(Keys.S);
            this[ClientAction.MoveLeft] = new Keybind(Keys.A);
            this[ClientAction.MoveRight] = new Keybind(Keys.D);
            this[ClientAction.Chat] = new Keybind(Keys.Enter);

            // bind buttons 0-8 to keys D1-D9
            for(int i = 1; i < 10; i++)
                this[0, i - 1] = new Keybind(Keys.D0 + i);

        }

        /// <summary>
        /// Gets or sets the key that activates this game action. 
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public Keybind this[ClientAction act]
        {
            get
            {
                if (keyMap.TryGetValue(act, out var val))
                    return val;
                return Keybind.None;
            }

            set => setKeybind(act, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="Keybind"/> that activates the given spell bar button. 
        /// </summary>
        /// <param name="barId">The spell bar identifier.</param>
        /// <param name="keyId">The key identifier within the spell bar.</param>
        public Keybind this[int barId, int keyId]
        {
            get
            {
                if (keyMap.TryGetValue(ClientActions.FromId(barId, keyId), out var val))
                    return val;
                return Keybind.None;
            }

            set => setKeybind(ClientActions.FromId(barId, keyId), value);
        }

        void setKeybind(ClientAction act, Keybind button)
        {
            //clear keybind if none
            if (button == Keybind.None)
            {
                keyMap.Remove(act);
                return;
            }

            //see if binding is new
            var oldKeyAction = keyMap.FirstOrDefault(kvp => kvp.Value == button).Key;
            if (oldKeyAction != act)
            {
                //remove old binding, if any
                keyMap.Remove(oldKeyAction);

                //add new binding
                keyMap[act] = button;
            }
        }
    }

    static class ClientActions
    {
        const int MaxButtonsPerBar = 20;

        public static ClientAction FromId(int barId, int keyId) =>
            ClientAction.ActionBar_0_0 + barId * MaxButtonsPerBar + keyId;

        public static bool IsBarAction(this ClientAction act) =>
            act >= ClientAction.ActionBar_0_0;

        public static int GetBarId(this ClientAction act) =>
            (act - ClientAction.ActionBar_0_0) / MaxButtonsPerBar;

        public static int GetButtonId(this ClientAction act) =>
            (act - ClientAction.ActionBar_0_0) % MaxButtonsPerBar;
    }
}
