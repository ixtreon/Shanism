using Shanism.Common;
using Shanism.Common.StubObjects;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Game;
using Shanism.Common.Message.Network;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;
using Shanism.Common.Serialization;
using System.IO;
using Shanism.Common.Interfaces.Objects;

namespace Shanism.Network.Client
{
    /// <summary>
    /// Holds all GameObjects sent by the server and performs RangeQueries for the client. 
    /// </summary>
    class ObjectCache : IObjectCache
    {
        readonly NetworkSerializer serializer = new NetworkSerializer();

        readonly Dictionary<uint, ObjectStub> _objectCache = new Dictionary<uint, ObjectStub>();

        readonly HashSet<uint> _unitsSeen = new HashSet<uint>();


        public IGameObject SeeObject(PlayerStatusMessage msg)
        {
            return getOrAdd(msg.HeroId, ObjectType.Hero);
        }

        IGameObject getOrAdd(uint objId, ObjectType objType)
        {
            ObjectStub obj;
            if (!TryGetValue(objId, out obj))
            {
                obj = serializer.Create(objId, objType);
                Add(objId, obj);
            }

            _unitsSeen.Add(obj.Id);

            return obj;
        }

        public IGameObject SeeObject(ObjectSeenMessage msg)
        {
            if (msg.ObjectType > ObjectType.Hero)
                throw new ArgumentException(nameof(msg), $"Did not expect to \"see\" a {msg.ObjectType}... ");

            //get or create the object in the cache
            return getOrAdd(msg.ObjectId, msg.ObjectType);
        }

        public bool TryGetValue(uint id, out ObjectStub obj)
            => _objectCache.TryGetValue(id, out obj);

        public void Add(uint id, ObjectStub obj)
            => _objectCache.Add(id, obj);


        public bool UnseeObject(ObjectUnseenMessage msg)
        {
            return _unitsSeen.Remove(msg.ObjectId);
        }

        internal void UpdateGame(GameFrameMessage msg)
        {
            serializer.ReadObjectStream(this, msg);
        }

    }
}
