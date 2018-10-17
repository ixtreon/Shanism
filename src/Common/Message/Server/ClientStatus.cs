using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Messages
{
    /// <summary>
    /// The server updates a client about their state.
    /// </summary>
    [ProtoContract]
    public class PlayerStatus : ServerMessage
    {
        public override ServerMessageType Type => ServerMessageType.PlayerStatus;

        [ProtoMember(1)]
        public readonly uint HeroId;


        PlayerStatus() { }

        /// <summary>
        /// Informs the client of the id of its hero. 
        /// </summary>
        /// <param name="heroId">The unique ID of the hero. </param>
        public PlayerStatus(uint heroId)
            : this()
        {
            HeroId = heroId;
        }
    }
}
