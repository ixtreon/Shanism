using Shanism.Common.Interfaces.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Network.Diff
{
    class DiffGuy
    {

        public void Write(BinaryWriter writer, IEnumerable<IGameObject> visibleObjects, int lastKnownId, int newId)
        {

            //first get the snapshotId of each of the visibleObjects:
            //lastKnownId if they were in the set, 0 otherwise.

            //go thru each object checking which properties changed
        }

        public void Read(BinaryReader reader, ICollection<IGameObject> visibleObjects)
        {

        }


        void write(BinaryWriter writer, BinaryReader oldVersion, IGameObject obj)
        {

        }

        IGameObject getObjectSnapshot(int snapshotId)
        {
            throw new NotImplementedException();
        }
    }
}
