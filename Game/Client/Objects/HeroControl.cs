using IO;
using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Objects
{
    class HeroControl : UnitControl
    {
        public IHero Hero { get; }

        public HeroControl(IHero hero)
            : base(hero)
        {
            this.Hero = hero;
        }

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);
        }
    }
}
