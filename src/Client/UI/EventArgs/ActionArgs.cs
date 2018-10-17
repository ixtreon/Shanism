using System;

namespace Shanism.Client.UI
{
    /// <summary>
    /// The arguments raised whenever the player completes an in-game action. 
    /// Usually a combination of one or more keys.
    /// </summary>
    public class ClientActionArgs : EventArgs
    {
        public ClientAction Action { get; }

        public ClientActionArgs(ClientAction action) 
        {
            Action = action;
        }
    }
}
