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
    /// An animated sprite. 
    /// </summary>
    class AnimatedSprite : Sprite
    {
        public readonly int TotalFrames;

        public int CurrentFrame { get; internal set; }

        public int Period { get; set; }

        private int timeElapsed = 0;

        private ShanoTexture texture;

        public AnimatedSprite(AnimationDefOld m)
            : base(m)
        {
            texture = new ShanoTexture(File);
            this.CurrentFrame = 0;
            this.TotalFrames = texture.Count;
            this.Period = m.Period;
            SourceRectangle = texture.GetTileRect(CurrentFrame);
        }

        public override void Update(int msElapsed)
        {
            if (TotalFrames <= 1)
                return;
            timeElapsed += msElapsed;
            while (timeElapsed > Period)
            {
                timeElapsed -= Period;
                CurrentFrame++;
                if (CurrentFrame == TotalFrames)
                    CurrentFrame = 0;
                SourceRectangle = texture.GetTileRect(CurrentFrame);
            }
        }
    }
}
