using IO.Common;
using IO.Objects;
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
        public readonly uint HeroId = 0;


        PlayerStatusMessage() { Type = MessageType.PlayerStatusUpdate; }

        /// <summary>
        /// Informs the client of the id of its hero. 
        /// </summary>
        /// <param name="hero"></param>
        public PlayerStatusMessage(uint heroId)
            : this()
        {
            HeroId = heroId;
        }
    }
}
