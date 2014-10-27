using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Maps;
using IO.Common;

namespace Engine.Objects
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
        /// Activated whenever the projecticle collides with a unit. 
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

        public Projectile(string model, Hero owner = null, string name = "streli mreli")
            : base(name)
        {
            this.Owner = owner;
            this.Invulnerable = true;
            this.Model = model;
        }

        internal override void UpdateLocation(int msElapsed)
        {
            var dNow = (Speed * msElapsed / 1000);
            var unit = new Vector(Math.Sin(Direction), Math.Cos(Direction));

            this.DistanceTravelled += dNow;

            if (DistanceTravelled > MaxRange && MaxRange != 0)
                this.Destroy();

            this.Location += unit * dNow;
        }

        internal override void UpdateEffects(int msElapsed)
        {
            var units = Map.GetUnitsInRange(Location, Size / 2)
                .Where(u => u.IsNonPlayable() && !u.IsDead);

            if(units.Any())
            {
                var targets = units
                    .Where(u => !unitsHit.Contains(u))
                    .OrderBy(u => u.Location.DistanceToSquared(Location));
                foreach (var target in targets)
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
        }

    }
}
