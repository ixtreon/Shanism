using IO.Common;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Message.Server
{
    [ProtoContract]
    public class PlayerStatusMessage : IOMessage
    {
        [ProtoMember(1)]
        public readonly int HeroId = -1;

        [ProtoMember(2)]
        public readonly Vector CameraPosition;

        private PlayerStatusMessage()
            : base(MessageType.PlayerStatusUpdate)
        { }

        /// <summary>
        /// Informs the client of the id of its hero. 
        /// </summary>
        /// <param name="hero"></param>
        public PlayerStatusMessage(IHero hero)
            : this()
        {
            HeroId = hero.Guid;
            CameraPosition = hero.Location;
        }

        /// <summary>
        /// Informs the client that it is currently observing. 
        /// </summary>
        /// <param name="cameraPos"></param>
        public PlayerStatusMessage(Vector cameraPos)
            : this()
        {
            CameraPosition = cameraPos;
        }
    }
}
