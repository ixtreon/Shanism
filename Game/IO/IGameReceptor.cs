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
    /// Represents an interface to a (local or remote) game server as seen from the game client.  
    /// Supports updating movement state, registering special actions (abilities), 
    /// as well as providing the client with information about the game. 
    /// </summary>
    public interface IGameReceptor
    {
        /// <summary>
        /// Gets whether has a connected to the server. 
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



        /// <summary>
        /// Gets all game objects in range of the main hero. 
        /// </summary>
        IEnumerable<IGameObject> GetNearbyGameObjects();

        /// <summary>
        /// Requests the specified chunk from the server. 
        /// </summary>
        /// <param name="chunk"></param>
        void RequestChunk(MapChunkId chunk);

        /// <summary>
        /// Sends the provided action to the server. 
        /// </summary>
        /// <param name="p"></param>
        void RegisterAction(ActionMessage p);

        /// <summary>
        /// The event raised whenever a chunk is received. 
        /// </summary>
        event Action<MapChunkId, TerrainType[,]> ChunkReceived;
    }
}
 