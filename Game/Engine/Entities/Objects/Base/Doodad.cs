using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Objects;
using IO.Common;
using IO;

namespace Engine.Entities.Objects
{
    /// <summary>
    /// Represents all in-game objects which are not units, such as trees, rocks, barrels, projectiles, special effects. 
    /// </summary>
    public class Doodad : GameObject, IDoodad
    {
        public override ObjectType Type {  get { return ObjectType.Doodad; } }


        public bool Destructible = true;

        /// <summary>
        /// Creates a new doodad with a default model and size at the (0, 0) location. 
        /// </summary>
        /// <param name="location">The location of the doodad. </param>
        public Doodad() { }

        public Doodad(
            string name = null,
            Vector? location = null,
            string modelName = null,
            double? scale = null)
            : base(name, location, modelName, scale) { }


        public Doodad(Doodad @base)
            : base(@base) { }

        public override string ToString()
        {
            return "Doodad @ {0}".F(Position);
        }
    }
}
