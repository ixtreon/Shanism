using Engine.Objects;
using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;

namespace Engine.Systems.Buffs
{
    /// <summary>
    /// Represents an instance of a buff applied to a unit. 
    /// </summary>
    public class BuffInstance : IBuffInstance
    {
        /// <summary>
        /// Gets the buff prototype of this instance. 
        /// </summary>
        public Buff Prototype { get; }

        /// <summary>
        /// Gets the target unit this buff instance is applied to. 
        /// </summary>
        public Unit Target { get; }

        /// <summary>
        /// Gets the duration left, in milliseconds, before this buff instance expires. 
        /// </summary>
        public int DurationLeft { get; private set; }

        /// <summary>
        /// Gets whether this buff should be removed from its target unit. 
        /// </summary>
        public bool ShouldDestroy 
        {
            get { return Prototype.IsTimed && DurationLeft <= 0; }
        }

        #region IBuff implementation
        public string Icon { get { return Prototype.Icon; } }

        public double Life { get { return Prototype.Life; } }

        public double Mana { get { return Prototype.Mana; } }

        public double Defense { get { return Prototype.Defense; } }

        public double Dodge { get { return Prototype.Dodge; } }

        public double MinDamage { get { return Prototype.MinDamage; } }

        public double MaxDamage { get { return Prototype.MaxDamage; } }

        public double MoveSpeed { get { return Prototype.MoveSpeed; } }

        public int MoveSpeedPercentage { get { return Prototype.MoveSpeedPercentage; } }

        public int AttackSpeed { get { return Prototype.AttackSpeed; } }

        public double Strength { get { return Prototype.Strength; } }

        public double Vitality { get { return Prototype.Vitality; } }

        public double Agility { get { return Prototype.Agility; } }

        public double Intellect { get { return Prototype.Intellect; } }

        public int FullDuration { get { return Prototype.FullDuration; } }

        public BuffType StackingType { get { return Prototype.StackingType; } }

        public bool Visible { get { return Prototype.Visible; } }

        public string Name { get { return Prototype.Name; } }

        public string Description { get { return Prototype.Description; } }
        #endregion


        public BuffInstance(Buff buff, Unit target)
        {
            Prototype = buff;
            Target = target;
            DurationLeft = buff.FullDuration;

            Prototype.OnApplied(this);
        }


        internal void Update(int msElapsed)
        {
            if (Prototype.IsTimed)
            {
                DurationLeft -= msElapsed;
                if (ShouldDestroy)
                {
                    Prototype.OnExpired(this);
                    return;
                }
            }

            Prototype.OnUpdate(this);
        }

        /// <summary>
        /// Refreshes the duration of this buff instance. 
        /// </summary>
        public void RefreshDuration()
        {
            DurationLeft = FullDuration;
            Prototype.OnRefresh(this);
        }
    }
}
