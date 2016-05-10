using Shanism.Common;
using Shanism.Common.Objects;
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
using Shanism.Common.Interfaces.Engine;
using System.IO;

namespace Shanism.Network.Client
{
    /// <summary>
    /// Holds all GameObjects sent by the server and performs RangeQueries for the client. 
    /// </summary>
    class ObjectCache : IObjectCache
    {
        readonly Dictionary<uint, IGameObject> _objectCache = new Dictionary<uint, IGameObject>();

        readonly HashSet<uint> _unitsSeen = new HashSet<uint>();


        public IGameObject SeeObject(ObjectSeenMessage msg)
        {
            if (msg.ObjectType > ObjectType.Hero)
                throw new ArgumentException(nameof(msg), $"Did not expect to \"see\" a {msg.ObjectType}... ");

            //get or create the object in the cache
            var obj = GetOrAdd(msg.ObjectType, msg.ObjectId);

            _unitsSeen.Add(obj.Id);

            return obj;
        }

        public IGameObject GetOrAdd(ObjectType objType, uint id)
        {
            IGameObject obj;
            if (!_objectCache.TryGetValue(id, out obj))
            {
                obj = StubFactory.Create(objType, id);
                _objectCache[id] = obj;
            }

            return obj;
        }


        public bool UnseeObject(ObjectUnseenMessage msg)
        {
            return _unitsSeen.Remove(msg.ObjectId);
        }

        internal void UpdateGame(GameFrameMessage msg)
        {
            var objs = ShanoWriter.ReadObjectStream(this, msg)
                .OfType<EntityStub>()
                .ToList();

            foreach (var obj in objs)
                UpdateObjectIds(obj);
        }

        /// <summary>
        /// Reverse of <see cref="Engine.Serialization.ShanoReader.FetchObjectIds"/>...
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateObjectIds(EntityStub obj)
        {
            switch(obj.ObjectType)
            {
                case ObjectType.Hero:
                    var h = (HeroStub)obj;
                    h.Abilities = h.AbilityIds
                        .Select(id => GetOrAdd(ObjectType.Ability, id))
                        .Cast<IAbility>()
                        .ToList();

                    goto case ObjectType.Unit;

                case ObjectType.Unit:
                    var u = (UnitStub)obj;

                    u.Buffs = u.BuffIds
                        .Select(id => GetOrAdd(ObjectType.BuffInstance, id))
                        .Cast<IBuffInstance>()
                        .ToList();

                    //yield return ((Unit)obj).Owner.Id; -- not a gameobject!!
                    goto case ObjectType.Doodad;

                case ObjectType.Doodad:
                case ObjectType.Effect:
                    break;
            }
        }
    }
}
