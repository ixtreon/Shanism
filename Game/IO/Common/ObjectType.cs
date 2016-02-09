using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using IO.Objects;
using ProtoBuf;

namespace IO.Common
{
    /// <summary>
    /// The different object types as seen by the client. 
    /// </summary>
    public enum ObjectType
    {
        Unit, Effect, Doodad, Hero,

        Buff, Ability, Item,
    }
}