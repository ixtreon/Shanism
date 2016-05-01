using Engine.Objects;
using Engine.Objects.Buffs;
using System.Collections.Generic;

namespace Engine.Systems.Buffs
{
    public interface IUnitBuffs : IEnumerable<BuffInstance>
    {
        /// <summary>
        /// Applies the given buff to the unit. 
        /// </summary>
        /// <param name="caster">The caster of the buff. Can be null. </param>
        /// <param name="b">The buff to apply. </param>
        BuffInstance TryApply(Unit caster, Buff b);

        /// <summary>
        /// Purges all buffs from this unit. 
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes the given buff instance from this unit. 
        /// </summary>
        /// <param name="buff"></param>
        void Remove(BuffInstance buff);

        /// <summary>
        /// Removes all instances of the given buff. 
        /// </summary>
        /// <param name="buffType">The buff prototype to remove instances of. </param>
        void Remove(Buff buffType);

        /// <summary>
        /// Removes a specified number of instances of the given buff from this unit's buffs. 
        /// </summary>
        /// <param name="buffType">The buff prototype to remove instances of. </param>
        /// <param name="nStacks">The maximum number of stacks of this buff to remove. </param>
        void Remove(Buff buffType, int nStacks);
    }
}