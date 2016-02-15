using IO;
using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;

namespace IO.Objects
{
    /// <summary>
    /// Represents an empty ability as reconstructed by a network client. 
    /// </summary>
    public class AbilityStub : IAbility
    {
        public uint Id { get; set; }

        public double CastRange { get; private set; }

        public int CastTime { get; private set; }

        public int Cooldown { get; private set; }

        public int CurrentCooldown { get; private set; }

        public string Description { get; private set; }

        public string Icon { get; private set; }

        public int ManaCost { get; private set; }

        public string Name { get; private set; }

        public AbilityTargetType TargetType { get; private set; }
    }
}
