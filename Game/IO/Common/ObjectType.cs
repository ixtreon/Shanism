using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using IO.Objects;
using IxSerializer.Modules;

namespace IO.Common
{
    /// <summary>
    /// An enumeration of the object types that are sent over the network. 
    /// </summary>
    [SerialKiller]
    public struct ObjectType
    {
        static readonly Dictionary<byte, ObjectType> objectDict = new Dictionary<byte, ObjectType>();

        public static readonly ObjectType Effect = new ObjectType(0, typeof(IEffect));
        public static readonly ObjectType Doodad = new ObjectType(1, typeof(IDoodad));
        public static readonly ObjectType Unit = new ObjectType(2, typeof(IUnit));
        public static readonly ObjectType Hero = new ObjectType(3, typeof(IHero));

        [SerialMember]
        public readonly byte Id;

        public readonly Type UnderlyingInterface;

        ObjectType(byte id, Type interfaceType)
        {
            Id = id;
            UnderlyingInterface = interfaceType;

            objectDict[id] = this;
        }

        public static implicit operator byte (ObjectType ty)
        {
            return ty.Id;
        }


        public static implicit operator ObjectType(byte b)
        {
            ObjectType val;
            if (!objectDict.TryGetValue(b, out val))
                throw new InvalidCastException("No {0} with Id = {1}".Format((object)nameof(ObjectType), b));
            return val;
        }

        public static bool operator == (ObjectType a, ObjectType b)
        {
            return a.Id == b.Id;
        }

        public static bool operator !=(ObjectType a, ObjectType b)
        {
            return a.Id != b.Id;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ObjectType))
                return false;
            return (ObjectType)obj == this;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return UnderlyingInterface.Name;
        }
    }
}