using Shanism.Common.Game;
using Shanism.Common.Message.Network;
using Shanism.Common.StubObjects;
using Shanism.Common.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using Shanism.Common.Message.Client;

namespace Shanism.Network.Client
{
    public class ClientSerializer
    {
        readonly GameSerializer serializer = new GameSerializer();

        public GameFrameMessage WriteClientFrame(uint gameFrame, ClientState state)
        {
            using (var ms = new MemoryStream())
            {
                ms.WriteUint24(gameFrame);
                ProtoBuf.Serializer.Serialize(ms, state);

                var bytes = ms.ToArray();
                return new GameFrameMessage(bytes);
            }
        }

        /// <summary>
        /// Reads the server frame saving all visible units. 
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="objCache">The object cache.</param>
        /// <param name="visibleObjects">The visible objects.</param>
        public void ReadServerFrame(GameFrameMessage msg, 
            IDictionary<uint, ObjectStub> objCache, ICollection<EntityStub> visibleObjects)
        {
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
                        visibleObjects.Add((EntityStub)obj);
                }

                if (ms.Length != ms.Position)
                    Log.Default.Warning("ServerFrame stream was longer than expected!");
            }
        }

        public ObjectStub Create(ObjectType objType, uint objId)
            => serializer.Create(objType, objId);

    }
}
