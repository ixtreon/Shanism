using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Commands;
using IO.Common;
using IO.Objects;

namespace IO
{
    /// <summary>
    /// Represents an interface to a (local or remote) game server as seen from the game client.  
    /// Supports updating movement state, registering special actions (abilities), 
    /// as well as providing the client with information about the game. 
    /// </summary>
    public interface IServer
    {
        /// <summary>
        /// Gets our hero. 
        /// </summary>
        IHero LocalHero { get; }

        /// <summary>
        /// Sets (or gets) the direction our hero is heading in. 
        /// </summary>
        MovementState MovementState { get; set; }


        void RegisterAction(ActionArgs p);


        void GetNearbyTiles(ref MapTile[,] tiles, out double x, out double y);

        /// <summary>
        /// Gets all entities in range of our hero. 
        /// </summary>
        IEnumerable<IUnit> GetUnits();

        IEnumerable<IGameObject> GetGameObjects();
    }
}
 