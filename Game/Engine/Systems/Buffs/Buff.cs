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
        string _rawDescription;

        /// <summary>
        /// Gets or sets icon of the buff. 
        /// </summary>
        public string Icon { get; set; }

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
            get { return StackingType != BuffType.Aura; }
        }

        public BuffType StackingType { get; set; }

        public bool Visible { get; set; }



        /// <summary>
        /// Creates a new buff.
        /// </summary>
        /// <param name="type">The type of the buff.</param>
        /// <param name="duration">The duration of the buff if it is timed. </param>
        public Buff(BuffType type = BuffType.NonStacking, int duration = 5000)
        {
            StackingType = type;
            Icon = IO.Constants.Content.DefaultIcon;
            FullDuration = duration;
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
