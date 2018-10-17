using Shanism.Client.IO;
using Shanism.Client.UI;
using System.Collections.Generic;

namespace Shanism.Client.Views
{
    partial class View
    {

        /// <summary>
        /// Override to implement a custom reload handler. 
        /// Executed once after <see cref="OnInit"/> and then every time
        /// the <see cref="ClientAction.ReloadUi"/> action is triggered.
        /// </summary>
        protected virtual void OnReload() { }

        /// <summary>
        /// Override to implement a custom initialization handler. 
        /// Executed once this view gets its game.
        /// </summary>
        protected virtual void OnInit() { }

        public virtual void WriteDebugStats(List<string> debugStats) { }


        protected virtual void OnKeyPress(Control target, Keybind k)
            => target.PressKey(k);

        protected virtual void OnKeyRelease(Control target, Keybind k)
            => target.ReleaseKey(k);

        protected virtual void OnCharacterInput(Control target, Keybind k)
            => target.InputChar(k);

        protected virtual void OnActionActivated(Control target, ClientAction act)
        {
            switch (act)
            {
                case ClientAction.ReloadUi:
                    OnReload();
                    break;

                default:
                    target.ActivateAction(act);
                    break;
            }
        }
    }
}
