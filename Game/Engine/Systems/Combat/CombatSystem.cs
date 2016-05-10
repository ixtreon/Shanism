using Shanism.Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Systems
{
    class CombatSystem : UnitSystem
    {
        public Unit Owner { get; }


        public CombatSystem(Unit target)
        {
            Owner = target;
        }


        /// <summary>
        /// Updates the <see cref="Unit.Mana"/> and <see cref="Unit.Life"/> based on <see cref="Unit.ManaRegen"/> and <see cref="Unit.LifeRegen"/>, respectively. 
        /// </summary>
        /// <param name="msElapsed"></param>
        internal override void Update(int msElapsed)
        {
            //TODO: handle negative life regen killing the unit. 
            var lifeDelta = Owner.LifeRegen * msElapsed / 1000;
            Owner.Life = Math.Min(Owner.MaxLife, Owner.Life + lifeDelta);

            var newMana = Owner.Mana + Owner.ManaRegen * msElapsed / 1000;
            Owner.Mana = Math.Min(Owner.MaxMana, newMana);
        }
    }
}
