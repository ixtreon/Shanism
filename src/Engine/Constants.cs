using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Engine
{
    static class Constants
    {
        //most of these are self-explanatory

        public static class Map
        {
            /// <summary>
            /// The minimum size of a quadtree chunk.
            /// </summary>
            public const int ChunkMinSize = 2;
        }


        public static class Units
        {
            /// <summary>
            /// The increase in damage reduction per point of defense.
            /// <para/>
            /// Incoming physical damage is divided by the current 
            /// reduction factor to calculate the final damage.
            /// Units with 0 defense have a reduction factor of 1. 
            /// </summary>
            public const float DamageReductionPerDefense = 0.05f;
        }

        public static class Heroes
        {

            public static class Attributes
            {
                //Strength gives damage and defense
                public const float DamagePerStrength = 1;
                public const float DefensePerStrength = 2;

                //vitality gives mana and life
                public const float LifePerVitality = 10;
                public const float ManaPerVitality = 4;

                //int gives magic damage and regen
                public const float LifeRegPerInt = 0.10f;
                public const float ManaRegPerInt = 0.10f;
                public const float MagicDamagePerInt = 1;

                //agility gives attack speed, dodge (precision? crit?)
                public const float AtkSpeedPerAgility = 1;        // as percentage;
                public const float DodgePerAgility = 0.1f;         // as percentage;
            }

            public static class Experience
            {
                /// <summary>
                /// The base experience needed to reach a new level. 
                /// </summary>
                public static int BaseReward { get; } = 9;

                /// <summary>
                /// The extra experience needed to reach each consecutive level. 
                /// </summary>
                public static int PerLevelReward { get; } = 1;

                public static int BaseRequired { get; } = 53;

                public static int PerLevelRequired { get; } = 153;
            }
        }


    }
}
