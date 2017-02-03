﻿using Lidgren.Network;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.StubObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Shanism.Network.Serialization;
using Shanism.Network.Common;

namespace Shanism.Network.Server
{
    /// <summary>
    /// Client's objects' state as seen by the network module. 
    /// Contains last ack, as well as all diffs for the last ack. 
    /// </summary>
    public class ClientStateTracker : ObjectCache
    {
        readonly Dictionary<uint, byte[]> Diffs = new Dictionary<uint, byte[]>();

        readonly PageWriter pages = new PageWriter();


        public ClientStateTracker(long bufferStart)
            : base(bufferStart)
        {

        }


        /// <summary>
        /// Writes a new game frame message.
        /// </summary>
        public void WriteFrame(NetBuffer msg, uint newFrame, 
            IReadOnlyCollection<IGameObject> objects)
        {
            if (CurrentFrame >= newFrame)
                throw new InvalidOperationException($"Writing frame {newFrame}");

            //write previous + current frames ID
            msg.Write(CurrentFrame);
            msg.Write(newFrame);
            

            //write vis. change mask
            pages.UpdatePages(Cache, objects);
            pages.Write(msg);

            //write object diffs
            var fw = new FieldWriter(msg);
            foreach (var obj in objects.OrderBy(o => o.Id))
            {
                //get last object state
                ObjectStub oldObject;
                if (!Cache.TryGetValue(obj.Id, out oldObject)
                    || oldObject.ObjectType != obj.ObjectType)
                    oldObject = Mapper.GetDefault(obj.ObjectType);

                fw.WriteByte(0, (byte)obj.ObjectType);
                Mapper.Write(oldObject, obj, fw);
            }

            //save diff with cur state
            Diffs.Add(newFrame, msg.Data.ToArray());
        }


        public void SetLastAck(uint ack)
        {
            if (CurrentFrame == ack)
                return;


            var diff = Diffs[ack];
            Diffs.Clear();

            //update all objects in VisibleObjects, recreating them if necessary
            ReadFrame(new NetBuffer { Data = diff, LengthBytes = diff.Length });
        }

    }
}
