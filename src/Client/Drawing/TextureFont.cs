using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Color = Microsoft.Xna.Framework.Color;
using Shanism.Common;
using System.Text.RegularExpressions;

namespace Shanism.Client.Drawing
{
    class TextureFont
    {

        internal readonly SpriteFont Font;

        //matches characters NOT part of this font.
        readonly Regex invalidCharFinder;

        /// <summary>
        /// The user-defined scale of this font. 
        /// </summary>
        public double UserScale { get; }

        /// <summary>
        /// Gets the line spacing (the distance from baseline to baseline)
        /// of this font in pixels.
        /// </summary>
        public double LineSpacing => Font.LineSpacing;
        
        /// <summary>
        /// Gets the height of the font in UI units. 
        /// </summary>
        public double HeightUi => UserScale;

        public double pixelToUiz { get; }

        public TextureFont(SpriteFont font, double scale = 1f)
        {
            UserScale = scale;
            Font = font;

            pixelToUiz = scale / font.LineSpacing;

            var recognizedCharacters = new string(font.Characters.ToArray());
            invalidCharFinder = new Regex($"[^{Regex.Escape(recognizedCharacters)}]");
        }


        /// UI string sizes

        //char position breakdown
        public double[] GetCharPositions(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new[] { 0.0 };

            var vals = new double[text.Length + 1];
            for (int i = 1; i <= text.Length; i++)
                vals[i] = Font.MeasureString(text.Substring(0, i)).X * pixelToUiz;

            return vals;
        }

        // measure no max
        public Vector MeasureString(string text)
        {
            return Font.MeasureString(text).ToVector() * pixelToUiz;
        }


        //split lines
        public string SplitLines(string text, double maxWidth)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var midLen = 2 * Font.Spacing + Font.MeasureString(" ").X;
            var sb = new StringBuilder(text.Length);
            var curLine = new StringBuilder();

            text = removeInvalidChars(text);

            var lines = text.Split('\n');
            for(int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var words = line.Split(' ');
                for(int j = 0; j < words.Length; j++)
                {
                    var word = words[j];
                    if (curLine.Length > 0)
                    {
                        var newWidth = Font.MeasureString($"{curLine} {word}").X * pixelToUiz; //method becomes O(N^2) in here

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

                curLine.Clear();
            }

            sb.Length--;    //trim last newline char
            return sb.ToString();
        }

        //measure with max
        public Vector MeasureString(string text, double maxWidth)
            => MeasureString(SplitLines(text, maxWidth));


        /// Px string sizes

        // measure no max
        public Vector MeasureStringPx(Screen s, string text)
            => MeasureString(text) * s.UiScale;
        //split lines
        public string SplitLinesPx(Screen s, string text, double maxWidthPx)
            => SplitLines(text, maxWidthPx / s.UiScale);
        // measure with max
        public Vector MeasureStringPx(Screen s, string text, double maxWidthPx)
            => MeasureString(text, maxWidthPx / s.UiScale) * s.UiScale;


        string removeInvalidChars(string s) 
            => invalidCharFinder.Replace(s, string.Empty);
    }
}
