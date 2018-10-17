using Shanism.Common;
using Shanism.Engine.Models.Systems;
using Shanism.Network;

namespace Shanism.Engine.Systems
{
    class NetworkSystem : GameSystem
    {
        public override string Name => "Network";
        public bool IsOnline { get; private set; }

        public NServer Server { get; private set; }


        /// <summary>
        /// Restarts the underlying network server
        /// passing the given game engine to it. 
        /// </summary>
        /// <param name="engine"></param>
        public void Restart(IEngine engine)
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
