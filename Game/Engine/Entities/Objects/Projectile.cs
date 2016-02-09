using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Maps;
using IO.Common;
using Engine.Entities.Objects;

namespace Engine.Entities.Objects
{
    /// <summary>
    /// A projectile is a special type of doodad that travels in a straight line
    /// and fires an event whenever it collides with <see cref="Unit"/>s on its way. 
    /// This class also supports setting the max distance travelled, whether the projectile is
    /// destroyed on collision and tracking of the units that are hit.  
    /// </summary>
    public class Projectile : Doodad
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
        private HashSet<Unit> unitsHit = new HashSet<Unit>();

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
        /// <param name="location">The starting position of the projectile. </param>
        /// <param name="model">The name of the model of this projectile. </param>
        /// <param name="owner">The owner unit of the projectile, if known. </param>
        /// <param name="name">The in-game name of the projectile. </param>
        /// <param name="ignoredUnits">The collection this projectile will not collide with. </param>
        public Projectile(Vector position, 
            string model = IO.Constants.Content.DefaultValues.ModelName, 
            Unit owner = null,
            string name = "streli mreli", 
            IEnumerable<Unit> ignoredUnits = null)
        {
            Position = position;
            ModelName = model;

            this.Owner = owner;
            this.Destructible = false;

            if(ignoredUnits != null)
                foreach(var u in ignoredUnits)
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
            this.DistanceTravelled += dist;

            //get valid targets
            //TODO: use Bounds.Size instead of textureSize
            var units = Map.GetUnitsInRange(Position, Scale / 2)
                .Where(u => u.IsNonPlayable() && !u.IsDead);

            if(units.Any())
            {
                //get units hit by the projectile
                var collidedUnits = units
                    .Where(u => !unitsHit.Contains(u))
                    .OrderBy(u => u.Position.DistanceToSquared(Position))
                    .ToList();

                // and fire the event. 
                foreach (var target in collidedUnits)
                {
                    unitsHit.Add(target);

                    OnUnitCollision?.Invoke(this, target);

                    if (DestroyOnCollision)
                    {
                        this.Destroy();
                        break;
                    }
                }
            }

            if (DistanceTravelled > MaxRange && MaxRange != 0)
                this.Destroy();

            base.Update(msElapsed);
        }

    }
}
