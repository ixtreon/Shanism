using Shanism.Common;
using Shanism.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Objects
{
    class HeroControl : UnitControl
    {
        public IHero Hero { get; }

        public HeroControl(IHero hero)
            : base(hero)
        {
            this.Hero = hero;
        }
    }
}
