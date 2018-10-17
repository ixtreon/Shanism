using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Network
{
    enum NetMessageHeader : byte
    {
        ProtoMessage = 0,
        GameFrame = 1,
    }
}
