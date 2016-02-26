using IO;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Network
{
    class NetworkSystem : GameSystem
    {
        public bool IsOnline { get; private set; } = false;

        public NServer Server { get; private set; }


        /// <summary>
        /// Starts the underlying network server
        /// passing the given game engine to it. 
        /// </summary>
        /// <param name="engine"></param>
        public void Start(IShanoEngine engine)
        {
            if (IsOnline)
            {
                Console.WriteLine("Trying to open the server for network play but it is already online!");
                return;
            }

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
