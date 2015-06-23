using Client.Objects;
using Client.Sprites;
using IO;
using IO.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Objects
{
    class HeroControl : UnitControl
    {

        public HeroControl(IUnit u)
            : base(u)
        {

        }

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);
        }
    }
}
