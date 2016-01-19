using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Objects;
using IO.Common;
using IO;

namespace Engine.Objects.Game
{
    /// <summary>
    /// Represents all in-game objects which are not units, such as trees, rocks, barrels, projectiles, special effects. 
    /// </summary>
    public class Doodad : GameObject, IDoodad
    {

        public override ObjectType ObjectType
        {
            get { return ObjectType.Doodad; }
        }

        private double _maxLife = 5;

        public double MaxLife
        {
            get { return _maxLife; }
            set
            {
                Life = Life / _maxLife * value;
                _maxLife = value;
            }
        }

        public double Life = 5;

        public bool Invulnerable = false;

        /// <summary>
        /// Creates a new doodad with a default model and size at the target location. 
        /// </summary>
        /// <param name="location">The location of the doodad. </param>
        public Doodad(Vector location)
            : base(location)
        {
        }

        public Doodad(Doodad @base)
            : base(@base) { }

        public override string ToString()
        {
            return "Doodad @ {0}".F(Position);
        }
    }
}
