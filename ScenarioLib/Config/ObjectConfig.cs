using IO.Common;
using IO.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScenarioLib
{
    /// <summary>
    /// Lists all objects that should be created on map startup. 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ObjectConfig
    {
        /// <summary>
        /// Gets or sets the list of <see cref="ObjectConstructor"/>s that are created. 
        /// </summary>
        [JsonProperty]
        public List<ObjectConstructor> ObjectList { get; set; } = new List<ObjectConstructor>();
    }

    /// <summary>
    /// A <see cref="IGameObject"/> instance created on scenario startup. 
    /// </summary>
    public class ObjectConstructor
    {
        /// <summary>
        /// The location where this object is created. 
        /// </summary>
        public Vector Location { get; set; }

        /// <summary>
        /// The owner of the object if it is a unit. 
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// The full name of the particular type of <see cref="IGameObject"/> that is created. 
        /// </summary>
        public string TypeName { get; set; }


        [JsonConstructor]
        internal ObjectConstructor() { }
    }
}
