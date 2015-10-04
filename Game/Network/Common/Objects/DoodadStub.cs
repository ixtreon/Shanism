using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Objects
{
    /// <summary>
    /// Represents an empty doodad as reconstructed by a network client. 
    /// </summary>
    class DoodadStub : ObjectStub, IDoodad
    {
        public DoodadStub() : base(-1) { }

        public DoodadStub(int guid)
            : base(guid)
        {

        }
    }
}
