using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Message.Network;
using Shanism.Common.StubObjects;
using System.IO;
using Shanism.Common.Serialization;
using Shanism.Network.Common;

namespace Shanism.Network.Client
{
    /// <summary>
    /// Holds all GameObjects sent by the server. 
    /// </summary>
    class ObjectCache
    {
        //grows to infinity...
        readonly Dictionary<uint, ObjectStub> _objectCache = new Dictionary<uint, ObjectStub>();

        readonly HashSet<EntityStub> _visibleEntities = new HashSet<EntityStub>();


        public IReadOnlyCollection<EntityStub> VisibleEntities => _visibleEntities;


        internal void ReadServerFrame(ServerFrameBuilder serializer, GameFrameMessage msg)
        {
            _visibleEntities.Clear();
            serializer.Read(msg, _objectCache, _visibleEntities);
        }

        public void ReadDiff(GameFrameMessage msg)
        {
            var diff = msg.Data;

            using(var ms = new MemoryStream(diff))
            using(var r = new BinaryReader(ms))
            {
                //read frame ID
                var newFrameId = r.ReadUInt32();

                //read vis change mask
                var pages = new PageReader(r);
                foreach(var id in pages.ChangedIds)
                {
                    if(_objectCache.ContainsKey(id))
                        _objectCache.Remove(id);
                    else
                        _objectCache.Add(id, null);
                }

                //read object diffs
                foreach(var kvp in _objectCache.OrderBy(kvp => kvp.Key))
                {

                }
            }
        }
    }
}
