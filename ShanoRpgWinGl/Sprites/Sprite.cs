using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShanoRpgWinGl
{
    /// <summary>
    /// A specific instance of a texture intended for drawing. 
    /// Supports slicing up a texture to display it frames, control of the frame duration and tint.  
    /// </summary>
    class Sprite
    {
        public readonly Texture2D Texture;

        public readonly int Rows;
        public readonly int Columns;

        public readonly int TotalFrames;

        public readonly int
            FrameWidth,
            FrameHeight;

        public int CurrentFrame { get; private set; }
        public int Period { get; set; }

        public Color Tint { get; set; }

        private int timeElapsed = 0;

        Rectangle[] sourceRect;

        /// <summary>
        /// Creates a new Sprite from the provided texture with the given size and period. 
        /// </summary>
        public Sprite(Texture2D texture, int rows, int columns, int period = 500)
        {
            if (rows < 1 || columns < 1)
                throw new ArgumentOutOfRangeException("Number of cells must be a positive integer. ");

            this.Texture = texture;
            this.Rows = rows;
            this.Columns = columns;

            this.FrameWidth = Texture.Width / Columns;
            this.FrameHeight = Texture.Height / Rows;

            this.CurrentFrame = 0;
            this.TotalFrames = Rows * Columns;

            this.Period = period;

            //pre-compute texture sources for the different frames. 
            this.sourceRect = new Rectangle[TotalFrames];
            for (int i = 0; i < TotalFrames; i++)
            {
                int row = i / Columns;
                int column = i % Columns;

                sourceRect[i] = new Rectangle(FrameWidth * column, FrameHeight * row, FrameWidth, FrameHeight);
            }

            this.Tint = Color.White;
        }

        /// <summary>
        /// Creates a new Sprite with a single frame and no period. 
        /// </summary>
        public Sprite(Texture2D texture)
            : this(texture, 1, 1)
        { }

        public Sprite Clone(int newPeriod)
        {
            return new Sprite(Texture, Rows, Columns, newPeriod);
        }


        public void Update(int msElapsed)
        {
            if (TotalFrames == 1)
                return;
            timeElapsed += msElapsed;
            while(timeElapsed > Period)
            {
                timeElapsed -= Period;
                CurrentFrame++;
                if (CurrentFrame == TotalFrames)
                    CurrentFrame = 0;
            }
        }


        public void Draw(SpriteBatch spriteBatch, Point location, Point size)
        {
            Draw(spriteBatch, location, size, Tint);
        }

        public void Draw(SpriteBatch spriteBatch, Point location, Point size, Color color)
        {
            var sourceRectangle = sourceRect[CurrentFrame];
            var destinationRectangle = new Rectangle(location.X, location.Y, size.X, size.Y);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, (Color)color);
        }

        public void DrawUi(SpriteBatch spriteBatch, Vector2 uiLocation, Vector2 uiSize)
        {
            DrawUi(spriteBatch, uiLocation, uiSize, Tint);
        }
        public void DrawUi(SpriteBatch spriteBatch, Vector2 uiLocation, Vector2 uiSize, Color color)
        {
            var pLoc = Screen.UiToScreen(uiLocation);
            var farPos = Screen.UiToScreen(uiLocation + uiSize);
            var pSize = farPos - pLoc;

            Draw(spriteBatch, pLoc, pSize, color);
        }

        public void DrawInGame(SpriteBatch spriteBatch, Vector2 gameLocation, Vector2 gameSize)
        {
            DrawInGame(spriteBatch, gameLocation, gameSize, Tint);
        }

        public void DrawInGame(SpriteBatch spriteBatch, Vector2 gameLocation, Vector2 gameSize, Color color)
        {
            var pLoc = Screen.GameToScreen(gameLocation);
            var farPos = Screen.GameToScreen(gameLocation + gameSize);
            var pSize = farPos - pLoc;

            Draw(spriteBatch, pLoc, pSize, color);
        }
    }
}
