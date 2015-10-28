using Client;
using Client.Textures;
using IO.Common;
using IO.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace Client.Sprites
{
    /// <summary>
    /// An instance of a model used for a specific game object or asset. 
    /// </summary>
    public abstract class Sprite
    {
        public struct PointsInfo
        {
            Sprite sprite;

            public PointsInfo(Sprite s) { sprite = s; }
            
            public Vector Center
            {
                get
                {
                    return new Vector((float)sprite.SourceRectangle.Center.X / sprite.Texture.Width,
                      (float)sprite.SourceRectangle.Center.Y / sprite.Texture.Height);
                }
            }

            /// <summary>
            /// dx, dy should be -1, 0, 1
            /// </summary>
            /// <param name="dx"></param>
            /// <param name="dy"></param>
            /// <returns></returns>
            public Vector Get(int dx, int dy)
            {
                return Center + new Vector((float)dx * sprite.SourceRectangle.Width / sprite.Texture.Width / 2, (float)dy * sprite.SourceRectangle.Width / sprite.Texture.Width / 2);
            }

            public Vector TopLeft
            {
                get
                {
                    return new Vector((float)sprite.SourceRectangle.Left / sprite.Texture.Width,
                      (float)sprite.SourceRectangle.Top / sprite.Texture.Height);
                }
            }
            public Vector TopRight
            {
                get
                {
                    return new Vector((float)sprite.SourceRectangle.Right / sprite.Texture.Width,
                      (float)sprite.SourceRectangle.Top / sprite.Texture.Height);
                }
            }
            public Vector BottomLeft
            {
                get
                {
                    return new Vector((float)sprite.SourceRectangle.Left / sprite.Texture.Width,
                      (float)sprite.SourceRectangle.Bottom / sprite.Texture.Height);
                }
            }
            public Vector BottomRight
            {
                get
                {
                    return new Vector((float)sprite.SourceRectangle.Right / sprite.Texture.Width,
                      (float)sprite.SourceRectangle.Bottom / sprite.Texture.Height);
                }
            }
        }

        public readonly PointsInfo Points;

        public readonly Texture2D Texture;

        public readonly AnimationDefOld Model;

        public TextureDef File
        {
            get { return Model.File; }
        }

        public Rectangle SourceRectangle { get; internal set; }

        public Color Color { get; set; }

        public Sprite(AnimationDefOld model)
        {
            this.Points = new PointsInfo(this);
            this.Color = Color.White;
            this.Model = model;
            this.Texture = TextureCache.Get(File);
        }

        public virtual void Update(int msElapsed) { }

        public virtual void DrawScreen(SpriteBatch spriteBatch, Point location, Point size, Color color)
        {
            var destinationRectangle = new Rectangle(location.X, location.Y, size.X, size.Y);

            spriteBatch.ShanoDraw(Texture, destinationRectangle, SourceRectangle, color);
        }


        public void DrawScreen(SpriteBatch spriteBatch, Point location, Point size)
        {
            DrawScreen(spriteBatch, location, size, Color);
        }

        public void Draw(SpriteBatch spriteBatch, Vector uiLocation, Vector uiSize)
        {
            Draw(spriteBatch, uiLocation, uiSize, Color);
        }
        public void Draw(SpriteBatch spriteBatch, Vector uiLocation, Vector uiSize, Color color)
        {
            var pLoc = Screen.UiToScreen(uiLocation);
            var farPos = Screen.UiToScreen(uiLocation + uiSize);
            var pSize = farPos - pLoc;

            DrawScreen(spriteBatch, pLoc, pSize, color);
        }

        public void DrawInGame(SpriteBatch spriteBatch, Vector gameLocation, Vector gameSize)
        {
            DrawInGame(spriteBatch, gameLocation, gameSize, Color);
        }

        public void DrawInGame(SpriteBatch spriteBatch, Vector gameLocation, Vector gameSize, Color color)
        {
            var pLoc = Screen.GameToScreen(gameLocation);
            var farPos = Screen.GameToScreen(gameLocation + gameSize);
            var pSize = farPos - pLoc;

            DrawScreen(spriteBatch, pLoc, pSize, color);
        }
    }
}
