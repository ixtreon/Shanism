using Engine.Entities.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities
{
    public static class UnitHelper
    {
        public static bool IsHero(this Unit u)
        {
            return u is Hero;
        }

        public static bool IsNonPlayable(this Unit u)
        {
            return !u.IsHero();
        }
    }
}
