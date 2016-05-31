using Shanism.Engine.Entities;
using Shanism.Engine.Objects.Buffs;
using Shanism.Engine.Systems;
using Shanism.Common.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Game;

namespace Shanism.Engine.Systems.Buffs
{
    /// <summary>
    /// Keeps hold of all buffs currently applied on a target unit
    /// and provides methods for applying and dispelling buffs. 
    /// </summary>
    public class BuffSystem : UnitSystem, IUnitBuffs
    {
        ConcurrentSet<BuffInstance> buffs { get; } = new ConcurrentSet<BuffInstance>();

        readonly Unit Owner;

        public BuffSystem(Unit target)
        {
            Owner = target;
        }


        //used exclusively by the update() method
        readonly List<BuffInstance> buffsToRemove = new List<BuffInstance>();

        internal override void Update(int msElapsed)
        {
            //update buffs, remove expired ones
            buffsToRemove.Clear();
            foreach (var b in buffs)
            {
                b.Update(msElapsed);
                if (b.HasExpired)
                    buffsToRemove.Add(b);
            }
             
            foreach (var b in buffsToRemove)
                buffs.TryRemove(b);


        }

        /// <summary>
        /// Applies the given buff to the unit. 
        /// </summary>
        /// <param name="buff">The buff to apply. </param>
        /// <param name="caster">The caster of the buff. </param>
        public BuffInstance Apply(Unit caster, Buff buff)
        {
            var newBuff = new BuffInstance(buff, caster, Owner);
            buffs.TryAdd(newBuff);

            var existingBuffs = buffs
                .Where(oldBuff => oldBuff.Equals(newBuff))
                .OrderBy(b => b.DurationLeft)
                .ToList();

            //remove a stack if need be
            if (buff.MaxStacks > 0 && existingBuffs.Count >= buff.MaxStacks)
                buffs.TryRemove(existingBuffs.First());

            if (buff.StackType == BuffStackType.Refresh)
                foreach (var bi in existingBuffs)
                    bi.RefreshDuration();

            return newBuff;
        }

        /// <summary>
        /// Removes all instances of the given buff. 
        /// </summary>
        /// <param name="buffType">The buff prototype to remove instances of. </param>
        public void Remove(Buff buffType)
        {
            var toRemove = buffs
                .Where(b => b.Prototype.Equals(buffType))
                .ToList();

            foreach (var b in toRemove)
                buffs.TryRemove(b);
        }

        /// <summary>
        /// Removes a specified number of instances of the given buff from this unit's buffs. 
        /// </summary>
        /// <param name="buffType">The buff prototype to remove instances of. </param>
        /// <param name="nStacks">The maximum number of stacks of this buff to remove. </param>
        public void Remove(Buff buffType, int nStacks)
        {
            var toRemove = buffs
                .Where(b => b.Prototype.Equals(buffType))
                .Take(nStacks)
                .ToList();

            foreach (var b in toRemove)
                buffs.TryRemove(b);
        }

        /// <summary>
        /// Removes the given buff instance from this unit. 
        /// </summary>
        /// <param name="buff"></param>
        public void Remove(BuffInstance buff)
        {
            if(buff != null)
                buffs.TryRemove(buff);
        }

        /// <summary>
        /// Purges all buffs from this unit. 
        /// </summary>
        public void Clear()
        {
            buffs.Clear();
        }

        public bool Contains(BuffInstance buff) => buffs.Contains(buff);


        #region IEnumerable<Buff> Implementation
        public IEnumerator<BuffInstance> GetEnumerator()
        {
            return buffs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return buffs.GetEnumerator();
        }
        #endregion
    }
}
