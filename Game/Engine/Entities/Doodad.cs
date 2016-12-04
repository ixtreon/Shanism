using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.StubObjects;
using Shanism.Common;
using Shanism.Common.Util;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Engine.Entities
{
    /// <summary>
    /// Represents all in-game objects which are not units but have collision. 
    /// </summary>
    public class Doodad : Entity, IDoodad
    {

        /// <summary>
        /// Gets the object type of this doodad. 
        /// Always has a value of <see cref="ObjectType.Doodad"/>. 
        /// </summary>
        public override ObjectType ObjectType { get; } = ObjectType.Doodad;

        /// <summary>
        /// Gets whether this doodad has collision. 
        /// Always has a value of false. 
        /// </summary>
        public override bool HasCollision => true;


        /// <summary>
        /// Creates a new doodad with default values. 
        /// </summary>
        public Doodad() { }


        /// <summary>
        /// Creates a new <see cref="Doodad"/> that is a clone of the given <see cref="Doodad"/>. 
        /// </summary>
        /// <param name="base">The doodad that is to be cloned. </param>
        public Doodad(Doodad @base)
            : base(@base) { }


    }
}
