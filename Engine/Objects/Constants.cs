using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Objects
{
    static class Constants
    {
        public const double BaseLifeRegen = 0.5;
        public const double BaseManaRegen = 2;
        public const double BaseMagicDamage = 0;

        // are those used anywhere?
        public  const double DamagePerStrength = 1;
        public const double DefensePerStrength = 2;

        public const double LifePerVitality = 10;
        public const double LifeRegPerVitality = 0.1;
        public const double ManaPerVitality = 4;
        public const double ManaRegPerVitality = 0.05;

        //public const double ManaPerInt = 8;
        public const double ManaRegPerInt = 0.10;
        public const double MagicDamagePerInt = 1;

        public const double AtkSpeedPerAgility = 1;        // as percentage;
        public const double DodgePerAgility = 0.1;         // as percentage;

        public const double DamageReductionPerDefense = 0.05;
    }
}
