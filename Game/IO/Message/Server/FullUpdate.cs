using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Message.Server
{
    public class FullUpdateMessage : IOMessage
    {
        public readonly IGameObject kur;

        public FullUpdateMessage()
            : base(MessageType.FullUpdate)
        {

        }
    }
}
