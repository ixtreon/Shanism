using DefaultScenario.Abilities;
using Engine;
using Engine.Entities.Objects;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultScenario.Units
{
    class Albimon : Hero
    {
        public Albimon(Player owner)
            : base(owner)
        {
            Name = "Albimon";

        }
    }
}
