using Engine.Objects.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Objects
{
    public static class UnitHelper
    {
        public static bool IsHero(this Unit u)
        {
            return typeof(Hero).IsAssignableFrom(u.GetType());
        }

        public static bool IsNonPlayable(this Unit u)
        {
            return !u.IsHero();
        }
    }
}
