using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.StubObjects;
using Shanism.Network.Serialization;
using Lidgren.Network;
using Shanism.Common;
using Shanism.Network.Common;

namespace Shanism.Network.Client
{
    /// <summary>
    /// Holds all GameObjects sent by the server. 
    /// </summary>
    public class ObjectCache
    {
        static readonly EntityMapper mapper = new EntityMapper();

        //grows to infinity...
        readonly Dictionary<uint, ObjectStub> _objectCache = new Dictionary<uint, ObjectStub>();

        readonly HashSet<EntityStub> _visibleEntities = new HashSet<EntityStub>();


        public IReadOnlyCollection<EntityStub> VisibleEntities => _visibleEntities;

        public uint CurrentFrame { get; set; }


        public void ReadFrame(NetBuffer msg)
        {
            // if a diff from a previous frame, ignore
            var lastFrameId = msg.ReadUInt32();
            var curFrameId = msg.ReadUInt32();


            if (lastFrameId < CurrentFrame)
                return;

            //read frame ID
            CurrentFrame = curFrameId;

            //read vis change mask
            //sync in-game objects with server diff
            var pages = new PageReader(msg);
            foreach (var id in pages.VisibleGuids)
            {
                if (_objectCache.ContainsKey(id))
                    _objectCache.Remove(id);
                else
                    _objectCache.Add(id, null);
            }

            //read object diffs
            var fr = new FieldReader(msg);
            foreach (var kvp in _objectCache.OrderBy(kvp => kvp.Key))
            {
                var curObj = kvp.Value;
                var curObjType = (ObjectType)fr.ReadByte(0);
                if (curObj == null || curObj.ObjectType != curObjType)
                    _objectCache[kvp.Key] = curObj = mapper.Create(curObjType, kvp.Key);

                mapper.Read(curObj, fr);
            }
        }
    }
}