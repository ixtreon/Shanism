using Shanism.Common.Game;
using Shanism.Common.Objects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Message.Server
{
    [ProtoContract]
    public class PlayerStatusMessage : IOMessage
    {
        
        [ProtoMember(1)]
        public readonly uint HeroId = 0;


        public override MessageType Type { get { return MessageType.PlayerStatusUpdate; } }

        PlayerStatusMessage() { }

        /// <summary>
        /// Informs the client of the id of its hero. 
        /// </summary>
        /// <param name="heroId">The unique ID of the hero. </param>
        public PlayerStatusMessage(uint heroId)
            : this()
        {
            HeroId = heroId;
        }
    }
}
