using Engine.Objects;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Events
{
    /// <summary>
    /// The event raised whenever a unit moves. 
    /// </summary>
    public class ObjectMoveArgs
    {
        public readonly Entity MovingUnit;

        public readonly Vector OldLocation;

        public readonly Vector NewLocation;

        internal ObjectMoveArgs(Entity obj, Vector oldLocation, Vector newLocation)
        {
            MovingUnit = obj;
            OldLocation = oldLocation;
            NewLocation = newLocation;
        }
    }
}
