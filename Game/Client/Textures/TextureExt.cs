using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using IO.Common;
using Color = Microsoft.Xna.Framework.Color;

namespace Client.Textures
{
    static class TextureExt
    {
        public static void Draw(this SpriteBatch sb, Texture2D tex, Point screenPosition, Point screenSize)
        {
            Draw(sb, tex, screenPosition, screenSize, Color.White);
        }
        
        public static void Draw(this SpriteBatch sb, Texture2D tex, Point screenPosition, Point screenSize, Color color)
        {
            var destinationRectangle = new Rectangle(screenPosition.X, screenPosition.Y, screenSize.X, screenSize.Y);
            sb.ShanoDraw(tex, destinationRectangle, color);
        }

        public static void DrawUi(this SpriteBatch sb, Texture2D tex, Vector gameLocation, Vector gameSize, Color color)
        {
            var pLoc = Screen.UiToScreen(gameLocation);
            var farPos = Screen.UiToScreen(gameLocation + gameSize);
            var pSize = farPos - pLoc;

            var destinationRectangle = new Rectangle(pLoc.X, pLoc.Y, pSize.X, pSize.Y);
            sb.ShanoDraw(tex, destinationRectangle, color);
        }

        public static void DrawGame(this SpriteBatch sb, Texture2D tex, Vector gameLocation, Vector gameSize, Color color)
        {
            var pLoc = Screen.GameToScreen(gameLocation);
            var farPos = Screen.GameToScreen(gameLocation + gameSize);
            var pSize = farPos - pLoc;

            var destinationRectangle = new Rectangle(pLoc.X, pLoc.Y, pSize.X, pSize.Y);
            sb.ShanoDraw(tex, destinationRectangle, color);
        }
    }
}
