using Shanism.Common.Game;
using Shanism.Common.Objects;
using Shanism.Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;

namespace Shanism.ScenarioLib
{
    /// <summary>
    /// A command for the creation of some type of <see cref="IEntity"/> at map startup. 
    /// </summary>
    public class ObjectConstructor
    {
        /// <summary>
        /// The location where the entity is to be created. 
        /// </summary>
        public Vector Location { get; set; }

        /// <summary>
        /// The full name of the particular type of <see cref="IEntity"/> that is created. 
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// The owner of the entity that is to be created. 
        /// Only used with objects inheriting <see cref="IUnit"/>. 
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// The animation that is to be displayed on the created object. 
        /// Especially useful for objects of base types (e.g. doodad, effect). 
        /// </summary>
        public string Animation { get; set; }


        public ShanoColor? Tint { get; set; }


        public double Size { get; set; }


        public ObjectConstructor Clone()
        {
            return (ObjectConstructor)MemberwiseClone();
        }
    }
}
