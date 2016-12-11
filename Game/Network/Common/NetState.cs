using Shanism.Common.Game;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.Serialization;
using Shanism.Common.StubObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Network.Common
{
    /// <summary>
    /// Client's objects' state as seen by the network module.
    /// </summary>
    class NetState
    {
        public uint LastAck;

        public Dictionary<uint, ObjectStub> VisibleObjects;


        public Dictionary<uint, byte[]> Diffs;

        /// <summary>
        /// Writes a new diff frame.
        /// </summary>
        public byte[] WriteDiff(uint curFrameId, List<IGameObject> objects)
        {
            //get vis. change mask

            using(var ms = new MemoryStream())
            {
                using(var wr = new BinaryWriter(ms))
                {
                    //write frame ID
                    wr.Write(curFrameId);

                    //write vis. change mask
                    var pages = new PageBuilder();
                    pages.MakePages(VisibleObjects, objects);
                    pages.Write(wr);

                    //write object diffs
                    var fw = new FieldWriter(wr);
                    foreach(var obj in objects)
                    {
                        ObjectStub oldObject;
                        if(!VisibleObjects.TryGetValue(obj.Id, out oldObject))
                            oldObject = ObjectStub.GetDefaultObject(obj.ObjectType);

                        oldObject.WriteDiff(fw, obj);
                    }
                }

                return ms.ToArray();
            }
        }

        public void SetLastAck(uint ack)
        {
            if(LastAck == ack)
                return;

            LastAck = ack;

            var diff = Diffs[ack];
            Diffs.Clear();

            //update all objects in VisibleObjects, recreating them if necessary

            throw new NotImplementedException();
        }

    }
}
