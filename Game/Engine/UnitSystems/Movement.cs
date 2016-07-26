using Shanism.Engine.Entities;
using Shanism.Common;
using Shanism.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Systems
{
    /// <summary>
    /// Provides a common way to order units moving around. 
    /// </summary>
    class MovementSystem : UnitSystem
    {
        const string MoveAnimation = Shanism.Common.Constants.Animations.Move;


        readonly Unit Owner;

        bool _isMoving;
        double _moveDirection;
        double _moveDistanceLeft;


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

            if (Owner.Animation == MoveAnimation)
                Owner.ResetAnimation();
        }

        ///
        internal void SetMovementState(double direction)
        {
            if (!(_isMoving 
                && _moveDirection.AlmostEqualTo(direction)))
            {
                _isMoving = true;
                _moveDirection = direction;
                _moveDistanceLeft = double.MaxValue;

                Owner.PlayAnimation(MoveAnimation, true);
            }
        }

        internal void SetMovementState(double direction, double maxDistance)
        {
            if (!(_isMoving
                && _moveDirection.AlmostEqualTo(direction)
                && maxDistance.AlmostEqualTo(_moveDistanceLeft)))
            {
                _isMoving = true;
                _moveDirection = direction;
                _moveDistanceLeft = maxDistance;

                Owner.PlayAnimation(MoveAnimation, true);
            }
        }

        public override void Update(int msElapsed)
        {
            if (!_isMoving || Owner.States.HasFlag(StateFlags.Stunned))
                return;

            //get the suggested move position
            var speed = Owner.MoveSpeed;
            var maxDist = speed * msElapsed / 1000;
            var suggestedDist = Math.Min(_moveDistanceLeft, maxDist);
            
            var newPos = resolveStep(Owner, _moveDirection, suggestedDist);
            var dist = newPos.DistanceTo(Owner.Position);
            var hasMovedMuch = dist / suggestedDist > 0.25;

            if (hasMovedMuch)
            {
                Owner.Position = newPos;
            }
            else
            {
                if (Owner.Animation == MoveAnimation)
                    Owner.ResetAnimation();
            }
        }

        static Vector resolveStep(Unit owner, double angle, double dist)
        {
            var suggestedPos = owner.Position.PolarProjection(angle, dist);

            if (owner.States.HasFlag(StateFlags.NoCollision))
                return suggestedPos;

            //fix terrain
            var startP = (suggestedPos - owner.Scale).Floor();
            var endP = (suggestedPos + owner.Scale).Ceiling();

            foreach(var terrainPt in startP.IterateToInclusive(endP))
            {
                var tty = owner.Map.Terrain.Get(terrainPt);
                if (isTileOk(tty, owner.CanFly, owner.CanSwim, owner.CanWalk))
                    continue;

                suggestedPos = fixTerrain(suggestedPos, terrainPt, owner.Scale);
            }

            if (!isTileOk(owner.Map.Terrain.Get(suggestedPos), owner.CanFly, owner.CanSwim, owner.CanWalk))
                suggestedPos = owner.Position;

            //fix objects
            var nearbyObjects = owner.Map
                .GetObjectsInRect(suggestedPos, new Vector((owner.Scale + Constants.Entities.MaxSize) / 2))
                .Where(u => u != owner 
                    && u.HasCollision);

            foreach(var obj in nearbyObjects)
            {
                suggestedPos = fixEntity(suggestedPos, obj, owner.Scale);
            }

            return suggestedPos;
        }

        static Vector fixEntity(Vector suggestedPos, Entity obj, double ourScale)
        {
            var minDist = (obj.Scale + ourScale) / 2;
            if (suggestedPos.DistanceTo(obj.Position) < minDist)
            {
                var ang = obj.Position.AngleTo(suggestedPos);
                suggestedPos = obj.Position.PolarProjection(ang, minDist);
            }

            return suggestedPos;
        }

        static Vector fixTerrain(Vector suggestedPos, Point terrainPt, double ourScale)
        {
            var closestPoint = suggestedPos.Clamp(terrainPt, terrainPt + Point.One);
            var distSq = suggestedPos.DistanceToSquared(closestPoint);

            var minDist = ourScale / 2;
            if (distSq < minDist * minDist)
            {
                var ang = closestPoint.AngleTo(suggestedPos);
                suggestedPos = closestPoint.PolarProjection(ang, minDist);
            }

            return suggestedPos;
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
