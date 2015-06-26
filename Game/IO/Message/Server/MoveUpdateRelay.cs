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
    class RelayMovementMessage : IOMessage
    {
        public OrderType ty = OrderType.Attack;

        private RelayMovementMessage() {  }

        public RelayMovementMessage(OrderType ord)
        {

        }
    }
}
