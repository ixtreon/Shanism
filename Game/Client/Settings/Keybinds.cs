using Client.Input;
using IO;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Settingsz
{
    //unused
    class KeybindSettings
    {
        const int MaxButtonsPerBar = 100;

        [JsonProperty]
        Dictionary<GameAction, Keybind> rawKeybinds = new Dictionary<GameAction, Keybind>();

        [JsonConstructor]
        KeybindSettings() { }

        public KeybindSettings(bool init)
        {
            if (!init) return;

            this[GameAction.ToggleMainMenu] = Keys.Escape;

            this[GameAction.ToggleCharacterMenu] = Keys.C;
            this[GameAction.ToggleSpellBook] = Keys.P;

            this[GameAction.Chat] = Keys.Enter;
            this[GameAction.ShowHealthBars] = Keys.Tab;

            this[GameAction.MoveUp] = Keys.W;
            this[GameAction.MoveDown] = Keys.S;
            this[GameAction.MoveLeft] = Keys.A;
            this[GameAction.MoveRight] = Keys.D;

            this[GameAction.ReloadUi] = new Keybind(ModifierKeys.Control | ModifierKeys.Shift, Keys.R);

            foreach(var i in Enumerable.Range(1, 9))
                this[0, i - 1] = new Keybind(Keys.D0 + i);


        }

        /// <summary>
        /// Gets or sets the key that activates this game action. 
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public Keybind this[GameAction act]
        {
            get { return rawKeybinds[act]; }
            set
            {
                setKeybind(act, value);
            }
        }

        public Keybind this[int barId, int keyId]
        {
            get { return rawKeybinds[actionId(barId, keyId)]; }
            set
            {
                var act = actionId(barId, keyId);
                setKeybind(act, value);
            }
        }

        public Keybind? TryGet(GameAction act)
        {
            return rawKeybinds.TryGetVal(act);
        }

        public Keybind? TryGet(int barId, int keyId)
        {
            var aid = actionId(barId, keyId);
            return rawKeybinds.TryGetVal(aid);
        }

        void setKeybind(GameAction act, Keybind button)
        {
            //see what this was doing
            var oldKeyAction = rawKeybinds.FirstOrDefault(kvp => kvp.Value == button).Key;
            if (oldKeyAction != act)
            {
                //remove old functionality
                rawKeybinds.Remove(oldKeyAction);

                //add new binding
                rawKeybinds[act] = button;
            }
        }

        static GameAction actionId(int barId, int keyId)
        {
            return GameAction.ActionBar + barId * MaxButtonsPerBar + keyId;
        }
    }
}
