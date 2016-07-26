using Shanism.Engine.Objects;
using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Objects.Abilities
{
    static class AbilityTypes
    {
        /// <summary>
        /// Returns all abilities of this unit that are of the given <see cref="AbilityTypeFlags"/>. 
        /// </summary>
        /// <param name="u"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Ability> GetAbilitiesOfType(this Unit u, AbilityTypeFlags type)
        {
            return u.Abilities
                .Where(a => a.GetType()
                    .GetCustomAttributes(typeof(AbilityTypeAttribute), false)
                    .Cast<AbilityTypeAttribute>()
                    .FirstOrDefault()?.Type.HasFlag(type) ?? false);
        }
    }

    [Flags]
    public enum AbilityTypeFlags
    {
        /// <summary>
        /// Indicates that an ability is to be spammed. 
        /// </summary>
        Spammable = 1,
    }
}
