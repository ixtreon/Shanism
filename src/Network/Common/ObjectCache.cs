using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.ObjectStubs;
using Shanism.Network.Serialization;
using Lidgren.Network;
using Shanism.Common;

namespace Shanism.Network.Common
{
    /// <summary>
    /// Holds all GameObjects sent by the server. 
    /// </summary>
    public class ObjectCache
    {
        protected static readonly ObjectMapper Mapper = new ObjectMapper();

        //grows to infinity...
        readonly Dictionary<uint, ObjectStub> _objects = new Dictionary<uint, ObjectStub>();

        readonly HashSet<EntityStub> _visibleEntities = new HashSet<EntityStub>();

        readonly PageReader pages = new PageReader();


        /// <summary>
        /// Gets the game frame at which this cache is currently.
        /// </summary>
        public uint CurrentFrame { get; set; }

        public long? CustomMessageStart { get; set; }


        public IReadOnlyCollection<EntityStub> VisibleEntities => _visibleEntities;

        public IReadOnlyDictionary<uint, ObjectStub> Cache => _objects;


        public ObjectCache(long bufferStart)
        {
            CustomMessageStart = bufferStart;
        }

        public void ReadFrame(NetBuffer msg)
        {
            if(CustomMessageStart != null)
                msg.Position = CustomMessageStart.Value;

            //write previous + current frames ID
            var fromFrame = msg.ReadUInt32();
            var toFrame = msg.ReadUInt32();

            // if a diff from a previous frame, ignore
            if (fromFrame != CurrentFrame)
                return;

            CurrentFrame = toFrame;

            //read vis change mask
            pages.Read(msg);

            // toggle obj. existence
            foreach (var id in pages.ChangedGuids)
            {
                if(_objects.TryGetValue(id, out ObjectStub obj))
                {
                    // was here -> remove
                    _objects.Remove(id);
                    if(obj is EntityStub)
                        _visibleEntities.Remove((EntityStub)obj);
                }
                else
                {
                    // doesn't exist -> add
                    _objects.Add(id, null);
                }
            }

            //update in-game objects with server diff
            var fr = new FieldReader(msg);
            foreach (var kvp in _objects.OrderBy(kvp => kvp.Key))
            {
                var obj = kvp.Value;
                var objType = (ObjectType)fr.ReadByte(0);

                // create or replace the object base, if needed
                if (obj == null || obj.ObjectType != objType)
                {
                    if (obj is EntityStub)
                        _visibleEntities.Remove((EntityStub)obj);

                    _objects[kvp.Key] = obj = Mapper.Create(objType, kvp.Key);

                    if (obj is EntityStub)
                        _visibleEntities.Add((EntityStub)obj);
                }

                // update the object state
                Mapper.Read(objType, obj, fr);
            }
        }
    }
}