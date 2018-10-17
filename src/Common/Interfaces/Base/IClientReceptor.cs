using Shanism.Common.Messages;

namespace Shanism.Common
{
    /// <summary>
    /// Issued by the game client to organize all communications with the server.
    /// </summary>
    public interface IClientReceptor
    {
        /// <summary>
        /// Gets the name of the client. 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the current state of the client.
        /// </summary>
        PlayerState PlayerState { get; }

        /// <summary>
        /// Sends the specified message to the client.
        /// </summary>
        void HandleMessage(ServerMessage msg);

    }
}
