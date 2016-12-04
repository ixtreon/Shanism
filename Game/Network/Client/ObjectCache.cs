using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Message.Network;
using Shanism.Common.StubObjects;
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
    }
}
