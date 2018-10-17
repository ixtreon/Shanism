using Shanism.Client.IO;
using System.Numerics;

namespace Shanism.Client.UI
{
    partial class Control
    {

        // keyboard

        public void PressKey(Keybind k) => OnKeyPress(new KeyboardArgs(k));

        public void ReleaseKey(Keybind k) => OnKeyRelease(new KeyboardArgs(k));

        public void InputChar(Keybind k) => OnCharInput(new KeyboardArgs(k));

        public void ActivateAction(ClientAction act) => OnActionActivated(new ClientActionArgs(act));


        // mouse up + down

        public void DoMouseDown(Vector2 absolutePosition, MouseButton button)
            => OnMouseDown(new MouseButtonArgs(this, absolutePosition, button));

        public void DoMouseUp(Vector2 absolutePosition, MouseButton button)
            => OnMouseUp(new MouseButtonArgs(this, absolutePosition, button));

        // mouse move/enter/leave

        public void DoMouseMove(Vector2 absolutePosition)
            => OnMouseMove(new MouseArgs(this, absolutePosition));

        public void DoMouseLeave(Vector2 absolutePosition)
            => OnMouseLeave(new MouseArgs(this, absolutePosition));

        public void DoMouseEnter(Vector2 absolutePosition)
            => OnMouseEnter(new MouseArgs(this, absolutePosition));

        // mouse click/scroll

        public void DoMouseClick(Vector2 absolutePosition, MouseButton button)
            => OnMouseClick(new MouseButtonArgs(this, absolutePosition, button));

        public void DoMouseDoubleClick(Vector2 absolutePosition, MouseButton button)
            => OnMouseDoubleClick(new MouseButtonArgs(this, absolutePosition, button));

        public void DoMouseScroll(Vector2 absolutePosition, float delta)
            => OnMouseScroll(new MouseScrollArgs(this, absolutePosition, delta));

        // drag & drop

        public void FinishDragDrop(Control source)
        {
            var args = new DragDropArgs(source, this);

            source.OnDragEnd(args);
            OnDropEnd(args);
        }
    }
}
