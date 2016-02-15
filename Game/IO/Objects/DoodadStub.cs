using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Objects
{
    /// <summary>
    /// Represents an empty doodad as reconstructed by a network client. 
    /// </summary>
    public class DoodadStub : ObjectStub, IDoodad
    {
        public DoodadStub() : base(0) { }

        public DoodadStub(uint guid)
            : base(guid)
        {

        }
    }
}
