using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Content;
using Color = Microsoft.Xna.Framework.Color;
using System.IO;
using Shanism.Common;

namespace Shanism.Client.Drawing
{
    class TextureFont
    {

        readonly SpriteFont Font;

        /// <summary>
        /// The user-defined scale of this font. 
        /// </summary>
        readonly double _userScale;



        /// <summary>
        /// Gets the current scaling factor based on the screen size and the user scaling. 
        /// </summary>
        double Scale => _userScale * Screen.FontScale;

        /// <summary>
        /// Gets the current height of the font in pixels. 
        /// </summary>
        public int HeightPx => (int)(Font.LineSpacing * Scale);

        /// <summary>
        /// Gets the character spacing of the font in pixels. 
        /// </summary>
        public int CharSpacingPx => (int)(Font.Spacing * Scale);

        /// <summary>
        /// Gets the height of the font in UI units. 
        /// </summary>
        public double HeightUi => HeightPx / Screen.UiScale;


        /// <summary>
        /// Gets the char character of this font in UI units. 
        /// </summary>
        public double CharSpacingUi => CharSpacingPx / Screen.UiScale;


        public TextureFont(SpriteFont font, double scale = 1f)
        {
            _userScale = scale;
            Font = font;
        }

        /// <summary>
        /// Draws the given multi-line string at the given screen coordinates. 
        /// </summary>
        /// <param name="sb">The SpriteBatch instance to draw on. </param>
        /// <param name="text">The text to write. </param>
        /// <param name="col">The color of the text. </param>
        /// <param name="p">The screen position to draw the text on. </param>
        /// <param name="xAnchor">The X anchor of the text. </param>
        /// <param name="yAnchor">The Y anchor of the text. </param>
        /// <param name="maxWidth">The maximum width the string is allowed to be. </param>
        /// <returns>The height of the string, in pixels. </returns>
        public void DrawString(SpriteBatch sb, string text,
            Color color, Vector p,
            float xAnchor, float yAnchor,
            double? maxWidth = null)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (maxWidth.HasValue)
                text = getLines(text, maxWidth.Value);

            var sz = Font.MeasureString(text).ToVector() * Scale;
            var drawPos = p - new Vector(sz.X * xAnchor, sz.Y * yAnchor);

            sb.DrawString(Font, text, drawPos.ToVector2(), color,
                0, Microsoft.Xna.Framework.Vector2.Zero,
                (float)Scale, SpriteEffects.None, 0);

        }


        public Vector MeasureStringUi(string text, double? maxWidth = null)
        {
            return MeasureString(text, (maxWidth * Screen.UiScale)) / Screen.UiScale;
        }

        public Vector MeasureString(string text,
            double? maxWidth = null)
        {
            if (string.IsNullOrEmpty(text))
                return Vector.Zero;

            if (maxWidth.HasValue)
                text = getLines(text, maxWidth.Value);

            return Font.MeasureString(text).ToVector() * Scale;
        }


        public double[] GetLineCharsPx(string s)
        {
            if (string.IsNullOrEmpty(s))
                return new double[] { 0 };

            return Enumerable.Range(0, s.Length + 1)
                .Select(i => (double)Font.MeasureString(s.Substring(0, i)).X)
                .ToArray();
        }

        public double[] GetLineCharsUi(string s)
        {
            var ws = GetLineCharsPx(s);

            for (int i = 1; i < ws.Length; i++)
                ws[i] = Screen.ScreenToUi(ws[i]);

            return ws;
        }

        string getLines(string text, double maxWidth)
        {
            var midLen = 2 * Font.Spacing + Font.MeasureString(" ").X;
            var sb = new StringBuilder(text.Length);
            var curLine = new StringBuilder();

            maxWidth /= Scale;

            foreach (var baseLn in text.Split('\n'))
            {
                curLine.Clear();

                foreach (var word in baseLn.Split(' '))
                {
                    if (curLine.Length > 0)
                    {
                        var newWidth = Font.MeasureString($"{curLine} {word}").X; //becomes O(N^2) in here
                        if (newWidth > maxWidth)
                        {
                            sb.Append(curLine);
                            sb.Append('\n');

                            curLine.Clear();
                        }
                        else
                        {
                            curLine.Append(' ');
                        }
                    }

                    curLine.Append(word);
                }

                sb.Append(curLine.ToString());
                sb.Append('\n');
            }

            sb.Length--;    //trim last newline char
            return sb.ToString();
        }
    }
}
