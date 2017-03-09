using Shanism.Common;
using Shanism.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Network
{
    class NetworkSystem : GameSystem
    {
        public override string SystemName => "Network";
        public bool IsOnline { get; private set; }

        public NServer Server { get; private set; }


        /// <summary>
        /// Restarts the underlying network server
        /// passing the given game engine to it. 
        /// </summary>
        /// <param name="engine"></param>
        public void Restart(IShanoEngine engine)
        {
            IsOnline = true;
            Server = new NServer(engine);
        }

        internal override void Update(int msElapsed)
        {
            if (!IsOnline) return;

            Server.Update(msElapsed);
        }
    }
}
