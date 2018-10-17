using Shanism.Client.IO;
using Shanism.Client.UI;
using Shanism.Common;

namespace Shanism.Client.Models.UI
{

    public struct MouseEventDispatcher
    {

        readonly MouseSystem Mouse;

        public MouseEventDispatcher(MouseSystem mouse)
        {
            Mouse = mouse;
        }

        public void RaiseUiEvents(Control oldHover, Control newHover)
        {
            // click first (why?)
            doMouseClick(newHover);

            // release
            DoMouseUp(oldHover);

            // change hover
            if (newHover != oldHover && !Mouse.IsDown(MouseButton.Left))
                ChangeHover(oldHover, newHover);

            // move
            if (Mouse.UiPosition != Mouse.OldUiPosition)
                newHover.DoMouseMove(Mouse.UiPosition);

            // press
            DoMouseDown(newHover);

            // scroll
            if (!Mouse.ScrollDelta.Equals(0))
                newHover.DoMouseScroll(Mouse.UiPosition, Mouse.ScrollDelta);
        }

        void ChangeHover(Control oldHover, Control newHover)
        {
            //leave old
            oldHover.DoMouseLeave(Mouse.UiPosition);

            // do drag-drop
            if (Mouse.IsJustReleased(MouseButton.Left) && oldHover.CanDrag)
                newHover.FinishDragDrop(oldHover);

            //enter new
            newHover.DoMouseEnter(Mouse.UiPosition);

        }

        void DoMouseUp(Control target)
        {
            foreach (var btn in Enum<MouseButton>.Values)
                if (Mouse.IsJustReleased(btn))
                    target.DoMouseUp(Mouse.UiPosition, btn);
        }

        void DoMouseDown(Control target)
        {
            foreach (var btn in Enum<MouseButton>.Values)
                if (Mouse.IsJustPressed(btn))
                {
                    if (MouseClickTracker.Default.IsDoubleClick(btn, target, Mouse.UiPosition))
                        target.DoMouseDoubleClick(Mouse.UiPosition, btn);
                    else
                        target.DoMouseDown(Mouse.UiPosition, btn);
                }
        }

        void doMouseClick(Control target)
        {
            foreach (var btn in Enum<MouseButton>.Values)
                if (Mouse.IsJustReleased(btn))
                    target.DoMouseClick(Mouse.UiPosition, btn);
        }
    }
}
