using Shanism.Engine.Entities;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Maps;

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

            //check terrain
            var terrainOk = checkTerrain(Owner.Map.Terrain, 
                new Ellipse(suggestedPos, Owner.Scale / 2),
                Owner.MovementType);

            if (!terrainOk)
                return Owner.Position;

            //if (!isTileOk(Owner.Map.Terrain.Get(suggestedPos), Owner))
            //    suggestedPos = Owner.Position;


            //fix objects
            if (!Owner.CanFly)
            {
                nearbies.Clear();
                Owner.Map.GetObjectsInRect(
                    suggestedPos,
                    new Vector((Owner.Scale + Constants.Entities.MaxSize) / 2),
                    nearbies);

                foreach (var obj in nearbies)
                    if (obj != Owner 
                        && obj.HasCollision 
                        && obj.Position.DistanceTo(suggestedPos) * 2 <= (Owner.Scale + obj.Scale))
                        return Owner.Position;
            }

            return suggestedPos;
        }

        static bool checkTerrain(ITerrainMap terrain, Ellipse e, MovementFlags flags)
        {
            var p1 = (e.Center - e.Radius).Floor();
            var p2 = (e.Center + e.Radius).Floor();

            for (int ix = p1.X; ix <= p2.X; ix++)
                for (int iy = p1.Y; iy <= p2.Y; iy++)
                {
                    var px = ix < e.Center.X ? ix + 1 : ix;
                    var py = iy < e.Center.Y ? iy + 1 : iy;
                    if (e.IsInside(new Vector(px, py)))
                    {
                        var p = new Point(ix, iy);
                        var tty = terrain.Get(p);
                        if (!isTileOk(tty, flags))
                            return false;
                    }
                }

            return true;
        }

        //static Vector fixEntity(Vector suggestedPos, Entity obj, double r)
        //{
        //    var er = r + obj.Scale / 2;
        //    if (suggestedPos.DistanceToSquared(obj.Position) < er * er)
        //    {
        //        var ang = obj.Position.AngleTo(suggestedPos);
        //        suggestedPos = obj.Position.PolarProjection(ang, er);
        //    }

        //    return suggestedPos;
        //}

        //static Vector fixTerrain(Vector suggestedPos, Point terrainPt, double r)
        //{
        //    var closestPoint = suggestedPos.Clamp(terrainPt, terrainPt + Point.One);
        //    var distSq = suggestedPos.DistanceToSquared(closestPoint);

        //    if (distSq < r * r)
        //    {
        //        var ang = closestPoint.AngleTo(suggestedPos);
        //        suggestedPos = closestPoint.PolarProjection(ang, r);
        //    }

        //    return suggestedPos;
        //}

        static bool isTileOk(TerrainType tt, MovementFlags moveType)
        {
            //no map
            if (tt == TerrainType.None)
                return false;

            //flying
            if (moveType == MovementFlags.All)
                return true;

            // water
            if (tt == TerrainType.Water || tt == TerrainType.DeepWater)
                return (moveType & MovementFlags.Water) == MovementFlags.Water;

            // ground
            return true;    //otherwise can just sit there..
        }
    }
}
