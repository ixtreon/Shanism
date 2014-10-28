using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShanoRpgWinGl.Sprites;
using ShanoRpgWinGl.UI;

namespace ShanoRpgWinGl.Objects
{
    abstract class ObjectControl : Control
    {
        public Sprite Sprite { get; private set; }

        public IGameObject Object { get; private set; }

        public Vector2? CustomLocation { get; set; }

        public ObjectControl(IGameObject obj)
        {
            this.Object = obj;
            Sprite = SpriteCache.NewModel(obj.Model);
            Sprite.Period = obj.Model.Period;
            //Sprite.Tint = obj.Tint;
        }

        public override void Update(int msElapsed)
        {
            //update the sprite
            Sprite.Update(msElapsed);

            //update object position
            var loc = CustomLocation ?? Object.Location.ToVector2();
            Vector2 sz = new Vector2((float)Object.Size / 2);
            var posLo = Screen.GameToUi(loc - sz);
            var posHi = Screen.GameToUi(loc + sz);

            this.AbsolutePosition = posLo;
            this.Size = posHi - posLo;

            base.Update(msElapsed);
        }

    }
}
