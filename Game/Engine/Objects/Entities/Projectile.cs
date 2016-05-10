using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Maps;
using Shanism.Common.Game;
using Shanism.Engine.Objects.Entities;
using Shanism.Common;

namespace Shanism.Engine.Objects.Entities
{
    /// <summary>
    /// A projectile is a special type of effect that travels in a straight line
    /// and fires an event whenever it collides with <see cref="Unit"/>s on its way. 
    /// This class also supports setting the max distance travelled, whether the projectile is
    /// destroyed on collision and tracking of the units that are hit.  
    /// </summary>
    public class Projectile : Effect
    {
        /// <summary>
        /// Gets or sets the speed of the projectile in game units per second. 
        /// </summary>
        public double Speed = 3;

        /// <summary>
        /// Gets or sets the direction (in radians) this projectile is traveling at. 
        /// </summary>
        public double Direction = 0;

        /// <summary>
        /// Gets or sets the maximum distance the projectile will travel before getting destroyed. 
        /// <para>Has a default value of 10. </para>
        /// </summary>
        public double MaxRange = 10;

        /// <summary>
        /// Gets or sets whether the projectile will be automatically destroyed after it collides with a unit. 
        /// The default value is true. 
        /// </summary>
        public bool DestroyOnCollision = true;

        /// <summary>
        /// Gets the distance this projectile has travelled so far. 
        /// </summary>
        /// <returns></returns>
        public double DistanceTravelled { get; private set; }

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
        /// <param name="owner">The unit that owns the projectile. </param>
        /// <param name="ignoredUnits">The collection this projectile will not collide with. </param>
        public Projectile(Unit owner, IEnumerable<Unit> ignoredUnits = null)
        {
            Owner = owner;

            if (ignoredUnits != null)
                foreach (var u in ignoredUnits)
                    unitsHit.Add(u);
        }

        /// <summary>
        /// This method is run every time a projectile is updated. 
        /// </summary>
        /// <param name="msElapsed"></param>
        internal override void Update(int msElapsed)
        {
            // update location
            var dist = (Speed * msElapsed / 1000);
            Position = Position.PolarProjection(Direction, dist);
            DistanceTravelled += dist;

            //get valid targets
            var units = Map
                .GetUnitsInRange(Position, (Scale + Constants.Units.MaximumObjectSize) / 2 )
                .Where(u => !u.IsDead
                    && u.Owner.IsEnemyOf(Owner)
                    && !unitsHit.Contains(u)
                    && u.Position.DistanceTo(Position) < (u.Scale + Scale) / 2)
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

            if (DistanceTravelled > MaxRange && MaxRange > 0)
                Destroy();

            base.Update(msElapsed);
        }

    }
}
