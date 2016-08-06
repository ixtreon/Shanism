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

        readonly HeroAttributes baseAttributes = new HeroAttributes(10);
        readonly HeroAttributes attributes = new HeroAttributes();

        IHeroAttributes IHero.BaseAttributes => baseAttributes;
        IHeroAttributes IHero.Attributes => attributes;

        public int Experience { get; private set; }
        public int ExperienceNeeded { get; private set; }

        public HeroStub(uint guid)
            : base(guid)
        {

        }
    }
}
