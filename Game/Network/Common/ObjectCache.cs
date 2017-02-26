using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.StubObjects;
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

        public long BufferStart { get; set; } = 0;


        public IReadOnlyCollection<EntityStub> VisibleEntities => _visibleEntities;

        public IReadOnlyDictionary<uint, ObjectStub> Cache => _objects;


        public ObjectCache(long bufferStart)
        {
            BufferStart = bufferStart;
        }

        public void ReadFrame(NetBuffer msg)
        {
            msg.Position = BufferStart;

            //write previous + current frames ID
            var fromFrame = msg.ReadUInt32();
            var toFrame = msg.ReadUInt32();

            // if a diff from a previous frame, ignore
            if (fromFrame != CurrentFrame)
                return;



            //read vis change mask
            pages.Read(msg);
            foreach (var id in pages.ChangedGuids)
            {
                ObjectStub existingObject;
                if (_objects.TryGetValue(id, out existingObject))
                {
                    _objects.Remove(id);
                    if (existingObject is EntityStub)
                        _visibleEntities.Remove((EntityStub)existingObject);
                }
                else
                {
                    _objects.Add(id, null);
                }
            }

            //update in-game objects with server diff
            var fr = new FieldReader(msg);
            foreach (var kvp in _objects.OrderBy(kvp => kvp.Key))
            {
                var curObj = kvp.Value;
                var curObjType = (ObjectType)fr.ReadByte(0);

                //create or replace the object, if needed
                if (curObj == null || curObj.ObjectType != curObjType)
                {
                    if (curObj is EntityStub)
                        _visibleEntities.Remove((EntityStub)curObj);

                    _objects[kvp.Key] = curObj = Mapper.Create(curObjType, kvp.Key);

                    if (curObj is EntityStub)
                        _visibleEntities.Add((EntityStub)curObj);
                }

                Mapper.Read(curObj, fr);
            }

            CurrentFrame = toFrame;

        }
    }
}