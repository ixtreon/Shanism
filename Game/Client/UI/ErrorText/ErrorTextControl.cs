using Shanism.Client.Assets;
using Shanism.Common.Game;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;

namespace Shanism.Client.UI
{
    class ErrorTextControl : Control
    {
        static readonly double TextYStart = 0.3;

        static readonly TextureFont Font = Content.Fonts.NormalFont;

        static readonly int FullTimespan = 5000;
        static readonly int FadeStart = 3000;
        static readonly int BrighterDuration = 500;
        static readonly int MaxErrorCount = 3;

        class ErrorData
        {
            public string Text;
            public int Timespan;
        }

        readonly LinkedList<ErrorData> errors = new LinkedList<ErrorData>();

        public ErrorTextControl()
        {
            CanHover = false;
        }


        public void LogError(string errorText)
        {
            var lastErr = errors.Last?.Value;
            var lastTxt = lastErr?.Text;

            if(lastTxt == errorText)
            {
                lastErr.Timespan = FullTimespan;
                return;
            }

            errors.AddLast(new ErrorData { Text = errorText, Timespan = FullTimespan });
            if (errors.Count > MaxErrorCount)
                errors.RemoveFirst();
        }

        protected override void OnUpdate(int msElapsed)
        {
            foreach (var err in errors)
                err.Timespan -= msElapsed;

            while (errors.Any() && (errors.First.Value.Timespan <= 0))
                errors.RemoveFirst();

            Maximize();
        }

        public override void OnDraw(Graphics g)
        {
            var y = TextYStart;
            foreach(var ed in errors.Reverse())
            {
                var brightness = (int)(Math.Max(0, (double)(BrighterDuration + ed.Timespan - FullTimespan) / BrighterDuration) * 25);
                var alpha = Math.Min(255, 255 * ed.Timespan / FadeStart);
                var c = Color.Red.SetAlpha(alpha).Brighten(brightness);

                g.DrawString(Font, ed.Text, c, new Vector(Size.X / 2, y), 0.5f, 0f);

                y += Font.HeightUi;
            }
        }

    }
}
