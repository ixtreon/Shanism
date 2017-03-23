using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.Input;

namespace Shanism.Client.UI
{
    /// <summary>
    /// The main control within the UIx system.
    /// Only one root control can be active at a time.
    /// </summary>
    public class RootControl : Control
    {
        /// <summary>
        /// Gets the first control that is under the mouse pointer
        /// and has <see cref="Control.CanHover"/> set to <c>true</c>. 
        /// </summary>
        public static Control GlobalHover { get; private set; }

        /// <summary>
        /// Gets the control that currently has keyboard focus. 
        /// A control must have its <see cref="Control.CanFocus"/> property set to <c>true</c> in order to become the <see cref="GlobalFocus"/>.
        /// </summary>
        public static Control GlobalFocus { get; private set; }



        Control hover, focus;

        public RootControl()
        {
            Root = this;
            CanHover = true;
            CanFocus = true;

            hover = this;
            focus = this;
        }

        public void UpdateMain(int msElapsed)
        {
            if (GlobalFocus == null || !GlobalFocus.IsChildOf(this))
                focus = this;

            var newHover = getHover(MouseInfo.UiPosition - Location);

            raiseMouseEvents(hover, newHover);

            raiseKeyboardEvents(GlobalFocus);

            hover = newHover;

            //update assembly-wide hover & focus
            GlobalHover = hover;
            GlobalFocus = focus;
        }

        public void SetFocus(Control c)
        {
            if (c.Root != this)
                throw new InvalidOperationException("Trying to focus a control that's not inside this root!");
            
            if(c.CanFocus)
                focus = c;
        }

        protected override void OnUpdate(int msElapsed)
        {
            base.OnUpdate(msElapsed);
        }
    }
}
