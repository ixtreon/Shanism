using Shanism.Client.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;

namespace Shanism.Client.UI.Game
{
    class ErrorTextControl : Control
    {
        static readonly double TextYStart = 0.3;


        static readonly int FullTimespan = 5000;
        static readonly int FadeStart = 3000;
        static readonly int MaxErrorCount = 3;

        class ErrorData
        {
            public string Text;
            public int Timespan;
        }

        readonly LinkedList<ErrorData> errors = new LinkedList<ErrorData>();

        TextureFont Font => Content.Fonts.NormalFont;

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

        public override void OnDraw(Canvas g)
        {
            var currentY = TextYStart;
            foreach(var ed in errors.Reverse())
            {
                var alpha = (byte)Math.Min(255, 255 * ed.Timespan / FadeStart);
                var c = Color.Red.SetAlpha(alpha);

                g.DrawString(Font, ed.Text, c, new Vector(Size.X / 2, currentY), 0.5f, 0f);

                currentY += Font.HeightUi;
            }
        }

    }
}
