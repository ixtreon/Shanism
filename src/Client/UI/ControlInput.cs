using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.Input;

namespace Shanism.Client.UI
{
    partial class Control
    {

        #region Mouse Input

        MouseArgs mouseBaseArgs => new MouseArgs(this, MouseInfo.UiPosition);
        MouseButtonArgs mouseLeftButtonArgs => new MouseButtonArgs(this, MouseInfo.UiPosition, MouseButton.Left);
        MouseButtonArgs mouseRightButtonArgs => new MouseButtonArgs(this, MouseInfo.UiPosition, MouseButton.Right);


        protected static void raiseMouseEvents(Control hover, Control newHover)
        {
            var isSameControl = (newHover == hover);

            //button release
            if (MouseInfo.LeftJustReleased)
                hover.raiseMouseReleasedEvents(hover.mouseLeftButtonArgs, isSameControl);

            if (MouseInfo.RightJustReleased)
                hover.raiseMouseReleasedEvents(hover.mouseRightButtonArgs, isSameControl);

            //hover update
            if (!MouseInfo.LeftDown)    //keep hover as long as left button is down
            {
                if (!isSameControl)
                {
                    //leave old control
                    hover.MouseLeave?.Invoke(hover.mouseBaseArgs);

                    // if we also released the button this frame and the source 
                    // supports drag-drop, raise the drag-drop events
                    //
                    // hover is kept when mouse is down 
                    // so that's what a drag-drop is
                    if (MouseInfo.LeftJustReleased && hover.CanDrag)
                    {
                        hover.OnDrag?.Invoke(newHover);
                        newHover.OnDrop?.Invoke(hover);
                    }

                    hover = newHover;

                    //enter new control
                    hover.MouseEnter?.Invoke(hover.mouseBaseArgs);
                }
            }


            //mouse move
            if (MouseInfo.UiPosition != MouseInfo.OldUiPosition)
                hover.MouseMove?.Invoke(hover.mouseBaseArgs);

            //button press
            if (MouseInfo.LeftJustPressed)
                hover.raiseMousePressEvents(hover.mouseLeftButtonArgs);

            if (MouseInfo.RightJustPressed)
                hover.raiseMousePressEvents(hover.mouseRightButtonArgs);

            //focus control
            if ((MouseInfo.LeftJustPressed || MouseInfo.RightJustPressed) && hover != null)
            {
                var c = hover;
                while (!c.CanFocus && c.Parent != null)
                    c = c.Parent;

                c.SetFocus();
            }
        }


        void raiseMouseReleasedEvents(MouseButtonArgs buttonReleasedArgs, bool isCursorInsideControl)
        {
            MouseUp?.Invoke(buttonReleasedArgs);

            //raise MouseClick only if mouse is inside the control
            if (isCursorInsideControl)
                MouseClick?.Invoke(buttonReleasedArgs);
        }

        void raiseMousePressEvents(MouseButtonArgs args)
        {
            MouseDown?.Invoke(args);
        }

        #endregion

        #region Keyboard Input

        protected static void raiseKeyboardEvents(Control focus)
        {
            if (focus == null)
                return;
            
            //release then press
            //actions then keys

            foreach (var k in KeyboardInfo.JustReleasedKeys)
                if (!k.IsModifier())
                    focus.KeyReleased?.Invoke(new Keybind(KeyboardInfo.Modifiers, k));


            foreach (var k in KeyboardInfo.JustPressedKeys)
                if (!k.IsModifier())
                    focus.KeyPressed?.Invoke(new Keybind(KeyboardInfo.Modifiers, k));

            foreach (var ga in KeyboardInfo.JustActivatedActions)
                focus.GameActionActivated?.Invoke(ga);
        }

        #endregion
    }
}
