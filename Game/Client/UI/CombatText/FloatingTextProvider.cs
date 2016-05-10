using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Message.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;
namespace Shanism.Client.UI.CombatText
{
    enum FloatingTextStyle
    {
        /// <summary>
        /// A floating text that goes up. 
        /// </summary>
        Top,

        /// <summary>
        /// A floating text that goes down. 
        /// </summary>
        Down,

        /// <summary>
        /// A floating text that goes alternately left and right. 
        /// Used for damage events. 
        /// </summary>
        Rainbow,
    }

    class FloatingTextProvider : Control
    {
        static int DefaultDuration = 1000;

        Vector[] textGravities =
        {
            new Vector(0, 10),
            new Vector(0, -10),
            new Vector(0, 10),
        };

        Vector[] textSpeeds =
        {
            new Vector(0, -12),
            new Vector(0, 12),
            new Vector(2.5, -7),
        };

        class TextData
        {
            public FloatingTextStyle Style;

            public Color Color;
            public Vector Location;
            public Vector Velocity;

            public string Text;
            public int Duration;
        }


        int rainbowXDirection = 1;

        readonly HashSet<TextData> labels = new HashSet<TextData>();


        public FloatingTextProvider()
        {
            CanHover = false;
        }


        public void AddDamageLabel(DamageEventMessage msg)
        {
            //get unit position
            var unit = ObjectManager.Default.TryGet(msg.UnitId);
            if (unit == null)
                return;

            var text = msg.ValueChange.ToString("0");
            var labelPos = unit.Position;
            var lblType = FloatingTextStyle.Rainbow;
            var color = Color.Red;

            AddLabel(labelPos, text, color, lblType);
        }

        public void AddLabel(Vector inGamePos, string text, Color c, FloatingTextStyle style)
        {
            var speed = textSpeeds[(int)style];
            if (style == FloatingTextStyle.Rainbow)
            {
                rainbowXDirection = -rainbowXDirection;
                speed *= new Vector(rainbowXDirection, 1);
            }

            var td = new TextData
            {
                Color = c,
                Location = inGamePos,
                Text = text,
                Duration = DefaultDuration,

                Velocity = speed,
                Style = style,
            };
            labels.Add(td);
        }

        protected override void OnUpdate(int msElapsed)
        {
            var toRemove = new List<TextData>();
            foreach (var td in labels)
            {
                td.Duration -= msElapsed;
                if (td.Duration < 0)
                    toRemove.Add(td);
                td.Velocity += textGravities[(int)td.Style] * msElapsed / 1000;
                td.Location += td.Velocity * msElapsed / 1000;
            }

            foreach (var td in toRemove)
                labels.Remove(td);

            Maximize();
        }

        public override void OnDraw(Graphics g)
        {
            foreach (var td in labels)
            {
                var screenPos = Screen.GameToScreen(td.Location);
                Content.Fonts.NormalFont.DrawStringPx(g.SpriteBatch, td.Text, td.Color, screenPos, 0.5f, 0.5f);
            }
        }
    }
}
