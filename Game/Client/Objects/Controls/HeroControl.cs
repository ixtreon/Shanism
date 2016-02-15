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

        protected override void OnUpdate(int msElapsed)
        {
            base.OnUpdate(msElapsed);
        }

        public override void OnDraw(Graphics g)
        {
            base.OnDraw(g);
        }
    }
}
