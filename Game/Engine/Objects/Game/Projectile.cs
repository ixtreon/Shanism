using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Maps;
using IO.Common;
using Engine.Objects.Game;

namespace Engine.Objects.Game
{
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
        public Hero Owner { get; set; }

        /// <summary>
        /// Marks the selected unit as already hit, so it won't be damaged twice. 
        /// </summary>
        /// <param name="u"></param>
        public void IgnoreUnit(Unit u)
        {
            unitsHit.Add(u);
        }

        public Projectile(string model, Vector location, Hero owner = null, string name = "streli mreli", IEnumerable<Unit> ignoredUnits = null)
            : base(model, location)
        {
            this.Owner = owner;
            this.Invulnerable = true;
            foreach(var u in ignoredUnits)
                unitsHit.Add(u);
        }

        internal override void Update(int msElapsed)
        {
            // update location
            var dist = (Speed * msElapsed / 1000);
            Location = Location.PolarProjection(Direction, dist);
            this.DistanceTravelled += dist;

            //get valid targets
            var units = Map.GetUnitsInRange(Location, Size / 2)
                .Where(u => u.IsNonPlayable() && !u.IsDead);

            if(units.Any())
            {
                //get units hit by the projectile
                var collidedUnits = units
                    .Where(u => !unitsHit.Contains(u))
                    .OrderBy(u => u.Location.DistanceToSquared(Location));

                // and fire the event. 
                foreach (var target in collidedUnits)
                {
                    unitsHit.Add(target);

                    if (OnUnitCollision != null)
                        OnUnitCollision(this, target);

                    if (DestroyOnCollision)
                    {
                        this.Destroy();
                        break;
                    }
                }
            }

            if (DistanceTravelled > MaxRange && MaxRange != 0)
                this.Destroy();
            
        }

    }
}
