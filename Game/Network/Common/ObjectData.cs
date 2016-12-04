using Shanism.Common;
using Shanism.Common.Interfaces.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Network.Common
{
    struct ObjectData : IComparable<ObjectData>
    {
        public IGameObject Object;

        public ObjectType Type;

        //dreams of FP lands...
        public static bool operator ==(ObjectData a, ObjectData b)
            => a.Object?.Id == b.Object?.Id;

        public static bool operator !=(ObjectData a, ObjectData b)
            => a.Object?.Id != b.Object?.Id;

        public override bool Equals(object obj)
            => (obj is ObjectData) && ((ObjectData)obj) == this;

        public override int GetHashCode() => Object.Id.GetHashCode();

        public int CompareTo(ObjectData other)
            => this.Object.Id.CompareTo(other.Object.Id);
    }
}
