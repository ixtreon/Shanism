using Lidgren.Network;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.StubObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Shanism.Network.Serialization;

namespace Shanism.Network.Server
{
    /// <summary>
    /// Client's objects' state as seen by the network module. 
    /// Contains last ack, as well as all diffs for the last ack. 
    /// </summary>
    public class ClientStateTracker
    {
        public uint LastAck { get; private set; }

        /// <summary>
        /// The visible objects corresponding to the last client ACK
        /// </summary>
        readonly Dictionary<uint, ObjectStub> VisibleObjects = new Dictionary<uint, ObjectStub>();


        readonly Dictionary<uint, NetBuffer> Diffs;

        /// <summary>
        /// Writes a new game frame message.
        /// </summary>
        public void WriteFrame(NetBuffer msg, uint curFrameId, IReadOnlyCollection<IGameObject> objects)
        {
            //write previous + current frames ID
            msg.Write(LastAck);
            msg.Write(curFrameId);

            //write vis. change mask
            var pages = new PageBuilder();
            pages.UpdatePages(objects);
            pages.Write(msg);

            //write object diffs
            var fw = new FieldWriter(msg);
            foreach (var obj in objects)
            {
                //get last object state
                ObjectStub oldObject;
                if (!VisibleObjects.TryGetValue(obj.Id, out oldObject))
                    oldObject = ObjectStub.GetDefaultObject(obj.ObjectType);

                oldObject.WriteDiff(fw, obj);
            }

            //save diff with cur state
            Diffs.Add(curFrameId, msg);

        }

        public void SetLastAck(uint ack)
        {
            if (LastAck == ack)
                return;

            LastAck = ack;

            var diff = Diffs[ack];
            Diffs.Clear();

            //update all objects in VisibleObjects, recreating them if necessary
            throw new NotImplementedException();
        }

    }
}
