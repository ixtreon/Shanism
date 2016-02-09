using Engine.Entities;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Systems
{
    /// <summary>
    /// Provides a common way to order units moving around. 
    /// </summary>
    class MovementSystem : UnitSystem
    {
        bool _isMoving;
        double _moveDirection;
        double _moveDistanceLeft;
        bool _moveSlowly;

        readonly Unit Owner;

        public MovementSystem(Unit owner)
        {
            Owner = owner;
        }

        /// <summary>
        /// Causes the unit to stop. 
        /// </summary>
        internal void Stop()
        {
            _isMoving = false;
        }

        ///
        internal void SetMovementState(double direction, double maxDistance = double.MaxValue, bool moveSlow = false)
        {
            _isMoving = true;
            _moveDirection = direction;
            _moveDistanceLeft = maxDistance;
            _moveSlowly = moveSlow;
        }

        internal override void Update(int msElapsed)
        {
            if (!_isMoving) return;

            //get the suggested move position
            var speed = _moveSlowly ? Owner.WalkSpeed : Owner.MoveSpeed;
            var maxDist = speed * msElapsed / 1000;
            var suggestedDist = Math.Min(_moveDistanceLeft, maxDist);
            
            Owner.Position = resolveStep(_moveDirection, suggestedDist);
        }

        Vector resolveStep(double angle, double dist, int nTries = 5)
        {
            do
            {
                var suggestedPos = Owner.Position.PolarProjection(angle, dist);
                if (CanStep(suggestedPos))
                    return suggestedPos;

                dist /= 2;
            }
            while (--nTries > 0);


            return Owner.Position;
        }

        public bool CanStep(Vector newPos)
        {
            var tt = Owner.Terrain.GetTerrain(newPos.Floor());

            //no map, no walk
            if (tt == TerrainType.None)
                return false;

            //water is cool if swim/fly
            if (tt == TerrainType.Water || tt == TerrainType.DeepWater)
                return Owner.CanFly || Owner.CanSwim;

            //otherwise should walk/fly..
            return Owner.CanFly || Owner.CanWalk;
        }
    }
}
