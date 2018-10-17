using Shanism.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Shanism.Common.Entities;
using Ix.Math;

namespace Shanism.Engine.Entities
{
    /// <summary>
    /// A type of unit that can gather experience points and has a number of attributes 
    /// that affect its main statistics (life, mana etc). 
    /// </summary>
    public class Hero : Unit, IHero
    {
        /// <summary>
        /// Gets the object type of this hero. 
        /// Always has a value of <see cref="ObjectType.Hero"/>. 
        /// </summary>
        public override ObjectType ObjectType => ObjectType.Hero;


        readonly HeroAttributes baseAttributes = new HeroAttributes(10);
        internal readonly HeroAttributes attributes = new HeroAttributes(0);


        IHeroAttributes IHero.BaseAttributes => baseAttributes;
        IHeroAttributes IHero.Attributes => attributes;

        /// <summary>
        /// The total experience of the hero. 
        /// </summary>
        int _experience;

        /// <summary>
        /// Gets the experience needed to reach the next level. 
        /// </summary>
        public int ExperienceNeeded 
            => Constants.Heroes.Experience.BaseRequired + Constants.Heroes.Experience.PerLevelRequired * Level;

        /// <summary>
        /// The current experience of the hero.
        /// </summary>
        public int Experience
        {
            get => _experience; 
            set
            {
                if (value < _experience)
                    throw new Exception("You must supply an experience value higher than the previous one!");
                _experience = value;
                while (_experience >= ExperienceNeeded)
                {
                    _experience -= ExperienceNeeded;
                    Level++;
                    OnLevelUp();
                }
            }
        }

        #region Base Stats
        /// <summary>
        /// Gets or sets the base strength of the hero. 
        /// </summary>
        public ref float BaseStrength => ref baseAttributes.Get(HeroAttribute.Strength);
        /// <summary>
        /// Gets or sets the base vitality of the hero. 
        /// </summary>
        public ref float BaseVitality => ref baseAttributes.Get(HeroAttribute.Vitality);
        /// <summary>
        /// Gets a reference to this hero's base intellect.
        /// </summary>
        public ref float BaseIntellect => ref baseAttributes.Get(HeroAttribute.Intellect);

        /// <summary>
        /// Gets or sets the base agility of the hero. 
        /// </summary>
        public ref float BaseAgility => ref baseAttributes.Get(HeroAttribute.Agility);
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

            DefaultOrder = null;
        }

        internal void updateHeroStats()
        {
            attributes.Set(baseAttributes);
            for(int i = Buffs.Count - 1; i >= 0; i--)
                attributes.Add(Buffs[i].Prototype.heroStats);
        }

        protected virtual void OnLevelUp() { }
    }
}
