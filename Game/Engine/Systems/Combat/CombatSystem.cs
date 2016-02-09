using Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Systems
{
    class CombatSystem : UnitSystem
    {
        public Unit Owner { get; }


        public CombatSystem(Unit target)
        {
            Owner = target;
        }


        /// <summary>
        /// Updates the <see cref="Mana"/> and <see cref="Life"/> based on <see cref="ManaRegen"/> and <see cref="LifeRegen"/>, respectively. 
        /// </summary>
        /// <param name="msElapsed"></param>
        internal override void Update(int msElapsed)
        {
            if (Owner.Life < Owner.MaxLife)
            {
                var lifeDelta = Owner.LifeRegen * msElapsed / 1000;
                Owner.Life = Math.Min(Owner.MaxLife, Owner.Life + lifeDelta);
            }

            if (Owner.Mana < Owner.MaxMana)
            {
                var newMana = Owner.Mana + Owner.ManaRegen * msElapsed / 1000;
                Owner.Mana = Math.Min(Owner.MaxMana, newMana);
            }
        }
    }
}
