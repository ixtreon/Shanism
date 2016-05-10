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
    //unused
    class KeybindSettings
    {

        [JsonProperty]
        Dictionary<GameAction, Keybind> rawKeybinds = new Dictionary<GameAction, Keybind>();


        public IEnumerable<KeyValuePair<GameAction, Keybind>> BoundActions => rawKeybinds;

        [JsonConstructor]
        KeybindSettings() { }

        public KeybindSettings(bool init)
        {
            if (!init) return;

            this[GameAction.ToggleMenus] = Keys.Escape;
            this[GameAction.ToggleCharacterMenu] = Keys.C;
            this[GameAction.ToggleAbilityMenu] = Keys.B;

            this[GameAction.ShowHealthBars] = Keys.Tab;

            this[GameAction.ReloadUi] = new Keybind(ModifierKeys.Control | ModifierKeys.Shift, Keys.R);
            this[GameAction.ToggleDebugInfo] = Keys.F12;


            this[GameAction.MoveUp] = Keys.W;
            this[GameAction.MoveDown] = Keys.S;
            this[GameAction.MoveLeft] = Keys.A;
            this[GameAction.MoveRight] = Keys.D;
            this[GameAction.Chat] = Keys.Enter;


            foreach (var i in Enumerable.Range(1, 9))
                this[0, i - 1] = new Keybind(Keys.D0 + i);


        }

        /// <summary>
        /// Gets or sets the key that activates this game action. 
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public Keybind this[GameAction act]
        {
            get { return rawKeybinds.TryGetVal(act) ?? Keybind.None; }
            set { setKeybind(act, value); }
        }

        public Keybind this[int barId, int keyId]
        {
            get { return rawKeybinds.TryGetVal(AbilityGameAction.FromId(barId, keyId)) ?? Keybind.None; }
            set
            {
                var act = AbilityGameAction.FromId(barId, keyId);
                setKeybind(act, value);
            }
        }

        public Keybind? TryGet(GameAction act)
        {
            return rawKeybinds.TryGetVal(act);
        }

        public Keybind? TryGet(int barId, int keyId)
        {
            var aid = AbilityGameAction.FromId(barId, keyId);
            return rawKeybinds.TryGetVal(aid);
        }

        void setKeybind(GameAction act, Keybind button)
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

    static class AbilityGameAction
    {
        const int MaxButtonsPerBar = 20;

        public static GameAction FromId(int barId, int keyId) =>
            GameAction.ActionBar_0_0 + barId * MaxButtonsPerBar + keyId;

        public static bool IsBarAction(this GameAction act) =>
            act >= GameAction.ActionBar_0_0;

        public static int GetBarId(this GameAction act) =>
            (act - GameAction.ActionBar_0_0) / MaxButtonsPerBar;

        public static int GetButtonId(this GameAction act) =>
            (act - GameAction.ActionBar_0_0) % MaxButtonsPerBar;
    }
}
