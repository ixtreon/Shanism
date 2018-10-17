using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Entities;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member


namespace Shanism.Common.ObjectStubs
{
    public class HeroStub : UnitStub, IHero
    {
        public override ObjectType ObjectType => ObjectType.Hero;

        public IHeroAttributes BaseAttributes { get; set; } = new HeroAttributes(0);
        public IHeroAttributes Attributes { get; set; } = new HeroAttributes(0);

        public int Experience { get; private set; }
        public int ExperienceNeeded { get; private set; }

        public HeroStub()
        {

        }

        public HeroStub(uint guid)
            : base(guid)
        {

        }
    }
}
