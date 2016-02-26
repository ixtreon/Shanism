using IO.Common;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Interfaces.Engine
{
    public interface IGameObject
    {
        /// <summary>
        /// Gets the ID of the object. 
        /// </summary>
        [ProtoIgnore]
        uint Id { get; }

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        [ProtoIgnore]
        ObjectType ObjectType { get; }
    }
}
