using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.Message.Network;
using Shanism.Common.StubObjects;
using Shanism.Common.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Network.Client
{
    public class NetworkSerializer
    {
        GameSerializer serializer = new GameSerializer();

        public ObjectStub Create(uint id, ObjectType ty)
            => serializer.Create(new ObjectHeader { Id = id, Type = ty });

        public void ReadObjectStream(IObjectCache objCache, GameFrameMessage msg)
        {
            var l = new List<ObjectStub>();

            using (var ms = new MemoryStream(msg.Data))
            using (var r = new BinaryReader(ms))
            {
                //read # items
                var nItems = r.ReadInt32();

                for (var i = 0; i < nItems; i++)
                {
                    //read type, id
                    var h = serializer.ReadHeader(r);

                    //fetch object
                    ObjectStub obj;
                    if (!objCache.TryGetValue(h.Id, out obj))
                    {
                        obj = serializer.Create(h);
                        objCache.Add(h.Id, obj);
                    }

                    serializer.Update(r, h, obj);

                    //save to update list
                    if (obj is EntityStub)
                        l.Add(obj);
                }
                if (ms.Length != ms.Position)
                    Log.Default.Warning("ServerFrame stream was longer than expected!");
            }

            foreach (var obj in l)
                UpdateObjectIds(objCache, obj);
        }

        public void UpdateObjectIds(IObjectCache cache, IGameObject obj)
        {
            switch (obj.ObjectType)
            {
                case ObjectType.Hero:
                    var h = (HeroStub)obj;


                    goto case ObjectType.Unit;

                case ObjectType.Unit:
                    var u = (UnitStub)obj;


                    goto case ObjectType.Doodad;

                case ObjectType.Doodad:
                case ObjectType.Effect:
                    break;
            }
        }
    }
}
