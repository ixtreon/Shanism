using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Interfaces.Entities;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member


namespace Shanism.Common.StubObjects
{
    public class HeroStub : UnitStub, IHero
    {

        public IHeroAttributes BaseAttributes { get; set; } = new HeroAttributes();
        public IHeroAttributes Attributes { get; set; } = new HeroAttributes();

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
