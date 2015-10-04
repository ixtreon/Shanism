using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Systems;
using IO.Objects;
using IO.Common;
using IO;
using Engine.Objects.Game;
using Engine.Objects;

namespace Engine.Systems.Buffs
{

    /// <summary>
    /// Represents any in-game effect which has a temporary or permanent effect on a unit's statistics. 
    /// </summary>
    public class Buff : ScenarioObject
    {
        int _moveSpeed;


        /// <summary>
        /// Gets or sets icon of the buff. Also see <see cref=""/>
        /// </summary>
        public string Icon { get; set; }


        /// <summary>
        /// Gets or sets the movement speed modifier of this buff. 
        /// </summary>
        public int MoveSpeed
        {
            get
            {
                return _moveSpeed;
            }

            set
            {
                _moveSpeed = Math.Max(-100, Math.Min(100, value));
            }
        }

        public int AttackSpeed { get; set; }

        public string Name { get; set; }

        public string RawDescription { get; set; }

        public string Description
        {
            get { return RawDescription.FormatWith(this); }
        }

        public string Id
        {
            get { return Name; }
        }

        /// <summary>
        /// Gets or sets the life modifier of this buff. 
        /// </summary>
        public double Life { get; set; }

        /// <summary>
        /// Gets or sets the mana modifier of this buff. 
        /// </summary>
        public double Mana { get; set; }

        /// <summary>
        /// Gets or sets the defense provided by this buff. 
        /// </summary>
        public double Defense { get; set; }

        /// <summary>
        /// Gets or sets the mnimum damage modifier of this buff. 
        /// </summary>
        public double MinDamage { get; set; }

        /// <summary>
        /// Gets or sets the maximum damage modifier of this buff. 
        /// </summary>
        public double MaxDamage { get; set; }

        /// <summary>
        /// Gets the strength modifier of this buff. 
        /// </summary>
        public double Strength { get; set; }

        /// <summary>
        /// Gets the vitality modifier of this buff. 
        /// </summary>
        public double Vitality { get; set; }

        /// <summary>
        /// Gets the agility modifier of this buff. 
        /// </summary>
        public double Agility { get; set; }

        /// <summary>
        /// Gets the intellect modifier of this buff. 
        /// </summary>
        public double Intellect { get; set; }

        /// <summary>
        /// The total duration of the buff, in miliseconds. 
        /// </summary>
        public int FullDuration { get; set; }

        public bool IsTimed
        {
            get { return Type != BuffType.Aura; }
        }

        public BuffType Type { get; private set; }


        /// <summary>
        /// Creates a new aura. 
        /// </summary>
        public Buff(BuffType type)
        {
            this.Type = type;
            this.Icon = "default2";
        }


        /// <summary>
        /// Creates a new buff with duration. 
        /// </summary>
        public Buff(BuffType type, int duration)
            : this(type)
        {
            this.FullDuration = duration;
        }
        

        public virtual void OnExpired(BuffInstance buff) { }

        public virtual void OnUpdate(BuffInstance buff) { }

        public virtual void OnApplied(BuffInstance buff) { }


    }
}
