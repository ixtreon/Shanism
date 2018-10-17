using Shanism.Engine.Entities;
using Shanism.Engine.Objects.Buffs;
using System.Collections.Generic;

namespace Shanism.Engine.Systems
{
    public interface IUnitBuffs : IReadOnlyList<BuffInstance>
    {

        /// <summary>
        /// Applies the given buff to the unit. 
        /// </summary>
        /// <param name="caster">The caster of the buff. Can be null. </param>
        /// <param name="b">The buff to apply. </param>
        BuffInstance Apply(Unit caster, Buff b);

        /// <summary>
        /// Purges all buffs from this unit. 
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes the given buff instance from this unit. 
        /// </summary>
        /// <param name="buff"></param>
        bool Remove(BuffInstance buff);

        /// <summary>
        /// Removes all instances of the given buff. 
        /// </summary>
        /// <param name="buffType">The buff prototype to remove instances of. </param>
        int Remove(Buff buffType);

        /// <summary>
        /// Removes a specified number of instances of the given buff from this unit's buffs. 
        /// </summary>
        /// <param name="buffType">The buff prototype to remove instances of. </param>
        /// <param name="nStacks">The maximum number of stacks of this buff to remove. </param>
        int Remove(Buff buffType, int nStacks);

        /// <summary>
        /// Gets whether the unit is affected by this buff instance.
        /// </summary>
        bool Contains(BuffInstance b);

        /// <summary>
        /// Gets whether the unit is affected by an instance of this buff type.
        /// </summary>
        bool Contains(Buff b);

    }
}