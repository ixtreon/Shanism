using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Objects;
using IO.Common;

namespace Client.Objects
{
    class EffectControl : ObjectControl
    {
        public EffectControl(IEffect effect) : base(effect)
        {

        }

        public override void OnDraw(Graphics g)
        {
            g.Draw(Sprite, Vector.Zero, Size);
        }
    }
}
