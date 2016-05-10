using Shanism.Common.Message;
using Shanism.Common.Message.Network;
using Shanism.Common.Message.Server;
using Shanism.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    /// <summary>
    /// A game receptor as exposed by the engine to the network module. 
    /// This type of receptor includes and supplements the functionality of an <see cref="IReceptor"/>. 
    /// </summary>
    public interface INetReceptor : IReceptor
    {
        GameFrameMessage GetCurrentFrame();
    }
}
