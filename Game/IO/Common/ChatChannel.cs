using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Common
{
    public class ChatChannel
    {
        public static readonly ChatChannel Say = new ChatChannel("Say");
        public static readonly ChatChannel Party = new ChatChannel("Party");
        public static readonly ChatChannel Faction = new ChatChannel("Faction");

        public ChatChannel(string name)
        {

        }
    }
}
