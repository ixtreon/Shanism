using Engine.Systems.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using IO.Util;

namespace Engine.Objects
{
    /// <summary>
    /// Represents all buffs currently applied on a target unit. 
    /// </summary>
    public class BuffSystem : IEnumerable<BuffInstance>
    {
        ConcurrentSet<BuffInstance> buffs { get; }

        Unit Target { get; }

        public BuffSystem(Unit u)
        {
            buffs = new ConcurrentSet<BuffInstance>();
            Target = u;
        }

        /// <summary>
        /// Applies the given buff to the unit. 
        /// </summary>
        /// <param name="b">The buff to apply. </param>
        public void Add(Buff b)
        {
            if (Target.IsDead)
                return;


            //if no such buffs, just apply a new instance. 
            if (buffs.TryAdd(new BuffInstance(b, Target)))
                return;

            var existing = buffs
                .Where(buff => b.Id == buff.Prototype.Id)
                .ToList();

            //if such a buff already exists, refer to the stacking type of the buff. 
            switch (b.StackingType)
            {
                case IO.Common.BuffType.Aura:
                case IO.Common.BuffType.NonStacking:
                    existing.FirstOrDefault()?.RefreshDuration();   //should have just one such buff
                    break;

                case IO.Common.BuffType.StackingRefresh:            //add new, refresh existing
                    foreach (var eb in existing)
                        eb.RefreshDuration();
                    buffs.TryAdd(new BuffInstance(b, Target));
                    break;

                case IO.Common.BuffType.StackingNormal:             //just add new buff instance
                    buffs.TryAdd(new BuffInstance(b, Target));
                    break;
            }
        }

        /// <summary>
        /// Removes all instances of the given buff. 
        /// </summary>
        /// <param name="buffType">The buff prototype to remove instances of. </param>
        public void Remove(Buff buffType)
        {
            var toRemove = buffs
                .Where(b => b.Prototype == buffType)
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
                .Where(b => b.Prototype == buffType)
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
            buffs.TryRemove(buff);
        }

        /// <summary>
        /// Purges all buffs from this unit. 
        /// </summary>
        public void Clear()
        {
            buffs.Clear();
        }

        internal void Update(int msElapsed)
        {
            var toRemove = new List<BuffInstance>();

            foreach (var b in buffs)
            {
                b.Update(msElapsed);
                if (b.ShouldDestroy)
                    toRemove.Add(b);
            }

            foreach (var b in toRemove)
                buffs.TryRemove(b);
        }

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
