using Shanism.Common;
using Shanism.Common.StubObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Game;
using Shanism.Common.Interfaces.Objects;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Shanism.Common.StubObjects
{
    public class AbilityStub : ObjectStub, IAbility
    {
        public double CastRange { get; set; }

        public int CastTime { get; set; }

        public int Cooldown { get; set; }

        public int CurrentCooldown { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }
        public int ManaCost { get; set; }

        public string Name { get; set; }

        public AbilityTargetType TargetType { get; set; }

        public AbilityStub() { }

        public AbilityStub(uint id)
            : base(id)
        {

        }
    }
}
