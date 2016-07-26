using Shanism.Common.Game;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Interfaces.Objects
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
