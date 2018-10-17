using Shanism.Client.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI.Game.Units
{
    /// <summary>
    /// Displays a (stretched!) <see cref="EntitySprite"/>.
    /// </summary>
    class EntitySpriteBox : Control
    {
        public EntitySprite Target { get; set; }

        public override void Draw(Canvas c)
        {
            base.Draw(c);

            if(Target != null)
                c.DrawSprite(Target, ClientBounds, Target.Tint);
        }
    }
}
