using Ix.Math;
using Shanism.Client.Sprites;
using Shanism.Common;
using System;

namespace Shanism.Client.UI
{
    public class SpriteBox : Control
    {
        Sprite texture;
        TextureSizeMode textureSizeMode = TextureSizeMode.Stretch;

        /// <summary>
        /// Gets the bounds of the texture within the control.
        /// </summary>
        protected RectangleF TextureBounds { get; private set; }

        /// <summary>
        /// Gets the texture color (tint). 
        /// </summary>
        public Color Tint { get; set; } = Color.White;


        /// <summary>
        /// Gets the way the texture scales as the size of the control changes.
        /// <see cref="TextureSizeMode.Stretch"/> by default.
        /// </summary>
        public TextureSizeMode SpriteSizeMode
        {
            get => textureSizeMode;
            set
            {
                textureSizeMode = value;
                TextureBounds = getTextureBounds();
            }
        }

        /// <summary>
        /// Gets or sets the texture that is displayed. 
        /// </summary>
        protected Sprite Sprite
        {
            get => texture;
            set
            {
                texture = value;
                TextureBounds = getTextureBounds();
            }
        }


        public SpriteBox()
        {
            Padding = 0;
        }


        protected override void OnSizeChanged(EventArgs e)
        {
            TextureBounds = getTextureBounds();
            base.OnSizeChanged(e);
        }

        /// <summary>
        /// Draws background and/or texture.
        /// </summary>
        /// <param name="c"></param>
        public override void Draw(Canvas c)
        {
            DrawBackground(c, BackColor);
            DrawSprite(c, Tint);
        }

        protected void DrawSprite(Canvas c, Color tint)
        {
            if (Sprite == null)
                return;

            c.DrawSprite(Sprite, TextureBounds, tint);
        }

        public override void Update(int msElapsed)
        {
            
        }


        RectangleF getTextureBounds()
        {
            if (texture == null)
                return RectangleF.Empty;

            var texSz = Screen.UI.FromScreen(texture.SourceRectangle.Size);
            var boxSz = ClientBounds.Size;

            switch (textureSizeMode)
            {
                case TextureSizeMode.Stretch:
                    return ClientBounds;

                case TextureSizeMode.FitZoom:
                    var texRatio = texSz.X / texSz.Y;
                    var boxRatio = boxSz.X / boxSz.Y;
                    if (texRatio > boxRatio)
                        texSz *= boxSz.X / texSz.X;
                    else
                        texSz *= boxSz.Y / texSz.Y;

                    goto case TextureSizeMode.Center;

                case TextureSizeMode.Center:
                    var pos = ClientBounds.Position + (boxSz - texSz) / 2;
                    return new RectangleF(pos, texSz);

            }

            throw new ArgumentOutOfRangeException(nameof(SpriteSizeMode));
        }
    }
}
