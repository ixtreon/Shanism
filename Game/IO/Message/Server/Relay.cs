using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Message.Server
{
    /// <summary>
    /// Relays a message from another client. 
    /// </summary>
    class RelayMessage : IOMessage
    {
        public RelayMessage()
            : base(MessageType.Relay)
        {

        }
    }
}
