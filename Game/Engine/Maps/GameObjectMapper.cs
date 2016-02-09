using Engine.Entities;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Maps
{
    /// <summary>
    /// Maps instances of <see cref="GameObject"/> to bins of type <see cref="Point"/>. 
    /// Also provides wrappers around <see cref="ScenarioObject.Update(int)"/> and <see cref="GameObject.IsDestroyed"/> to the map provider. 
    /// </summary>
    class GameObjectMapper : IObjectMapper<GameObject, Point>
    {
        double binSize { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObjectMapper"/> class.
        /// </summary>
        /// <param name="binSize">The size of each bin. </param>
        public GameObjectMapper(double binSize)
        {
            this.binSize = binSize;
        }

        public Point GetBinId(Vector pos)
        {
            return (pos / binSize).Floor();
        }

        /// <summary>
        /// Returns the id of the bin an object belongs to.
        /// </summary>
        public Point GetBinId(GameObject obj)
        {
            return GetBinId(obj.Position);
        }

        /// <summary>
        /// Gets whether an object should be removed from the map as soon as possible.
        /// </summary>
        public bool ShouldRemove(GameObject obj)
        {
            return obj.IsDestroyed;
        }

        /// <summary>
        /// Updates the state of an object based on the time elapsed since the last update.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="msElapsed">The time elapsed since the last update, in milliseconds.</param>
        public void Update(GameObject obj, int msElapsed)
        {
            obj.Update(msElapsed);
        }


        /// <summary>
        /// Returns whether this object keeps a bin alive by forcing it to update.
        /// If all objects in a chunk return false, the chunk is marked as inactive (NYI).
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool ForcesBinUpdates(GameObject obj)
        {
            return true;    // NYI
        }
    }
}
