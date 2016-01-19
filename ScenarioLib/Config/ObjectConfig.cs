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

        public ObjectConstructor(IGameObject obj, Vector location)
        {
            TypeName = obj.GetType().FullName;
            Location = location;
        }
    }
}
