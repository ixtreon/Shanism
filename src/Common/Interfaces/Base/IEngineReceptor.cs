using Shanism.Common.Entities;
using Shanism.Common.Messages;
using System.Collections.Generic;

namespace Shanism.Common
{
    /// <summary>
    /// Issued by the server to a specific client
    /// to handle communication between the two parties.
    /// </summary>
    public interface IEngineReceptor
    {
        /// <summary>
        /// Gets the unique identifier of this player.
        /// </summary>
        uint PlayerId { get; }

        /// <summary>
        /// Gets whether the player is the host.
        /// </summary>
        bool IsHost { get; }

        /// <summary>
        /// Gets all entities visible by the receptor.
        /// </summary>
        IReadOnlyCollection<IEntity> VisibleEntities { get; }

        /// <summary>
        /// Instructs the server to drop the current player.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Sends the given message to the server.
        /// </summary>
        void HandleMessage(ClientMessage msg);

        /// <summary>
        /// Returns a string with data useful for debugging.
        /// </summary>
        string GetDebugString();

        void StartPlaying();
    }
}
