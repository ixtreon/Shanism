using IO.Common;
using IxSerializer.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Message.Server
{
    [SerialKiller]
    public class PlayerStatusMessage : IOMessage
    {
        public override MessageType Type
        {
            get { return MessageType.PlayerStatusUpdate; }
        }

        [SerialMember]
        public readonly int HeroId = -1;

        [SerialMember]
        public readonly Vector CameraPosition;

        private PlayerStatusMessage() { }

        /// <summary>
        /// Informs the client of the id of its hero. 
        /// </summary>
        /// <param name="hero"></param>
        public PlayerStatusMessage(IHero hero)
        {
            HeroId = hero.Guid;
            CameraPosition = hero.Position;
        }

        /// <summary>
        /// Informs the client that it is currently observing. 
        /// </summary>
        /// <param name="cameraPos"></param>
        public PlayerStatusMessage(Vector cameraPos)
        {
            CameraPosition = cameraPos;
        }
    }
}
