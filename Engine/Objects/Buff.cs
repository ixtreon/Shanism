using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Systems;

namespace Engine.Objects
{
    /// <summary>
    /// Represents any in-game effect which has a temporary or permanent effect on a unit's statistics. 
    /// </summary>
    public class Buff : ScenarioObject
    {
        public readonly int MoveSpeedPerc,
            AttackSpeedPerc;


        public readonly double Life,
            Mana,
            Defense,
            MinDamage,
            MaxDamage,
            Strength,
            Vitality,
            Agility,
            Intellect;

        /// <summary>
        /// The total duration of the buff, in seconds. 
        /// </summary>
        public double Duration;

        public double DurationLeft;

        public bool IsAura
        {
            get { return double.IsPositiveInfinity(Duration); }
        }

        /// <summary>
        /// Creates a new aura. 
        /// </summary>
        public Buff(int moveSpeed = 0, int atkSpeed = 0, double life = 0, double mana = 0,
            double defense = 0, double minDmg = 0, double maxDmg = 0)
            : this(double.PositiveInfinity, moveSpeed, atkSpeed, life, mana, defense, minDmg, maxDmg)
        { }


        /// <summary>
        /// Creates a new aura. 
        /// </summary>
        public Buff(double duration, int moveSpeed = 0, int atkSpeed = 0, double life = 0, double mana = 0, 
            double defense = 0, double minDmg = 0, double maxDmg = 0)
        {
            this.Duration = duration;
            this.DurationLeft = duration;

            this.MoveSpeedPerc = moveSpeed;
            this.AttackSpeedPerc = atkSpeed;
            this.Life = life;
            this.Mana = mana;
            this.Defense = defense;
            this.MinDamage = minDmg;
            this.MaxDamage = maxDmg;
        }

    }
}
