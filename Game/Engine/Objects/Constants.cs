using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Objects
{
    static class Constants
    {
        //most of these are self-explanatory

        public const double BaseLifeRegen = 0.5;
        public const double BaseManaRegen = 2;
        public const double BaseMagicDamage = 0;

        public static class Attributes
        {
            public const double DamagePerStrength = 1;
            public const double DefensePerStrength = 2;

            public const double LifePerVitality = 10;
            public const double LifeRegPerVitality = 0.1;
            public const double ManaPerVitality = 4;
            public const double ManaRegPerVitality = 0.05;

            public const double ManaRegPerInt = 0.10;
            public const double MagicDamagePerInt = 1;

            public const double AtkSpeedPerAgility = 1;        // as percentage;
            public const double DodgePerAgility = 0.1;         // as percentage;
        }

        public static class Experience
        {
            public const int Base = 10;
            public const int LevelFactor = 15;
        }

        public const double DamageReductionPerDefense = 0.05;
    }
}
