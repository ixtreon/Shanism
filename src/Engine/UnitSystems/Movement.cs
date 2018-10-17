using Ix.Math;
using Shanism.Common;
using Shanism.Engine.Entities;
using System;
using System.Numerics;

namespace Shanism.Engine.Systems
{
    // TODO: this needs a rewrite

    /// <summary>
    /// Updates the unit's current movementstate. See <see cref="Unit.MovementState"/>.
    /// </summary>
    class MovementSystem : UnitSystem
    {

        bool hasCollision()
            => Type != MovementType.Flying
            && (Owner.StateFlags & StateFlags.NoCollision) == 0;

        bool canMoveTo(TerrainType tty)
            => tty.IsGround()
            || (tty.IsWater() && Type.CanSwim());


        public MovementType Type { get; set; } = MovementType.Walking;

        readonly Unit Owner;

        public MovementSystem(Unit owner)
        {
            Owner = owner;
        }

        public override void Update(int msElapsed)
        {
            var baseMsMod = Owner.StateFlags.GetMoveSpeedMultiplier();
            var state = Owner.MovementState;
            if (Type == MovementType.None || baseMsMod.Equals(0) || !state.IsMoving)
                return;

            //get the suggested move position
            var dist = baseMsMod * Owner.MoveSpeed * msElapsed / 1000;
            if (state.HasMaxDistance)
                dist = Math.Min(state.MaxDistance, dist);

            //fix collision
            var finalPos = resolveStep(state.MoveDirection, dist);

            //only continue if we moved at least a bit.
            var finalDist = finalPos.DistanceTo(Owner.Position);
            var hasMovedMuch = finalDist / dist > 0.1;

            if (hasMovedMuch)
            {
                Owner.Position = finalPos;
            }
        }

        Vector2 resolveStep(float angle, float dist)
        {
            if (dist < 1e-5) // lets for 0.01sq/s @ 1000fps
                return Owner.Position;

            var suggestedPos = Owner.Position.PolarProjection(angle, dist);

            if (Owner.StateFlags.HasFlag(StateFlags.NoCollision))
                return suggestedPos;

            //check terrain
            var pathingShape = new Ellipse(suggestedPos, Owner.Scale / 2);
            var terrainOk = checkTerrain(pathingShape, angle);

            if (!terrainOk)
                return Owner.Position;

            //if (!isTileOk(Owner.Map.Terrain.Get(suggestedPos), Owner))
            //    suggestedPos = Owner.Position;


            //check objects
            if (hasCollision())
            {
                foreach (var obj in Owner.Map.GetObjectsInRange(new Ellipse(suggestedPos, Owner.Scale / 2)))
                    if (obj != Owner && obj.HasCollision)
                        return Owner.Position;
            }

            return suggestedPos;
        }


        bool checkTerrain(Ellipse e, float moveAngle)
        {
            var terrain = Owner.Map.Terrain;
            var p1 = (e.Center - e.Radius).Floor();
            var p2 = (e.Center + e.Radius).Floor();

            for (int ix = p1.X; ix <= p2.X; ix++)
                for (int iy = p1.Y; iy <= p2.Y; iy++)
                {
                    if (IsTileInEllipse(ix, iy))
                    {
                        var tty = terrain.Get(new Point(ix, iy));
                        if (!canMoveTo(tty))
                            return false;
                    }
                }

            return true;

            bool IsTileInEllipse(int x, int y)
                => e.Contains(ClosestPointInTile(x, y));

            Vector2 ClosestPointInTile(int tileX, int tileY) => new Vector2(
                (tileX < e.Center.X) ? tileX + 1 : tileX,
                (tileY < e.Center.Y) ? tileY + 1 : tileY
            );
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
    }
}
