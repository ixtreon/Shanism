using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Client.Sprites;
using Client.UI;

namespace Client.Objects
{
    /// <summary>
    /// Represents the in-game control of a specific doodad. 
    /// </summary>
    class DoodadControl : ObjectControl
    {
        /// <summary>
        /// Gets the doodad associated with this DoodadControl. 
        /// </summary>
        public IDoodad Doodad
        {
            get { return (IDoodad)Object; }
        }


        public DoodadControl(IDoodad d)
            : base(d)
        {
            this.ClickThrough = true;
        }

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);
        }

        public override void Draw(SpriteBatch sb)
        {
            Vector2 sz = new Vector2((float)Doodad.Size);

            var c = Color.White;
            Sprite.DrawScreen(sb, ScreenPosition, ScreenSize, c);
        }
    }
}
 