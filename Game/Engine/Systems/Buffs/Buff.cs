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
    public class Buff : ScenarioObject, IBuff
    {
        int _moveSpeed;


        /// <summary>
        /// Gets or sets icon of the buff. 
        /// </summary>
        public string Icon { get; set; }




        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the raw description of the buff. 
        /// <para/>
        /// Uses <see cref="IO.Ext.FormatWith{T}(string, T)"/> to parse "{strength}" into "5". 
        /// </summary>
        public string RawDescription { get; set; }
        
        /// <summary>
        /// Gets the formatted description of this buff. 
        /// </summary>
        public string Description
        {
            get { return RawDescription.FormatWith(this); }
        }

        /// <summary>
        /// Gets the identifier of this buff. Currently its name. 
        /// </summary>
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
        /// Gets or sets the dodge (evasion) modifier provided by this buff. 
        /// </summary>
        public double Dodge { get; set; }

        /// <summary>
        /// Gets or sets the movement speed modifier of this buff. 
        /// </summary>
        public double MoveSpeed { get; set; }

        /// <summary>
        /// Gets or sets the movement speed percentage modifier of this buff. 
        /// </summary>
        public int MoveSpeedPercentage
        {
            get { return _moveSpeed; }
            set { _moveSpeed = Math.Max(-100, Math.Min(100, value)); }
        }

        /// <summary>
        /// Gets or sets the attack speed percentage modifier of this buff. 
        /// </summary>
        public int AttackSpeed { get; set; }

        /// <summary>
        /// Gets or sets the mnimum damage modifier of this buff. 
        /// </summary>
        public double MinDamage { get; set; }

        /// <summary>
        /// Gets or sets the maximum damage modifier of this buff. 
        /// </summary>
        public double MaxDamage { get; set; }

        /// <summary>
        /// Gets or sets the strength modifier of this buff. 
        /// </summary>
        public double Strength { get; set; }

        /// <summary>
        /// Gets or sets the vitality modifier of this buff. 
        /// </summary>
        public double Vitality { get; set; }

        /// <summary>
        /// Gets or sets the agility modifier of this buff. 
        /// </summary>
        public double Agility { get; set; }

        /// <summary>
        /// Gets or sets the intellect modifier of this buff. 
        /// </summary>
        public double Intellect { get; set; }

        /// <summary>
        /// Gets or sets the total duration of the buff, in miliseconds. 
        /// </summary>
        public int FullDuration { get; set; }

        /// <summary>
        /// Gets whether this buff is timed or indefinite (i.e. an aura). 
        /// </summary>
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
