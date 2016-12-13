using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Network.Serialization
{
    public class PageReader
    {
        /// <summary>
        /// The sorted list of IDs that changed
        /// </summary>
        public List<uint> VisibleGuids { get; } = new List<uint>();

        public PageReader()
        {

        }

        public PageReader(NetIncomingMessage r)
        {
            throw new NotImplementedException();
        }
    }
}
