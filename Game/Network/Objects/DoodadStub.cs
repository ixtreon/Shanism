using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Objects
{
    class DoodadStub : ObjectStub, IDoodad
    {
        public DoodadStub(int guid)
            : base(guid)
        {

        }
    }
}
