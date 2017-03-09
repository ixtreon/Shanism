using System.Linq;
using Shanism.ScenarioLib;
using Shanism.Engine;
using Shanism.Common;
using Microsoft.Xna.Framework.Graphics;

namespace Shanism.Editor.MapAdapter
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shanism.Editor.MapAdapter.Brush" />
    class ObjectConstructorBrush : Brush
    {
        readonly ObjectConstructor constructor;

        Entity @object;

        Vector lastPlacedPos = Vector.MinValue;

        public ObjectConstructorBrush(IEditorEngine engine, ObjectConstructor oc)
            : base(engine)
        {
            constructor = oc;

            recreateObject();
        }


        void recreateObject()
        {
            @object = Engine.CreateObject(constructor.Clone());
        }

        /// <summary>
        /// Called when [draw].
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="inGamePos">The in game position.</param>
        public override void OnDraw(IEditorMapControl control, Vector inGamePos)
        {
            var llPos = control.GameClient.GameToScreen(inGamePos - @object.Scale / 2);
            var urPos = control.GameClient.GameToScreen(inGamePos + @object.Scale / 2);
            var tint = Color.White;

            Texture2D tex;
            if (!control.DefaultContent.TryGet(@object.Model, out tex))
            {
                tex = control.EditorContent.Circle;
                tint = (canPlace(inGamePos) ? Color.Blue : Color.Red).SetAlpha(50);
            }

            control.SpriteBatch.ShanoDraw(tex, llPos, urPos - llPos, tint);
        }

        bool canPlace(Vector pos) =>
            (!@object.HasCollision && lastPlacedPos.DistanceTo(pos) > @object.Scale / 2) ||
            !Engine.StartupObjects
                .Any(o => o.Position.DistanceTo(pos) < (o.Scale + @object.Scale) / 2);

        public override void Place(Vector inGamePos)
        {
            //check against other placed objects
            if (!canPlace(inGamePos))
                return;

            //add to client engine
            @object.Position = lastPlacedPos = inGamePos;
            Engine.AddObject(@object);

            //add to scenario config
            var oc = constructor.Clone();
            oc.Location = inGamePos;
            Map.Objects.Add(oc);

            //create a fresh instance of that object constructor
            recreateObject();
        }
    }
}
