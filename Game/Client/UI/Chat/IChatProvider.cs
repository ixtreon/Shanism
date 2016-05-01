using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.Chat
{
    interface IChatProvider
    {
        event Action<string> MessageSent;
    }
}
