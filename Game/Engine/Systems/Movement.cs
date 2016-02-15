using Engine.Entities;
using IO;
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
            var suggestedPos = Owner.Position.PolarProjection(angle, dist);

            if (Owner.HasState(UnitFlags.NoCollision))
                return suggestedPos;

            //fix terrain
            var startP = (suggestedPos - Owner.Scale).Floor();
            var endP = (suggestedPos + Owner.Scale).Ceiling();

            foreach(var p in startP.IterateToInclusive(endP))
            {
                if (isTileOk(Owner.Terrain.GetTerrain(p), Owner.CanFly, Owner.CanSwim, Owner.CanWalk))
                    continue;

                var closestPoint = suggestedPos.Clamp(p, p + Point.One);
                var distSq = suggestedPos.DistanceToSquared(closestPoint);

                var minDist = Owner.Scale / 2;
                if (distSq < minDist * minDist)
                {
                    var ang = closestPoint.AngleTo(suggestedPos);
                    suggestedPos = closestPoint.PolarProjection(ang, minDist);
                }
            }

            if (!isTileOk(Owner.Terrain.GetTerrain(suggestedPos), Owner.CanFly, Owner.CanSwim, Owner.CanWalk))
                suggestedPos = Owner.Position;

            //fix objects
            var nearbyObjects = Owner.Map
                .GetObjectsInRange(Owner.Position, dist + Constants.Units.MaxCollisionSize)
                .Where(u => u != Owner && u.HasCollision)
                .ToList();

            foreach(var obj in nearbyObjects)
            {
                var minDist = (obj.Scale + Owner.Scale) / 2;
                if (obj.Position.DistanceToSquared(suggestedPos) < minDist * minDist)
                {
                    var ang = obj.Position.AngleTo(suggestedPos);
                    suggestedPos = obj.Position.PolarProjection(ang, minDist);
                }
            }

            return suggestedPos;
        }

        static bool resolveTile(Vector ourPos, double ourScale, Point terrainPos, TerrainType tt, bool canFly, bool canSwim, bool canWalk)
        {
            //var tt = owner.Terrain.GetTerrain(pos.Floor());

            //see if we are close enough to this terrain tile
            var closestPoint = ourPos.Clamp(terrainPos, Point.One);
            var distSq = ourPos.DistanceToSquared(closestPoint);

            if (distSq > ourScale * ourScale)
                return true;

            return isTileOk(tt, canFly, canSwim, canWalk);
        }
        
        static bool isTileOk(TerrainType tt, bool canFly, bool canSwim, bool canWalk)
        {
            //no map, no walk
            if (tt == TerrainType.None)
                return false;

            if (canFly)
                return true;

            //water is cool if swim
            if (tt == TerrainType.Water || tt == TerrainType.DeepWater)
                return canSwim;

            //otherwise should walk..
            return canWalk;
        }
    }
}
