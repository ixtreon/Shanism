using Client;
using Client.Textures;
using IO.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Sprites
{
    /// <summary>
    /// An instance of a model used for a specific game object or asset. 
    /// </summary>
    public abstract class Sprite
    {
        public readonly Texture2D Texture;

        public readonly AnimationDef Model;

        public TextureDef File
        {
            get { return Model.File; }
        }

        public Rectangle SourceRectangle { get; internal set; }

        public Color Color { get; set; }

        public Sprite(AnimationDef model)
        {
            this.Color = Color.White;
            this.Model = model;
            this.Texture = TextureCache.Get(File);
        }

        public virtual void Update(int msElapsed) { }

        public virtual void DrawScreen(SpriteBatch spriteBatch, Point location, Point size, Color color)
        {
            var destinationRectangle = new Rectangle(location.X, location.Y, size.X, size.Y);

            spriteBatch.Draw(Texture, destinationRectangle, SourceRectangle, (Color)color);
        }


        public void DrawScreen(SpriteBatch spriteBatch, Point location, Point size)
        {
            DrawScreen(spriteBatch, location, size, Color);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 uiLocation, Vector2 uiSize)
        {
            Draw(spriteBatch, uiLocation, uiSize, Color);
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 uiLocation, Vector2 uiSize, Color color)
        {
            var pLoc = Screen.UiToScreen(uiLocation);
            var farPos = Screen.UiToScreen(uiLocation + uiSize);
            var pSize = farPos - pLoc;

            DrawScreen(spriteBatch, pLoc, pSize, color);
        }

        public void DrawInGame(SpriteBatch spriteBatch, Vector2 gameLocation, Vector2 gameSize)
        {
            DrawInGame(spriteBatch, gameLocation, gameSize, Color);
        }

        public void DrawInGame(SpriteBatch spriteBatch, Vector2 gameLocation, Vector2 gameSize, Color color)
        {
            var pLoc = Screen.GameToScreen(gameLocation);
            var farPos = Screen.GameToScreen(gameLocation + gameSize);
            var pSize = farPos - pLoc;

            DrawScreen(spriteBatch, pLoc, pSize, color);
        }
    }
}
