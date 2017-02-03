using Shanism.Common.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Shanism.Common.StubObjects
{
    public class EffectStub : EntityStub, IEffect
    {
        public override ObjectType ObjectType => ObjectType.Effect;

        public EffectStub() { }

        public EffectStub(uint guid)
            : base(guid)
        {

        }
    }
}
