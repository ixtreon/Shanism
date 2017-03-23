using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI.Chat
{
    interface IChatSource
    {
        event Action<string> ChatMessageSent;
    }
}
