using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Serialization
{
    public class PageReader
    {
        /// <summary>
        /// The sorted list of IDs that changed
        /// </summary>
        public List<uint> ChangedIds { get; } = new List<uint>();

        public PageReader(BinaryReader r)
        {
            throw new NotImplementedException();
        }
    }
}
