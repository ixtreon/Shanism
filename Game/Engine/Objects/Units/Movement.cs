using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Objects
{
    partial class Unit
    {

        /// <summary>
        /// Gets the current movement speed of the unit. 
        /// </summary>
        public double MoveSpeed { get; protected internal set; }

        /// <summary>
        /// Gets the walking speed of the unit, which is the running speed divided by 3. 
        /// </summary>
        public double WalkSpeed { get { return MoveSpeed / 3; } }


        /// <summary>
        /// Gets whether this guy can walk on any terrain 
        /// without collision. 
        /// </summary>
        public bool CanFly { get; set; } = false;

        /// <summary>
        /// Gets whether this guy can walk on water. 
        /// </summary>
        public bool CanSwim { get; set; } = false;

        /// <summary>
        /// Gets whether this guy can walk on dense terrain. 
        /// </summary>
        public bool CanWalk { get; set; } = true;
        

        /// <summary>
        /// Gets whether the unit can step on the kind of terrain at the given point. 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool CanStep(Vector v)
        {
            var tt = Terrain.GetTerrainAt(v.Floor());

            //no map, no walk
            if (tt == TerrainType.None)
                return false;

            //otherwise cool if flying/swimming
            if (CanFly || CanSwim)
                return true;

            return tt != TerrainType.Water && tt != TerrainType.DeepWater;
        }


        /// <summary>
        /// Causes the unit to move in the given direction. Works only once per update frame 
        /// and only if the unit has not moved during this frame already. 
        /// 
        /// This is the official way to make units walk around. 
        /// </summary>
        /// <param name="msElapsed"></param>
        /// <param name="direction"></param>
        /// <param name="maxDistance"></param>
        /// <param name="walk">Whether to walk instead of running. </param>
        internal void Move(double msElapsed, double direction, double maxDistance = double.MaxValue, bool walk = false)
        {
            //if we have already moved or teleported this turn, return
            if (Position != FuturePosition)
                return;

            //get the distance, final position
            var speed = walk ? WalkSpeed : MoveSpeed;
            var moveDist = Math.Min(maxDistance, speed * msElapsed / 1000);
            var pos = Position.PolarProjection(direction, moveDist);

            if (!CanStep(pos))
                return;

            //finally move us there
            setLocation(pos, true);
        }


    }
}
