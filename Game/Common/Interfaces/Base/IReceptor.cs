using Shanism.Common.Game;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    /// <summary>
    /// A game receptor as exposed by a <see cref="IShanoEngine"/> to all clients. 
    /// 
    /// Event based for simplicity.  
    /// </summary>
    public interface IReceptor
    {
        /// <summary>
        /// Gets the unique identifier of this player.
        /// </summary>
        uint Id { get; }

        /// <summary>
        /// Gets the name of the player.
        /// </summary>
        string Name { get; }

        //ConnectionState State { get; }

        /// <summary>
        /// The event raised whenever the server sends a message to the player. 
        /// </summary>
        event Action<IOMessage> MessageSent;

        /// <summary>
        /// Returns a string with data useful for debugging.
        /// </summary>
        string GetDebugString();

        IReadOnlyCollection<IEntity> VisibleEntities { get; }
    }
}
