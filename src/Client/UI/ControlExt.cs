using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI
{
    public static class ControlExt
    {

        public static void AddMouseDragEvent(this Control c, MouseButton button, UiEventHandler<MouseArgs> dragHandler)
            => AddMouseDragEvent(c, button, null, dragHandler, null);

        public static void AddMouseDragEvent(this Control c, MouseButton button,
            CancellableUiEventHandler<MouseButtonArgs> startHandler,
            UiEventHandler<MouseArgs> dragHandler,
            UiEventHandler<MouseButtonArgs> endHandler = null)
        {
            var isDragging = false;

            c.MouseDown += (o, e) =>
            {
                if (e.Button != button) 
                    return;

                if (startHandler?.Invoke(o, e) != false)
                    isDragging = true;
            };

            c.MouseMove += (o, e) =>
            {
                if (isDragging)
                    dragHandler?.Invoke(o, e);
            };

            c.MouseUp += (o, e) =>
            {
                if (e.Button != button) return;

                isDragging = false;
                endHandler?.Invoke(o, e);
            };
        }
    }
}
