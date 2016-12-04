using Shanism.Common;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.Message.Network;
using Shanism.Common.Serialization;
using Shanism.Common.StubObjects;
using Shanism.Network.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shanism.Network.Common
{
    public class ServerFrameBuilder
    {
        GameSerializer serializer = new GameSerializer();

        ObjectBitmaskBuilder bitmaskBuilder = new ObjectBitmaskBuilder();

        public GameFrameMessage Write(IReceptor receptor)
        {
            var visibleObjects = new HashSet<ObjectData>();
            foreach (var e in receptor.VisibleEntities)
                FetchObjectIds(receptor.Id, e, visibleObjects);

            //create bitmask of object ids
            var objs = visibleObjects.ToList();

            using (var ms = new MemoryStream())
            {
                //write # items followed by the items
                using (var wr = new BinaryWriter(ms))
                {
                    wr.Write(visibleObjects.Count);

                    bitmaskBuilder.WriteBitmask(visibleObjects, wr);

                    foreach (var obj in visibleObjects)
                        serializer.Write(wr, obj.Object, obj.Type);
                }

                var bytes = ms.ToArray();
                return new GameFrameMessage(0, bytes);
            }
        }

        /// <summary>
        /// Reads the server frame saving all visible units. 
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="objCache">The object cache.</param>
        /// <param name="visibleObjects">The visible objects.</param>
        public void Read(GameFrameMessage msg,
            IDictionary<uint, ObjectStub> objCache, ICollection<EntityStub> visibleObjects)
        {
            using (var ms = new MemoryStream(msg.Data))
            using (var r = new BinaryReader(ms))
            {
                //read # items
                var nItems = r.ReadInt32();

                //read items ids
                bitmaskBuilder.ReadBitmask(r);
                var l = bitmaskBuilder.VisibleIds;

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
                        visibleObjects.Add((EntityStub)obj);
                }

                if (ms.Length != ms.Position)
                    Log.Default.Warning("ServerFrame stream was longer than expected!");
            }
        }

        /// <summary>
        /// Appends all objects related to the given entity 
        /// as seen by the player with the given ID
        /// into the specified collection.
        /// </summary>
        static void FetchObjectIds(uint playerId, IEntity nearbyEntity,
            ICollection<ObjectData> c)
        {
            var writeAsType = nearbyEntity.ObjectType;

            if (nearbyEntity is IHero && ((IHero)nearbyEntity).OwnerId != playerId)
                writeAsType = ObjectType.Unit;

            c.Add(new ObjectData { Object = nearbyEntity, Type = writeAsType });
        }

    }

}
