using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Engine
{
    static class Constants
    {
        //most of these are self-explanatory

        public static class Engine
        {
            public const bool UseMultithreading = false;

            /// <summary>
            /// The size of a single chunk of the entity map in in-game units. 
            /// </summary>
            public const int MapChunkSize = 2;
        }

        public static class RangeEvents
        {
            /// <summary>
            /// The maximum range of a RangeEvent measured in chunks. 
            /// </summary>
            public const int MaxRangeChunks = (int)(MaxRangeUnits / Engine.MapChunkSize + 0.5);

            /// <summary>
            /// The maximum range of a RangeEvent measured in in-game units. 
            /// </summary>
            public const double MaxRangeUnits = 32;
        }

        public static class Entities
        {
            /// <summary>
            /// The default size of a unit. 
            /// </summary>
            public const double DefaultSize = 2.5;

            /// <summary>
            /// The minimum size of an entity. 
            /// </summary>
            public const double MinSize = 0.1;

            /// <summary>
            /// The maximum size of an entity. 
            /// </summary>
            public const double MaxSize = 20;
        }

        public static class Units
        {
            public const double BaseLifeRegen = 0.5;
            public const double BaseManaRegen = 2;
            public const double BaseMagicDamage = 0;


            /// <summary>
            /// The damage reduction per point of defense. 
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
                public const int Base = 10;

                /// <summary>
                /// The bonus experience needed to reach each consecutive new level. 
                /// </summary>
                public const int LevelFactor = 15;
            }
        }


    }
}
