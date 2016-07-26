using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Objects.Abilities
{
    /// <summary>
    /// Specifies that an ability is meant to be constantly spammed towards one's opponent. 
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
