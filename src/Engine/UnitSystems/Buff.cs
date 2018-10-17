using Shanism.Engine.Entities;
using Shanism.Engine.Objects.Buffs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;

namespace Shanism.Engine.Systems
{
    /// <summary>
    /// Keeps hold of all buffs currently applied on a target unit
    /// and provides methods for applying and dispelling buffs. 
    /// </summary>
    class BuffSystem : UnitSystem, IUnitBuffs
    {

        readonly List<BuffInstance> buffList = new List<BuffInstance>();
        readonly Dictionary<BuffInstance, int> buffSet = new Dictionary<BuffInstance, int>();

        readonly Unit Owner;


        public BuffSystem(Unit target)
        {
            Owner = target;
        }

        public int Count => buffList.Count;

        public BuffInstance this[int id] => buffList[id];

        public override void Update(int msElapsed)
        {
            //update buffs, remove expired ones
            for(int i = 0; i < buffList.Count; i++)
            {
                var b = buffList[i];

                b.Update(msElapsed);

                if(b.HasExpired || b.IsDestroyed)
                    remove(b, i--);
            }
        }

        /// <summary>
        /// Applies the given buff to the unit. 
        /// </summary>
        /// <param name="prototype">The buff to apply. </param>
        /// <param name="caster">The caster of the buff. </param>
        public BuffInstance Apply(Unit caster, Buff prototype)
        {
            if(prototype == null)
                return null;

            // remove stacks
            if(prototype.MaxStacks > 0)
            {
                //throw new Exception("You can't debug me!");

                var curStacks = countBuffsOfType(prototype);
                var stacksToRemove = prototype.MaxStacks - curStacks;

                while(stacksToRemove > 0 && tryFindShortestTimeLeft(prototype, out var id, out var buff))
                {
                    remove(buff, id);
                    stacksToRemove--;
                }
            }

            //refresh durations
            if(prototype.StackType == BuffStackType.Refresh)
                for(int i = 0; i < buffList.Count; i++)
                    if(buffList[i].Equals(prototype))
                        buffList[i].RefreshDuration();

            // apply the buff
            var newBuff = new BuffInstance(prototype, caster, Owner);
            add(newBuff);
            prototype.OnApplied(newBuff);

            return newBuff;
        }

        /// <summary>
        /// Removes all instances of the given buff. 
        /// </summary>
        /// <param name="prototype">The buff prototype to remove instances of. </param>
        /// <returns>The number of stacks of the buff that were removed.</returns>
        public int Remove(Buff prototype)
            => Remove(prototype, buffList.Count);

        /// <summary>
        /// Removes a specified number of instances of the given buff from this unit's buffs. 
        /// </summary>
        /// <param name="prototype">The buff prototype to remove instances of. </param>
        /// <param name="stacksToRemove">The maximum number of stacks of this buff to remove. </param>
        /// <returns>The number of stacks of the buff that were removed.</returns>
        public int Remove(Buff prototype, int stacksToRemove)
        {
            var stacksRemoved = 0;
            for(int i = 0; i < buffList.Count && stacksToRemove > 0; i++)
            {
                var b = buffList[i];
                if(b.Prototype.Id == prototype.Id)
                {
                    remove(b, i--);
                    stacksToRemove--;
                    stacksRemoved++;
                }
            }

            return stacksRemoved;
        }

        /// <summary>
        /// Removes the given buff instance from this unit. 
        /// </summary>
        /// <param name="buff"></param>
        public bool Remove(BuffInstance buff)
        {
            if(!buffSet.TryGetValue(buff, out var id))
                return false;

            remove(buff, id);
            return true;
        }

        /// <summary>
        /// Purges all buffs from this unit. 
        /// </summary>
        public void Clear()
        {
            buffList.Clear();
            buffSet.Clear();
        }

        public bool Contains(Buff b)
        {
            for(int i = 0; i < buffList.Count; i++)
                if(buffList[i].Prototype.Id == b.Id)
                    return true;
            return false;
        }
        public bool Contains(BuffInstance b)
            => buffSet.ContainsKey(b);

        int countBuffsOfType(Buff b)
        {
            var count = 0;
            for(int i = 0; i < buffList.Count; i++)
                if(buffList[i].Prototype.Id == b.Id)
                    count++;
            return count;
        }

        bool tryFindShortestTimeLeft(Buff prototype, out int id, out BuffInstance buff)
        {
            id = -1;
            buff = null;
            float duration = float.MaxValue;
            BuffInstance curBuff;
            for(int i = 0; i < buffList.Count; i++)
                if((curBuff = buffList[i]).Prototype.Id == prototype.Id)
                    if(curBuff.DurationLeft < duration)
                    {
                        id = i;
                        buff = curBuff;
                        duration = curBuff.DurationLeft;
                    }
            return buff != null;
        }

        void add(BuffInstance b)
        {
            buffSet.Add(b, buffList.Count);
            buffList.Add(b);
        }

        void remove(BuffInstance b, int id)
        {
            buffList.RemoveAtFast(id);
            buffSet.Remove(b);
            if(id < buffList.Count)
                buffSet[buffList[id]] = id;
        }

        public IEnumerator<BuffInstance> GetEnumerator() => buffList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
