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
        public double Speed = 3;

        public double Direction = 0;

        /// <summary>
        /// Gets or sets the maximum distance the Projectile will travel before getting destroyed. 
        /// <para>Has a default value of 10. </para>
        /// </summary>
        public double MaxRange = 10;

        /// <summary>
        /// Gets or sets whether this Projectile will be automatically destroyed when it collides with a unit. 
        /// <para>Has a default value of true. </para>
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


        private HashSet<Unit> unitsHit = new HashSet<Unit>();

        public Hero Owner { get; set; }

        public Projectile(string model, Hero owner = null, string name = "streli mreli")
            : base(name)
        {
            this.Owner = owner;
            this.Invulnerable = true;
            this.Model = model;
        }
        public override void UpdateLocation(int msElapsed)
        {
            var dNow = (Speed * msElapsed / 1000);
            var unit = new Vector(Math.Sin(Direction), Math.Cos(Direction));

            this.DistanceTravelled += dNow;

            if (DistanceTravelled > MaxRange && MaxRange != 0)
                this.Destroy();

            this.Location += unit * dNow;
        }

        public override void UpdateEffects(int msElapsed)
        {
            var units = Map.GetUnitsInRange(Location, Size)
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
