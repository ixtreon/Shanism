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


        readonly List<Entity> nearbies = new List<Entity>();

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

            var newPos = resolveStep(_moveDirection, suggestedDist);
            var dist = newPos.DistanceTo(Owner.Position);
            var hasMovedMuch = dist / suggestedDist > 0.1;

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

        Vector resolveStep(double angle, double dist)
        {
            var suggestedPos = Owner.Position.PolarProjection(angle, dist);

            if (Owner.States.HasFlag(StateFlags.NoCollision))
                return suggestedPos;

            //fix terrain
            var r = Owner.Scale / 2;
            var sx = (int)Math.Floor(suggestedPos.X - r);
            var sy = (int)Math.Floor(suggestedPos.Y - r);
            var ex = (int)Math.Floor(suggestedPos.X + r);
            var ey = (int)Math.Floor(suggestedPos.Y + r);
            for (int ix = sx; ix <= ex; ix++)
                for (int iy = sy; iy <= ey; iy++)
                {
                    var p = new Point(ix, iy);
                    var tty = Owner.Map.Terrain.Get(p);
                    if (!isTileOk(tty, Owner))
                        suggestedPos = fixTerrain(suggestedPos, p, r);
                }

            if (!isTileOk(Owner.Map.Terrain.Get(suggestedPos), Owner))
                suggestedPos = Owner.Position;


            //fix objects
            nearbies.Clear();
            Owner.Map.GetObjectsInRect(
                suggestedPos, 
                new Vector((Owner.Scale + Constants.Entities.MaxSize) / 2),
                nearbies);

            foreach (var obj in nearbies)
                if(obj != Owner && obj.HasCollision)
                    suggestedPos = fixEntity(suggestedPos, obj, r);

            return suggestedPos;
        }

        static Vector fixEntity(Vector suggestedPos, Entity obj, double r)
        {
            var er = r + obj.Scale / 2;
            if (suggestedPos.DistanceToSquared(obj.Position) < er * er)
            {
                var ang = obj.Position.AngleTo(suggestedPos);
                suggestedPos = obj.Position.PolarProjection(ang, er);
            }

            return suggestedPos;
        }

        static Vector fixTerrain(Vector suggestedPos, Point terrainPt, double r)
        {
            var closestPoint = suggestedPos.Clamp(terrainPt, terrainPt + Point.One);
            var distSq = suggestedPos.DistanceToSquared(closestPoint);

            if (distSq < r * r)
            {
                var ang = closestPoint.AngleTo(suggestedPos);
                suggestedPos = closestPoint.PolarProjection(ang, r);
            }

            return suggestedPos;
        }

        static bool isTileOk(TerrainType tt, Unit u)
        {
            if (tt == TerrainType.None) //no map, no ok
                return false;

            if (u.CanFly)               //if u fly all is ok
                return true;

            if (tt == TerrainType.Water || tt == TerrainType.DeepWater)
                return u.CanSwim;       //water is cool if swim

            return true;    //otherwise can just sit there..
        }
    }
}
