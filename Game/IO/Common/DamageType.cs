using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Common
{
    public enum DamageType
    {
        Physical,
        Magical,
    }

    public static class DamageTypeExt
    {
        public static double GetResistance(this IUnit u, DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Physical:
                    return 1 / (Constants.Engine.DamageReductionPerDefense * u.Defense + 1);

                case DamageType.Magical:
                    return 0;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
