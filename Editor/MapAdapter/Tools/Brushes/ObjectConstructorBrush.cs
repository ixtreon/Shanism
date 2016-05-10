using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Game;
using Shanism.Common.Objects;
using Shanism.ScenarioLib;
using Microsoft.Xna.Framework;
using Shanism.Client;
using Shanism.Common.Message;
using Shanism.Engine;
using Shanism.Engine.Objects;
using Shanism.Engine.Objects.Entities;
using Shanism.Common.Message.Server;
using Shanism.Common;

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
            var llPos = control.Client.GameToScreen(inGamePos - @object.Scale / 2);
            var urPos = control.Client.GameToScreen(inGamePos + @object.Scale / 2);
            var tint = Color.White;
            var tex = control.DefaultContent.TryGetRaw(@object.AnimationName);
            if (tex == null)
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

        public override IOMessage Place(Vector inGamePos)
        {
            //check against other placed objects
            if (!canPlace(inGamePos))
                return null;

            //add to client engine
            @object.Position = lastPlacedPos = inGamePos;
            Engine.AddObject(@object);
            var msg = new ObjectSeenMessage(@object);

            //add to scenario config
            var oc = constructor.Clone();
            oc.Location = inGamePos;
            Map.Objects.Add(oc);

            //create a fresh instance of that object constructor
            recreateObject();

            return msg;
        }
    }
}
