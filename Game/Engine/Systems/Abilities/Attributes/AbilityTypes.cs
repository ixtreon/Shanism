using Engine.Objects;
using Engine.Objects.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Systems.Abilities
{
    static class AbilityTypes
    {
        public static IEnumerable<Ability> GetAbilitiesOfType(this Unit u, AbilityType type)
        {
            return u.Abilities
                .Where(a => a.GetType()
                    .GetCustomAttributes(typeof(AbilityTypeAttribute), false)
                    .Cast<AbilityTypeAttribute>()
                    .FirstOrDefault()?.Type.HasFlag(type) ?? false);
        }
    }

    [Flags]
    public enum AbilityType
    {
        /// <summary>
        /// Indicates that an ability is to be spammed. 
        /// </summary>
        Spammable = 1,
    }
}
