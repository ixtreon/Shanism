using Engine.Systems.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Engine.Objects
{
    public class UnitBuffs : IEnumerable<BuffInstance>
    {
        /// <summary>
        /// Gets the buffs of the unit. 
        /// </summary>
        private readonly Dictionary<string, BuffInstance> buffs = new Dictionary<string, BuffInstance>();

        public readonly Unit Unit;

        public UnitBuffs(Unit u)
        {
            Unit = u;
        }

        /// <summary>
        /// Applies the given buff to the unit. 
        /// </summary>
        /// <param name="b">The buff to apply. </param>
        public void Add(Buff b)
        {
            if(Unit.IsDead)
                return;

            if (!buffs.ContainsKey(b.Id))
                buffs.Add(b.Id, new BuffInstance(b, Unit));
        }

        public void Remove(string buffId)
        {
            throw new NotImplementedException();
        }

        public void Remove(Buff buffType)
        {
            throw new NotImplementedException();
        }

        public void Remove(BuffInstance buff)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            buffs.Clear();
        }

        public void Update(int msElapsed)
        {
            var toRemove = new List<BuffInstance>();

            foreach (var b in buffs.Values)
            {
                b.Update(msElapsed);
                if (b.ShouldDestroy)
                    toRemove.Add(b);
            }

            foreach (var b in toRemove)
            {
                buffs.Remove(b.Buff.Id);
            }
        }

        public IEnumerator<BuffInstance> GetEnumerator()
        {
            return buffs.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return buffs.Values.GetEnumerator();
        }
    }
}
