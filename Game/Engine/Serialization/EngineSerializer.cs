using Shanism.Common.Game;
using Shanism.Common.Message.Network;
using Shanism.Common.Serialization;
using Shanism.Engine.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shanism.Engine.Serialization
{

    public class EngineSerializer
    {
        GameSerializer serializer = new GameSerializer();


        public GameFrameMessage PrepareGameFrame(Player pl, IEnumerable<Entity> visibleObjects)
        {
            var relatedObjects = new HashSet<ObjectData>();
            foreach (var e in visibleObjects)
                FetchObjectIds(pl, e, relatedObjects);

            byte[] datas;
            using (var ms = new MemoryStream())
            {
                using (var wr = new BinaryWriter(ms))
                {
                    //write # items
                    wr.Write(relatedObjects.Count);

                    foreach (var obj in relatedObjects)
                        serializer.Write(wr, obj.Object, obj.Type);
                }
                datas = ms.ToArray();
            }

            return new GameFrameMessage(datas);
        }

        static void FetchObjectIds(Player pl, Entity obj, ICollection<ObjectData> l)
        {
            var writeAsType = obj.ObjectType;
            switch (obj.ObjectType)
            {
                case ObjectType.Hero:
                    //write as hero only if we own it
                    if (((Hero)obj).Owner != pl)
                    {
                        writeAsType = ObjectType.Unit;
                        goto case ObjectType.Unit;
                    }

                    //foreach (var ability in ((Hero)obj).Abilities)
                    //    l.Add(new ObjectData
                    //    {
                    //        Object = ability,
                    //        Type = ObjectType.Ability,
                    //    });

                    goto case ObjectType.Unit;

                case ObjectType.Unit:
                    //foreach (var buff in ((Unit)obj).Buffs)
                    //    l.Add(new ObjectData
                    //    {
                    //        Object = buff,
                    //        Type = ObjectType.BuffInstance,
                    //    });

                    goto case ObjectType.Doodad;

                case ObjectType.Doodad:
                case ObjectType.Effect:
                    l.Add(new ObjectData
                    {
                        Object = obj,
                        Type = writeAsType,
                    });
                    break;
            }
        }
        struct ObjectData
        {
            public GameObject Object;

            public ObjectType Type;


            //dreams of FP lands...
            public static bool operator ==(ObjectData a, ObjectData b)
                => a.Object.Id == b.Object.Id;

            public static bool operator !=(ObjectData a, ObjectData b)
                => !(a == b);

            public override bool Equals(object obj)
                => (obj is ObjectData)
                && ((ObjectData)obj) == this;

            public override int GetHashCode()
                => (int)Object.Id;
        }

    }
}
