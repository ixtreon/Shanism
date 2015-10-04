using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;
using IO.Objects;
using IO.Message.Client;
using IO.Message.Server;

namespace IO
{
    /// <summary>
    /// A (local or remote) game server as visible by the game client.  
    /// Supports updating movement state, registering special actions (abilities), 
    /// as well as providing the client with information about the game. 
    /// </summary>
    public interface IGameReceptor
    {
        /// <summary>
        /// Gets whether we are connected to the server. 
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// Gets whether we have a hero. 
        /// </summary>
        bool HasHero { get; }
        
        /// <summary>
        /// Gets the current camera position. 
        /// </summary>
        Vector CameraPosition { get; }

        /// <summary>
        /// Gets our hero. Undefined unless <see cref="HasHero"/> is true. 
        /// </summary>
        IHero MainHero { get; }

        /// <summary>
        /// Sets the direction our hero is heading in. 
        /// </summary>
        MovementState MovementState { set; }


        IEnumerable<IGameObject> VisibleObjects { get; }

        /// <summary>
        /// Requests the specified chunk from the server. 
        /// </summary>
        /// <param name="chunk"></param>
        void RequestChunk(MapChunkId chunk);

        /// <summary>
        /// Registers the given action with the server. 
        /// </summary>
        /// <param name="p"></param>
        void RegisterAction(ActionMessage p);

        /// <summary>
        /// Causes the server object to update.  
        /// </summary>
        void Update(int msElapsed);

        /// <summary>
        /// The event raised whenever a new chunk is received. 
        /// </summary>
        event Action<MapChunkId, TerrainType[,]> ChunkReceived;
    }
}
 