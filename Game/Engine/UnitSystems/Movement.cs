using Shanism.Engine.Entities;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Systems
{
    /// <summary>
    /// Updates the unit's current movementstate. See <see cref="Unit.MovementState"/>.
    /// </summary>
    class MovementSystem : UnitSystem
    {
        const string MoveAnimation = Shanism.Common.Constants.Animations.Move;


        readonly Unit Owner;

        readonly List<Entity> nearbies = new List<Entity>();

        public MovementSystem(Unit owner)
        {
            Owner = owner;
        }

        public override void Update(int msElapsed)
        {
            var state = Owner.MovementState;
            if (Owner.IsStunned || !Owner.CanMove || !state.IsMoving)
                return;
            

            //get the suggested move position
                var speed = Owner.MoveSpeed;
            var dist = speed * msElapsed / 1000;
            if (state.HasMaxDistance)
                dist = Math.Min(state.MaxDistance, dist);

            //fix collision
            var finalPos = resolveStep(state.MoveDirection, dist);

            //only continue if we moved at least a bit.
            var finalDist = (float)finalPos.DistanceTo(Owner.Position);
            var hasMovedMuch = finalDist / dist > 0.1;

            if (hasMovedMuch)
            {
                Owner.Position = finalPos;
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

            if (Owner.StateFlags.HasFlag(StateFlags.NoCollision))
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
