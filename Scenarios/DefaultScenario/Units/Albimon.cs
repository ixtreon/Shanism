using Engine;
using Engine.Objects.Game;
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
        public Albimon(Player owner, Vector loc)
            : base(owner, loc)
        {
            Name = "Albimon";
        }
    }
}
