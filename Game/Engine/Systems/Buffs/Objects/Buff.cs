using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Systems;
using IO.Objects;
using IO.Common;
using IO;
using Engine.Entities.Objects;
using Engine.Entities;

namespace Engine.Systems.Buffs
{

    /// <summary>
    /// Represents any in-game effect which has a temporary or permanent effect on a unit's statistics. 
    /// </summary>
    public class Buff : ScenarioObject, IBuff
    {
        int _moveSpeed;
        string _rawDescription;

        /// <summary>
        /// Gets or sets icon of the buff. 
        /// </summary>
        public string Icon { get; set; } = IO.Constants.Content.DefaultValues.Icon;

        /// <summary>
        /// Gets or sets the name of the buff. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the raw description of the buff. 
        /// <para/>
        /// Uses <see cref="IO.Ext.FormatWith{T}(string, T)"/> to parse "{strength}" into "5". 
        /// </summary>
        public string RawDescription
        {
            get { return _rawDescription; }
            set
            {
                _rawDescription = value;
                Description = _rawDescription.FormatWith(this);
            }
        }

        /// <summary>
        /// Gets the formatted description of this buff. 
        /// </summary>
        public string Description { get; private set; }

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
        /// Gets or sets the critical strike chance modifier provided by this buff. 
        /// </summary>
        public double Crit { get; set; }

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
        public int AttackSpeedPercentage { get; set; }

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
        /// Only used if <see cref="IsTimed"/> is true. 
        /// </summary>
        public int FullDuration { get; set; } = 5000;


        /// <summary>
        /// Gets or sets the type of this buff. 
        /// </summary>
        public BuffType Type { get; set; } = BuffType.NonStacking;

        /// <summary>
        /// Gets or sets whether this buff has an icon and shows in the default buff bar. 
        /// </summary>
        public bool HasIcon { get; set; } = true;


        /// <summary>
        /// Gets whether this buff is timed or indefinite (i.e. if its <see cref="Type"/> is <see cref="BuffType.NonStacking"/>). 
        /// </summary>
        public bool IsTimed
        {
            get { return Type != BuffType.Aura; }
        }



        /// <summary>
        /// Creates a new buff with default values. 
        /// </summary>
        /// <param name="type">The type of the buff.</param>
        /// <param name="duration">The duration of the buff if it is timed. </param>
        public Buff()
        {
        }

        public Buff(Buff @base)
        {
            this.Life = @base.Life;
            this.Mana = @base.Mana;
            this.Defense = @base.Defense;
            this.Dodge = @base.Dodge;
            this.MoveSpeed = @base.MoveSpeed;
            this.MoveSpeedPercentage = @base.MoveSpeedPercentage;
            this.AttackSpeedPercentage = @base.AttackSpeedPercentage;
            this.MinDamage = @base.MinDamage;
            this.MaxDamage = @base.MaxDamage;
            this.Strength = @base.Strength;
            this.Vitality = @base.Vitality;
            this.Agility = @base.Agility;
            this.Intellect = @base.Intellect;
            this.FullDuration = @base.FullDuration;
            this.Type = @base.Type;
            this.HasIcon = @base.HasIcon;
        }


        /// <summary>
        /// The method executed whenever a buff expires from a target. 
        /// </summary>
        /// <param name="buff"></param>
        public virtual void OnExpired(BuffInstance buff) { }

        /// <summary>
        /// The method executed on every update frame for which a buff is on a target. 
        /// </summary>
        /// <param name="buff"></param>
        public virtual void OnUpdate(BuffInstance buff) { }

        /// <summary>
        /// The method executed whenever a buff is refreshed on a target. 
        /// </summary>
        /// <param name="buff"></param>
        public virtual void OnRefresh(BuffInstance buff) { }

        /// <summary>
        /// The method executed whenever a buff is initially applied to a target. 
        /// </summary>
        /// <param name="buff"></param>
        public virtual void OnApplied(BuffInstance buff) { }


    }
}
