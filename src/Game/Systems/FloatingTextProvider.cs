using Shanism.Client.Game.Systems.FloatingText;
using Shanism.Client.IO;
using Shanism.Client.Systems;
using Shanism.Client.Text;
using Shanism.Common;
using System.Collections.Generic;
using System.Numerics;

namespace Shanism.Client.Game.Systems
{
    enum FloatingTextStyle
    {
        /// <summary>
        /// Throws up.
        /// </summary>
        Up,

        /// <summary>
        /// Always throw em down.
        /// </summary>
        Down,

        /// <summary>
        /// Throws a rainbow left; then right; then left...
        /// </summary>
        Rainbow,

        /// <summary>
        /// Throws em slowly left to right; then again.
        /// </summary>
        Sprinkle,
    }

    class FloatingTextProvider : ISystem
    {
        static int DefaultDuration = 1000;

        class FloatingTextData
        {
            public FloatingTextStyle Style;

            public Color Color;
            public Vector2 Location;
            public Vector2 Velocity;
            public Vector2 Gravity;

            public string Text;
            public int Duration;
        }

        static readonly Dictionary<FloatingTextStyle, IStyleFactory> textMakers = new Dictionary<FloatingTextStyle, IStyleFactory>
        {
            { FloatingTextStyle.Up, new UpFactory() },
            { FloatingTextStyle.Down, new DownFactory() },
            { FloatingTextStyle.Rainbow, new RainbowFactory() },
            { FloatingTextStyle.Sprinkle, new SprinkleFactory() },
        };


        readonly List<FloatingTextData> labels = new List<FloatingTextData>();
        readonly Font mainFont;
        readonly ScreenSystem screen;

        public FloatingTextProvider(ScreenSystem screen, FontCache fonts)
        {
            this.screen = screen;

            mainFont = fonts.NormalFont;
        }

        public void AddLabel(Vector2 inGamePos, string text, Color textColor, FloatingTextStyle style)
        {
            textMakers[style].CalcParams(inGamePos, out var speed, out var gravity);

            labels.Add(new FloatingTextData
            {
                Text = text,
                Duration = DefaultDuration,
                Style = style,
                Color = textColor,

                Location = inGamePos,
                Velocity = speed,
                Gravity = gravity,
            });
        }

        public void Update(int msElapsed)
        {
            for (int i = 0; i < labels.Count; i++)
            {
                var l = labels[i];
                l.Duration -= msElapsed;

                if (l.Duration > 0)
                {
                    l.Velocity += l.Gravity * msElapsed / 1000;
                    l.Location += l.Velocity * msElapsed / 1000;
                }
                else
                {
                    labels.RemoveAtFast(i--);
                }
            }
        }

        public void Draw(CanvasStarter g)
        {
            using (var c = g.BeginInGame())
            {
                for (int i = 0; i < labels.Count; i++)
                {
                    var l = labels[i];

                    c.DrawString(mainFont, l.Text, l.Location, l.Color, anchor: AnchorPoint.Center);
                }
            }
        }
    }
}
