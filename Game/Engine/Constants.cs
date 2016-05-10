using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Engine
{
    static class Constants
    {
        //most of these are self-explanatory


        public static class GameMap
        {
            /// <summary>
            /// The size of a map chunk in in-game units. 
            /// </summary>
            public const int ChunkSize = 8;
        }

        public static class RangeEvents
        {
            /// <summary>
            /// The maximum range of a RangeEvent measured in chunks. 
            /// </summary>
            public const int MaxRangeChunks = 4;

            /// <summary>
            /// The maximum range of a RangeEvent measured in in-game units. 
            /// </summary>
            public const double MaxRangeUnits = MaxRangeChunks * GameMap.ChunkSize;
        }

        public static class Units
        {
            public const double MaxVisionRange = 10;

            public const double DefaultUnitSize = 2.5;


            public const double MaximumObjectSize = 20;

            /// <summary>
            /// The damage reduction per point of defense. 
            /// </summary>
            public const double DamageReductionPerDefense = 0.05;

            public const double BaseLifeRegen = 0.5;
            public const double BaseManaRegen = 2;
            public const double BaseMagicDamage = 0;


            public static class Attributes
            {
                public const double DamagePerStrength = 1;
                public const double DefensePerStrength = 2;

                public const double LifePerVitality = 10;
                public const double ManaPerVitality = 4;

                public const double LifeRegPerInt = 0.10;
                public const double ManaRegPerInt = 0.10;
                public const double MagicDamagePerInt = 1;

                public const double AtkSpeedPerAgility = 1;        // as percentage;
                public const double DodgePerAgility = 0.1;         // as percentage;
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
