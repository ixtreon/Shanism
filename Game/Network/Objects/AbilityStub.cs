using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Objects
{
    class AbilityStub : IAbility
    {
        public double CastRange { get; private set; }

        public int CastTime { get; private set; }

        public int Cooldown { get; private set; }

        public int CurrentCooldown { get; private set; }

        public string Description { get; private set; }

        public string Icon { get; private set; }

        public int ManaCost { get; private set; }

        public string Name { get; private set; }
    }
}
