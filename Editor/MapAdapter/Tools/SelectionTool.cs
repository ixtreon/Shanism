using Microsoft.Xna.Framework;
using Shanism.Client;
using Shanism.Common;
using Shanism.Common.Util;
using Shanism.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shanism.Editor.MapAdapter
{
    class SelectionTool : MapTool
    {
        bool isSelecting;
        RectangleF selectionRect;

        HashSet<Entity> selectedObjects = new HashSet<Entity>();


        public event Action<IEnumerable<Entity>> SelectionChanged;

        public SelectionTool(IEditorEngine e) : base(e)
        {

        }


        public override void OnMouseDown(MouseButtons btn, Vector inGamePos)
        {
            if (btn != MouseButtons.Left) return;

            isSelecting = true;
            selectionRect = new RectangleF(inGamePos, Vector.Zero);
        }

        public override void OnMouseMove(MouseButtons btn, Vector inGamePos)
        {
            if (btn != MouseButtons.Left) return;

            if (isSelecting)
                selectionRect = new RectangleF(selectionRect.Position, inGamePos - selectionRect.Position);
        }

        public override void OnMouseUp(MouseButtons btn, Vector inGamePos)
        {
            if (btn != MouseButtons.Left) return;

            isSelecting = false;
            selectionRect = new RectangleF(selectionRect.Position, inGamePos - selectionRect.Position)
                .MakePositive();

            //de-tint old selection
            foreach (var obj in selectedObjects)
                obj.CurrentTint = obj.DefaultTint;

            selectedObjects = new HashSet<Entity>(Engine.StartupObjects
                .Where(obj => obj.Position.DistanceTo(selectionRect) < obj.Scale / 2));

            //tint new selection
            foreach (var obj in selectedObjects)
                obj.CurrentTint = Common.Util.Color.Blue;

            SelectionChanged?.Invoke(selectedObjects);
        }

        public override void Dispose()
        {
            foreach (var obj in selectedObjects)
                obj.CurrentTint = obj.DefaultTint;
        }

        public override void OnDraw(IEditorMapControl control, Vector inGamePos)
        {
            var selectionColor = Microsoft.Xna.Framework.Color.Blue;

            if (isSelecting)
            {
                var selection = selectionRect.MakePositive();
                var objStart = control.Client.GameToScreen(selection.Position);
                var objEnd = control.Client.GameToScreen(selection.FarPosition);

                control.SpriteBatch.ShanoDraw(control.EditorContent.Blank, objStart, objEnd - objStart, selectionColor.SetAlpha(100));
            }

            //var sb = control.SpriteBatch;
            //foreach (var obj in selectedObjects)
            //{
            //    var objStart = control.Engine.GameToScreen(obj.Position - obj.Scale / 2);
            //    var objEnd = control.Engine.GameToScreen(obj.Position + obj.Scale / 2);

            //    sb.ShanoDraw(control.EditorContent.CircleOutline, objStart, objEnd - objStart, selectionColor);
            //}
        }

        public override void OnKeyPress(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    if (!selectedObjects.Any()) return;

                    //remove from the client engine
                    foreach (var o in selectedObjects)
                        Engine.RemoveObject(o);

                    //remove from the scenario config
                    Map.Objects.RemoveAll(oc => selectionRect.Contains(oc.Location));

                    selectedObjects.Clear();
                    break;
            }
        }
    }
}
