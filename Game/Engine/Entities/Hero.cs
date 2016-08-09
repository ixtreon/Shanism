using Shanism.Engine.Systems;
using Shanism.Common;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Shanism.Common.Game;
using Shanism.Engine.Common;
using Shanism.Engine.Entities;
using Shanism.Common.Message.Client;
using Shanism.Common.StubObjects;
using Shanism.Common.Util;
using Shanism.Common.Interfaces.Entities;
using Shanism.Engine.Objects.Orders;

namespace Shanism.Engine.Entities
{
    /// <summary>
    /// A type of unit that can gather experience points and has a number of attributes 
    /// that affect its main statistics (life, mana etc). 
    /// </summary>
    public class Hero : Unit, IHero
    {
        //multiplied by _level to obtain the Xp needed to level up. 
        const int XpPerLevel = 100;

        /// <summary>
        /// Gets the object type of this hero. 
        /// Always has a value of <see cref="ObjectType.Hero"/>. 
        /// </summary>
        public override ObjectType ObjectType => ObjectType.Hero;


        readonly HeroAttributes baseAttributes = new HeroAttributes(10);
        internal readonly HeroAttributes attributes = new HeroAttributes();


        IHeroAttributes IHero.BaseAttributes => baseAttributes;
        IHeroAttributes IHero.Attributes => attributes;

        /// <summary>
        /// The total experience of the hero. 
        /// </summary>
        int _experience;

        /// <summary>
        /// Gets the experience needed to reach the next level. 
        /// </summary>
        public int ExperienceNeeded => XpPerLevel * Level;

        /// <summary>
        /// The current experience of the hero.
        /// </summary>
        public int Experience
        {
            get { return _experience; }
            set
            {
                if (value < _experience)
                    throw new Exception("You must supply an experience value higher than the previous one!");
                _experience = value;
                while (_experience >= ExperienceNeeded)
                {
                    _experience -= ExperienceNeeded;
                    Level++;
                }
            }
        }

        #region Base Stats
        /// <summary>
        /// Gets or sets the base strength of the hero. 
        /// </summary>
        public float BaseStrength
        {
            get { return baseAttributes[HeroAttribute.Strength]; }
            set { baseAttributes[HeroAttribute.Strength] = value; }
        }
        /// <summary>
        /// Gets or sets the base vitality of the hero. 
        /// </summary>
        public float BaseVitality
        {
            get { return baseAttributes[HeroAttribute.Vitality]; }
            set { baseAttributes[HeroAttribute.Vitality] = value; }
        }
        /// <summary>
        /// Gets or sets the base intellect of the hero. 
        /// </summary>
        public float BaseIntellect
        {
            get { return baseAttributes[HeroAttribute.Intellect]; }
            set { baseAttributes[HeroAttribute.Intellect] = value; }
        }
        /// <summary>
        /// Gets or sets the base agility of the hero. 
        /// </summary>
        public float BaseAgility
        {
            get { return baseAttributes[HeroAttribute.Agility]; }
            set { baseAttributes[HeroAttribute.Agility] = value; }
        }
        #endregion

        #region Current Stats
        /// <summary>
        /// Gets the current strength of the hero. 
        /// </summary>
        public float Strength => attributes[HeroAttribute.Strength];
        /// <summary>
        /// Gets the current vitality of the hero. 
        /// </summary>
        public float Vitality => attributes[HeroAttribute.Vitality];
        /// <summary>
        /// Gets the current intellect of the hero. 
        /// </summary>
        public float Intellect => attributes[HeroAttribute.Intellect];
        /// <summary>
        /// Gets the current agility of the hero. 
        /// </summary>
        public float Agility => attributes[HeroAttribute.Agility];
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Hero"/> class.
        /// </summary>
        /// <param name="owner">The owner of the hero.</param>
        public Hero(Player owner)
            : base(owner)
        {
            Model = "units/hero";

            BaseMoveSpeed = 12;

            BaseMaxLife = 100;
            BaseMaxMana = 5;
        }

        internal void updateHeroStats()
        {
            attributes.Set(baseAttributes);
            foreach (var b in Buffs)
                attributes.Add(b.Prototype.heroStats);
        }
    }
}
