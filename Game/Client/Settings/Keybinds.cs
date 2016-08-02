using Shanism.Client.Input;
using Shanism.Common;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client
{
    class KeybindSettings
    {
        [JsonProperty]
        Dictionary<ClientAction, Keybind> rawKeybinds = new Dictionary<ClientAction, Keybind>();

        public IReadOnlyDictionary<ClientAction, Keybind> BoundActions => rawKeybinds;

        [JsonConstructor]
        KeybindSettings() { }

        public KeybindSettings(bool init)
        {
            if (!init) return;

            this[ClientAction.ToggleMenus] = Keys.Escape;
            this[ClientAction.ToggleCharacterMenu] = Keys.C;
            this[ClientAction.ToggleAbilityMenu] = Keys.B;

            this[ClientAction.ShowHealthBars] = Keys.Tab;

            this[ClientAction.ReloadUi] = new Keybind(ModifierKeys.Control | ModifierKeys.Shift, Keys.R);
            this[ClientAction.ToggleDebugInfo] = Keys.F12;


            this[ClientAction.MoveUp] = Keys.W;
            this[ClientAction.MoveDown] = Keys.S;
            this[ClientAction.MoveLeft] = Keys.A;
            this[ClientAction.MoveRight] = Keys.D;
            this[ClientAction.Chat] = Keys.Enter;


            foreach (var i in Enumerable.Range(1, 9))
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
                Keybind val;
                if (rawKeybinds.TryGetValue(act, out val))
                    return val;
                return Keybind.None;
            }

            set { setKeybind(act, value); }
        }

        public Keybind this[int barId, int keyId]
        {
            get
            {
                Keybind val;
                if (rawKeybinds.TryGetValue(ClientAbilityActions.FromId(barId, keyId), out val))
                    return val;
                return Keybind.None;
            }

            set { setKeybind(ClientAbilityActions.FromId(barId, keyId), value); }
        }

        void setKeybind(ClientAction act, Keybind button)
        {
            //clear keybind if none
            if (button == Keybind.None)
            {
                rawKeybinds.Remove(act);
                return;
            }

            //see if binding is new
            var oldKeyAction = rawKeybinds.FirstOrDefault(kvp => kvp.Value == button).Key;
            if (oldKeyAction != act)
            {
                //remove old binding, if any
                rawKeybinds.Remove(oldKeyAction);

                //add new binding
                rawKeybinds[act] = button;
            }
        }
    }

    static class ClientAbilityActions
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
