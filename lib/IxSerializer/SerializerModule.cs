using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IxSerializer
{
    /// <summary>
    /// A module for the serializer extension. 
    /// </summary>
    public abstract class SerializerModule
    {
        /// <summary>
        /// Gets whether this module can de/serialize the given type. 
        /// </summary>
        /// <param name="ty">The type to serialize. </param>
        public abstract bool CanSerialize(Type ty);

        /// <summary>
        /// Serializes the given type. 
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="ty"></param>
        /// <param name="o"></param>
        public abstract void Serialize(BinaryWriter buf, Type ty, object o);

        public abstract object Deserialize(BinaryReader buf, Type ty);

        public virtual bool AreEqual(Type ty, object a, object b)
        {
            return a.Equals(b);
        }

        public void Initialize()
        {
            OnInitialize();
        }

        protected virtual void OnInitialize() { }
    }
}
