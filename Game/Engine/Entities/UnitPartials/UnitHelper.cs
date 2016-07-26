using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Entities
{
    partial class Unit
    {
        public bool IsHero()
        {
            return this is Hero;
        }

        public bool IsNonPlayable()
        {
            return !Owner.IsHuman;
        }
    }
}
