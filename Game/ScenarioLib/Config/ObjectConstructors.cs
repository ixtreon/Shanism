using Shanism.Common.Util;
using Shanism.Common;

namespace Shanism.ScenarioLib
{
    /// <summary>
    /// A command for the creation of some type of 
    /// a <see cref="Common.Interfaces.Entities.IEntity"/> at map startup. 
    /// </summary>
    public class ObjectConstructor
    {
        /// <summary>
        /// The location where the entity is to be created. 
        /// </summary>
        public Vector Location { get; set; }

        /// <summary>
        /// The full name of the particular type of <see cref="Common.Interfaces.Entities.IEntity"/> that is created. 
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// The owner of the entity that is to be created. 
        /// Only used with objects inheriting <see cref="Common.Interfaces.Entities.IUnit"/>. 
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// The animation that is to be displayed on the created object. 
        /// Especially useful for objects of base types (e.g. doodad, effect). 
        /// </summary>
        public string Model { get; set; }


        public Color? Tint { get; set; }


        public float Size { get; set; }


        public ObjectConstructor Clone()
        {
            return (ObjectConstructor)MemberwiseClone();
        }
    }
}
