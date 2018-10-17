using Shanism.Client.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client;
using System.Numerics;
using Shanism.Client.Sprites;

namespace Shanism.Editor.UI.InGame.Animations
{
    class AnimationPreview : SpriteBox
    {

        public void SetAnimation(ShanoAnimation a)
        {
            Sprite = new DynamicSprite { Animation = a };
        }

        public AnimationPreview()
        {
            
        }

        public override void Draw(Canvas c)
        {

            base.Draw(c);


            //c.Draw(animation, Vector2.Zero, Size);
        }
    }
}
