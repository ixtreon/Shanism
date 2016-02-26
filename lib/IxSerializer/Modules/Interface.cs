using IxSerializer.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IxSerializer.Modules
{

    class InterfaceSerializer : SerializerModule
    {
        public override bool CanSerialize(Type ty)
        {
            return typeof(IxSerializable).IsAssignableFrom(ty);
        }

        public override object Deserialize(BinaryReader buf, Type ty)
        {
            var obj = (IxSerializable)Activator.CreateInstance(ty);
            obj.Deserialize(buf);

            return obj;
        }

        public override void Serialize(BinaryWriter buf, Type ty, object o)
        {
            ((IxSerializable)o).Serialize(buf);
        }
    }

    public interface IxSerializable
    {
        /// <summary>
        /// Deserializes the data from the specified reader into this object. 
        /// </summary>
        void Deserialize(BinaryReader r);

        /// <summary>
        /// Serializes this object to the given writer. 
        /// </summary>
        void Serialize(BinaryWriter w);
    }
}
