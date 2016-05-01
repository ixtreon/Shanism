using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Content;
using IO.Common;
using Color = Microsoft.Xna.Framework.Color;
using System.IO;

namespace Client.Assets
{
    class TextureFont
    {
        Rectangle[] keys = new Rectangle[256];

        /// <summary>
        /// The base height of the font in pixels. 
        /// </summary>
        readonly int _baseHeight;

        /// <summary>
        /// The base character spacing of this font in pixels. 
        /// </summary>
        readonly double _baseCharSpacing = 2;

        /// <summary>
        /// The user-defined scale of this font. 
        /// </summary>
        readonly double _userScale;


        public readonly Texture2D Texture;


        /// <summary>
        /// Gets the current scaling factor based on the screen size and the user scaling. 
        /// </summary>
        double Scale => _userScale * Screen.FontScale;

        /// <summary>
        /// Gets the current height of the font in pixels. 
        /// </summary>
        public int HeightPx => (int)(_baseHeight * Scale);

        /// <summary>
        /// Gets the character spacing of the font in pixels. 
        /// </summary>
        public int CharSpacingPx => (int)(_baseCharSpacing * Scale);

        /// <summary>
        /// Gets the height of the font in UI units. 
        /// </summary>
        public double HeightUi => HeightPx / Screen.UiScale;


        /// <summary>
        /// Gets the char character of this font in UI units. 
        /// </summary>
        public double CharSpacingUi => CharSpacingPx / Screen.UiScale; 


        public TextureFont(ContentManager content, string name, double scale = 1f, double characterSpacing = 2)
        {
            _baseCharSpacing = characterSpacing;
            _userScale = scale;
            Texture = content.Load<Texture2D>(name);

            var xmlSchema = new XmlDocument();
            xmlSchema.Load(Path.Combine(Content.DefaultContentDirectory, name + ".xml"));

            foreach(XmlNode node in xmlSchema.SelectNodes("font/chars/char"))
            {
                //var sId = node.Attributes.GetNamedItem("key").Value;

                //var id = parseInt(sId);
                var id = parseInt(node.Attributes.GetNamedItem("id").InnerText);
                var x = parseInt(node.Attributes.GetNamedItem("x").InnerText);
                var y = parseInt(node.Attributes.GetNamedItem("y").InnerText);
                var w = parseInt(node.Attributes.GetNamedItem("width").InnerText);
                var h = parseInt(node.Attributes.GetNamedItem("height").InnerText);
                if(x == -1 || y == -1 || w == -1 || h == -1 || id == -1)
                    continue;

                keys[id] = new Rectangle(x, y + 1, w, h - 1);
                _baseHeight = Math.Max(_baseHeight, w);
            }
        }

        public TextureFont(TextureFont f, double relativeScale = 1)
        {
            this.Texture = f.Texture;
            this._userScale = f._userScale * relativeScale;
            this._baseHeight = f._baseHeight;
            this._baseCharSpacing = f._baseCharSpacing;

            this.keys = (Rectangle[])f.keys.Clone();
        }

        /// <summary>
        /// Draws the given multi-line string at the given UI coordinates. 
        /// </summary>
        /// <param name="sb">The SpriteBatch instance to draw on. </param>
        /// <param name="text">The text to write. </param>
        /// <param name="col">The color of the text. </param>
        /// <param name="p">The UI location to draw the text on. </param>
        /// <param name="xAnchor">The X anchor of the text. 0 is left, 1 is right, 0.5 is middle.  </param>
        /// <param name="yAnchor">The Y anchor of the text. 0 is top, 1 is bottom, 0.5 is middle. </param>
        /// <param name="maxWidth">The maximum width the string is allowed to be. </param>
        /// <returns>The height of the string, in pixels. </returns>
        public int DrawStringUi(SpriteBatch sb, string text, 
            Color color, Vector pos, 
            float xAnchor, float yAnchor,
            double? maxValue = null)
        {
            var screenPos = Screen.UiToScreen(pos);
            return DrawStringPx(sb, text, color, screenPos, xAnchor, yAnchor, (maxValue * Screen.UiScale));
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
        public int DrawStringPx(SpriteBatch sb, string text, 
            Color color, Vector p, 
            float xAnchor, float yAnchor,
            double? maxWidth = null)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            //split the string into lines, and anchor vertically
            var lines = getLines(text, maxWidth ?? double.MaxValue).ToArray();
            var y = p.Y - (HeightPx * yAnchor * lines.Length);

            foreach (var ln in lines)
            {
                // get the line width and anchor horizontally
                var lnWidth = getLineWidth(ln);
                var x = p.X - (lnWidth * xAnchor);

                // draw the lines
                foreach (var c in ln.Where(c => c > 0 && c < 256))
                {
                    drawChar(sb, c, color, new Vector(x, y));
                    x += CharSpacingPx + (keys[c].Width * Scale);
                }

                // move to a new line
                y += HeightPx;
            }
            return 0;
        }

        public Vector MeasureStringUi(string text = "WOW LOOK AT THAT STRING! gj fi dat!", double maxWidth = 5000)
        {
            var p = MeasureString(text, (maxWidth * Screen.UiScale));
            return new Vector(p.X / Screen.UiScale, p.Y / Screen.UiScale);
        }

        public Vector MeasureString(string text, double maxWidth = double.MaxValue)
        {
            if (string.IsNullOrEmpty(text))
                return Vector.Zero;
            var lines = getLines(text, maxWidth);
            var w = lines.Any() ? lines.Max(l => getLineWidth(l)) : 0;
            var h = HeightPx * lines.Count();
            return new Vector(w, h);
        }

        /// <summary>
        /// Gets the width of the given one-line (!) string in pixels. 
        /// </summary>
        /// <param name="s"></param>
        double getLineWidth(string s)
        {
            return (s.Sum(c => keys[c].Width) * Scale) + (CharSpacingPx * (s.Length - 1));
        }


        public double[] GetLineCharsPx(string s)
        {
            if (string.IsNullOrEmpty(s))
                return new[] { 0.0 };

            var ws = new double[s.Length + 1];
            ws[0] = 0;
            ws[1] = Scale * keys[s[0]].Width;

            for (int i = 1; i < s.Length; i++)
                ws[i + 1] = ws[i] + CharSpacingPx + (Scale * keys[s[i]].Width);

            return ws;
        }

        public double[] GetLineCharsUi(string s)
        {
            var ws = GetLineCharsPx(s);

            for (int i = 1; i < ws.Length; i++)
                ws[i] = Screen.ScreenToUi(ws[i]);

            return ws;
        }



        /// <summary>
        /// Splits the given string into lines, optionally making each line shorter than the specified maximum width in pixels. 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxWidth"></param>
        IEnumerable<string> getLines(string text, double maxWidth = double.MaxValue)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var curChar = text[0];
                var charWidth = getCharSize(curChar).X;
                var currentWidth = charWidth;
                var newWidth = currentWidth;

                var lastCut = 0;
                var lastWordBreak = 0;

                for (int i = 1; i < text.Length; i++)
                {
                    //grab the char and its width
                    curChar = text[i];
                    charWidth = getCharSize(curChar).X;
                    newWidth = currentWidth + CharSpacingPx + charWidth;

                    //save spacebars
                    if (char.IsWhiteSpace(curChar))
                        lastWordBreak = i;

                    //determine whether to go to a new line
                    //always write at least one word though
                    if (curChar == '\n' || (newWidth > maxWidth && lastWordBreak > lastCut))
                    {
                        yield return text.Substring(lastCut, lastWordBreak - lastCut);
                        lastCut = lastWordBreak + 1;

                        currentWidth = charWidth;
                    }
                    else
                        currentWidth = newWidth;
                }
                yield return text.Substring(lastCut, text.Length - lastCut);  //ugh
            }
        }
        
        void drawChar(SpriteBatch sb, char c, Color col, Vector pos)
        {
            if (c < 0 || c > 255)
                return;

            var sz = getCharSize(c);

            sb.ShanoDraw(Texture, keys[c], pos, sz, col);
        }

        /// <summary>
        /// Unspecified behaviour for '\n'!
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private Vector getCharSize(char c)
        {
            if (c < 0 || c > 255 || (keys[c] == Rectangle.Empty && c != '\n'))
                return Vector.Zero;

            return (Vector)keys[c].Size * Scale;
        }

        private int parseInt(string i)
        {
            var id = -1;
            int.TryParse(i, out id);
            return id;
        }
    }
}
