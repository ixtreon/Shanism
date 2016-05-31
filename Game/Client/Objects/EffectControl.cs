using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Objects;
using Shanism.Common.Game;
using Shanism.Common;

namespace Shanism.Client.Objects
{
    class EffectControl : ObjectControl
    {
        public EffectControl(IEffect effect) : base(effect)
        {
            CanHover = false;
        }

        public override void OnDraw(Graphics g)
        {
            g.Draw(Sprite, Vector.Zero, Size, Object.CurrentTint.ToColor(), (float)ZOrder);
        }
    }
}
