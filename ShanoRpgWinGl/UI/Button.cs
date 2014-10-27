using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShanoRpgWinGl.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanoRpgWinGl.UI
{
    /// <summary>
    /// A button that shows an image and can be clicked. 
    /// </summary>
    class Button : UserControl
    {
        public Texture2D Texture { get; set; }

        public Keys Keybind { get; set; }

        public bool HasBorder { get; set; }

        public Button()
        {
            this.MouseDown += onMouseDown;
        }

        private void onMouseDown(Vector2 p)
        {
            
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture ?? SpriteCache.BlankTexture.Texture, ScreenPosition, ScreenSize);

            if (HasBorder)
            {
                var border = MouseOver ? SpriteCache.Icon.BorderHover : SpriteCache.Icon.Border;
                border.Draw(sb, ScreenPosition, ScreenSize);
            }
        }
    }
}
