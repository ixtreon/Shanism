using Ix.Math;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Shanism.Engine.Entities
{
    /// <summary>
    /// A special type of effect that travels in a straight line.
    /// Raises an event whenever it collides with enemy <see cref="Unit"/>s on its way. 
    /// Supports setting the max distance travelled, whether the projectile is
    /// destroyed on collision, and tracks units that are hit.  
    /// </summary>
    public class Projectile : Effect
    {
        /// <summary>
        /// Gets or sets the speed of the projectile in game units per second. 
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the direction (in radians) this projectile is traveling at. 
        /// </summary>
        public float Direction { get; set; }

        /// <summary>
        /// Gets or sets the maximum distance the projectile will travel before getting destroyed. 
        /// <para>Has a default value of 10. </para>
        /// </summary>
        public float MaxRange { get; set; }

        /// <summary>
        /// Gets or sets whether the projectile will be automatically destroyed after it collides with a unit. 
        /// The default value is true. 
        /// </summary>
        public bool DestroyOnCollision { get; set; }

        /// <summary>
        /// Gets the distance this projectile has travelled so far. 
        /// </summary>
        /// <returns></returns>
        public float DistanceTravelled { get; private set; }

        /// <summary>
        /// Raised whenever the projecticle collides with a unit. 
        /// </summary>
        public event Action<Projectile, Unit> OnUnitCollision;

        /// <summary>
        /// Contains all units we have hit so far. 
        /// </summary>
        readonly HashSet<Unit> unitsHit = new HashSet<Unit>();

        /// <summary>
        /// Gets or sets the owner of this projectile. 
        /// </summary>
        /// <returns></returns>
        public Unit Owner { get; set; }

        /// <summary>
        /// Marks the selected unit as already hit, so it won't be damaged twice. 
        /// </summary>
        /// <param name="u"></param>
        public void IgnoreUnit(Unit u)
        {
            unitsHit.Add(u);
        }

        /// <summary>
        /// Creates a new projectile at the specified in-game location.
        /// </summary>
        /// <param name="owner">The unit that owns the projectile.</param>
        /// <param name="ignoredUnits">The collection this projectile will not collide with.</param>
        /// <param name="speed">The speed of the projectile, in tiles per second.</param>
        /// <param name="direction">The direction of the projectile in radians.</param>
        /// <param name="maxRange">The maximum range travelled before the projectile is destroyed.</param>
        /// <param name="collideDestroy">Whether to automatically destroy the projectile when it collides an unit.</param>
        public Projectile(Unit owner, 
            IEnumerable<Unit> ignoredUnits = null,
            float speed = 20, 
            float direction = 0, 
            float maxRange = 10, 
            bool collideDestroy = true)
        {
            Owner = owner;

            Speed = speed;
            Direction = direction;
            MaxRange = maxRange;
            DestroyOnCollision = collideDestroy;

            if (ignoredUnits != null)
                foreach (var u in ignoredUnits)
                    unitsHit.Add(u);
        }

        /// <summary>
        /// This method is run every time a projectile is updated. 
        /// </summary>
        /// <param name="msElapsed"></param>
        protected override void OnUpdate(int msElapsed)
        {
            if (IsDestroyed)
                return;

            // update location
            var dist = (Speed * msElapsed / 1000);
            Position = Position.PolarProjection(Direction, dist);
            DistanceTravelled += dist;

            //get valid targets
            var units = Map
                .GetUnitsInRange(new Ellipse(Position, Scale / 2))
                .Where(u => !u.IsDead && u.Owner.IsEnemyOf(Owner) && !unitsHit.Contains(u))
                .ToList();

            if (units.Any())
            {
                if (DestroyOnCollision)
                {
                    var firstUnit = units.ArgMin(u => u.Position.DistanceToSquared(Position));

                    OnUnitCollision?.Invoke(this, firstUnit);

                    Destroy();
                    return;
                }

                // and fire the event. 
                foreach (var target in units)
                {
                    unitsHit.Add(target);

                    OnUnitCollision?.Invoke(this, target);
                }
            }

            if (MaxRange > 0 && DistanceTravelled > MaxRange)
                Destroy();

        }

    }
}
