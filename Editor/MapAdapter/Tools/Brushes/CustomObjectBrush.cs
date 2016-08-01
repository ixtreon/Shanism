using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Game;
using Shanism.Common.StubObjects;
using Shanism.ScenarioLib;
using Microsoft.Xna.Framework;
using Shanism.Client;
using Shanism.Common.Message;
using Shanism.Engine;
using Shanism.Engine.Objects;
using Shanism.Engine.Entities;
using Shanism.Common.Message.Server;
using Shanism.Common;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Editor.MapAdapter
{
    class CustomObjectBrush : Brush
    {
        Entity Object { get; set; }

        public CustomObjectBrush(IEditorEngine engine, IEntity objProto)
            : base(engine)
        {
            recreateObject(objProto.GetType().FullName);
        }

        void recreateObject(string objType)
        {
            var objOwner = Player.Aggressive;

            var oc = new ObjectConstructor
            {
                TypeName = objType,
                Owner = objOwner.ToString(),
            };

            Object = Engine.CreateObject(oc);

            if (Object == null)
            {
                //TODO: handle missing type name
            }
        }

        public override void OnDraw(IEditorMapControl control, Vector inGamePos)
        {
            if (Object == null)
                return;

            var llPos = control.Client.GameToScreen(inGamePos - Object.Scale / 2);
            var urPos = control.Client.GameToScreen(inGamePos + Object.Scale / 2);
            var tex = control.EditorContent.Circle;
            var c = canPlace(inGamePos) ? Color.Blue : Color.Red;

            control.SpriteBatch.ShanoDraw(tex, llPos, urPos - llPos, c.SetAlpha(50));
        }

        bool canPlace(Vector pos) => !Engine.StartupObjects.Any(o => o.Position.DistanceTo(pos) < (o.Scale + Object.Scale) / 2);

        public override IOMessage Place(Vector inGamePos)
        {
            if (Object == null)
                return null;

            //var objCtor = new ObjectConstructor(Object, inGamePos);

            Object.Position = inGamePos;

            //check against other placed objects
            //TODO: check if objs have collision
            if (!canPlace(inGamePos))
                return null;

            //add to client engine
            Engine.AddObject(Object);

            //add to scenario config
            Map.Objects.Add(new ObjectConstructor
            {
                Location = inGamePos,
                TypeName = Object.GetType().FullName,
            });

            recreateObject(Object.GetType().FullName);
            return null;
        }
    }
}
