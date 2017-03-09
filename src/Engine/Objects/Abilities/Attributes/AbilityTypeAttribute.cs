using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Objects.Abilities
{
    /// <summary>
    /// Specifies the <see cref="AbilityTypeFlags"/> of an ability. 
    /// These are used to inform the engine (and eventually the behaviour/AI subsystem) of an ability's intended use.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    class AbilityTypeAttribute : Attribute
    {
        public readonly AbilityTypeFlags Type;

        public AbilityTypeAttribute(AbilityTypeFlags type)
        {
            this.Type = type;
        }
    }
}
