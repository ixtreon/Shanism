using IO;
using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace IO.Objects
{
    public class AbilityStub : ObjectStub, IAbility
    {
        public double CastRange { get; private set; }

        public int CastTime { get; private set; }

        public int Cooldown { get; private set; }

        public int CurrentCooldown { get; private set; }

        public string Description { get; private set; }

        public string Icon { get; private set; }
        public int ManaCost { get; private set; }

        public string Name { get; private set; }

        public AbilityTargetType TargetType { get; private set; }

        public AbilityStub() { }

        public AbilityStub(uint id)
            : base(id)
        {

        }
    }
}
