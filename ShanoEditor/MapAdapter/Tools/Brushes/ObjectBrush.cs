using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;
using IO.Objects;
using ScenarioLib;
using Microsoft.Xna.Framework;
using Client;
using IO.Message;
using Engine;
using Engine.Objects;
using Engine.Objects.Entities;
using IO.Message.Server;

namespace ShanoEditor.MapAdapter
{
    class ObjectConstructorBrush : Brush
    {
        readonly ObjectConstructor ocPrototype;

        public ObjectConstructorBrush(IEditorEngine engine, ObjectConstructor oc)
            : base(engine)
        {
            ocPrototype = oc;
        }

        public override void OnDraw(IEditorMapControl control, Vector inGamePos)
        {
            var llPos = control.Engine.GameToScreen(inGamePos - ocPrototype.Size / 2);
            var urPos = control.Engine.GameToScreen(inGamePos + ocPrototype.Size / 2);
            var tex = control.EditorContent.Circle;
            var c = canPlace(inGamePos) ? Color.Blue : Color.Red;

            control.SpriteBatch.ShanoDraw(tex, llPos, urPos - llPos, c.SetAlpha(50));
        }

        bool canPlace(Vector pos) 
            => !Engine.StartupObjects
            .Any(o => o.Position.DistanceTo(pos) < (o.Scale + ocPrototype.Size) / 2);

        public override IOMessage Place(Vector inGamePos)
        {
            var oc = ocPrototype.Clone();
            oc.Location = inGamePos;

            var obj = Engine.CreateObject(oc);

            //check against other placed objects
            if (!canPlace(inGamePos))
                return null;

            //add to client engine
            Engine.AddObject(obj);

            //add to scenario config
            Map.Objects.Add(oc);

            var msg = new ObjectSeenMessage(obj);

            return msg;
        }
    }
}
