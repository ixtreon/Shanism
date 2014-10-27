using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShanoRpgWinGl.Sprites;
using ShanoRpgWinGl.UI;

namespace ShanoRpgWinGl.Objects
{
    class DoodadControl : ObjectControl
    {
        public IDoodad Doodad
        {
            get { return (IDoodad)Object; }
        }


        public DoodadControl(IDoodad d)
            : base(d)
        {
            Sprite.Period = 100;
        }

        public override void Update(int msElapsed)
        {
            // ...
            base.Update(msElapsed);
        }

        public override void Draw(SpriteBatch sb)
        {
            Vector2 sz = new Vector2((float)Doodad.Size);

            var c = Color.White;
            Sprite.Draw(sb, ScreenPosition, ScreenSize, c);
        }
    }
}
