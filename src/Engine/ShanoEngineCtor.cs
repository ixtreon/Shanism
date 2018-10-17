using Shanism.Common;
using Shanism.Engine.Systems;
using Shanism.Network.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Shanism.Engine
{
    partial class ShanoEngine
    {
        public static IEngine ConnectTo(string hostAddress)
            => new NetworkEngine(hostAddress);
    }
}
