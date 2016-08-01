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
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Network.Client
{
    /// <summary>
    /// Holds all GameObjects sent by the server and performs RangeQueries for the client. 
    /// </summary>
    class ObjectCache
    {
        //grows to infinity...
        readonly Dictionary<uint, ObjectStub> _objectCache = new Dictionary<uint, ObjectStub>();

        public HashSet<EntityStub> VisibleEntities { get; } = new HashSet<EntityStub>();

        internal void ReadServerFrame(ClientSerializer serializer, GameFrameMessage msg)
        {
            VisibleEntities.Clear();
            serializer.ReadServerFrame(msg, _objectCache, VisibleEntities);
        }
    }
}
