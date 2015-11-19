using Engine.Objects.Game;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultScenario.Doodads
{
    class Tree : Doodad
    {
        public Tree(Vector location)
            : base("pruchka", location)
        {
            this.Size = 5;
            this.Name = "Tree";
        }
    }
}
