using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Engine.Events;
using Shanism.Engine.Objects.Buffs;

namespace ScenarioTemplate.Abilities
{
    class ShanoBuff : Buff
    {
        public ShanoBuff()
        {
            Name = "Dummy Buff";
            RawDescription = "Provides {";

            Icon = Constants.Content.DefaultValues.Icon;

            Type = BuffType.NonStacking;
            FullDuration = 5000;
            
        }

        public override void OnApplied(BuffInstance buff)
        {
            //This code is executed whenever this buff is initially applied to a unit. 
        }

        public override void OnExpired(BuffInstance buff)
        {
            //This code is executed whenever this buff is initially expires from a unit. 
        }
    }
}
