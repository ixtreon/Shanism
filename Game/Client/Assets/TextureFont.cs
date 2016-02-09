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
        /// The base height of the font. 
        /// </summary>
        readonly int _baseHeight;

        /// <summary>
        /// The base character spacing of this font. 
        /// </summary>
        readonly double _baseCharSpacing = 2;

        /// <summary>
        /// The user-defined scale of this font. 
        /// </summary>
        readonly double _fontScale;




        public int ScreenHeight
        {
            get { return (int)(_baseHeight * Scale); }
        }


        /// <summary>
        /// Gets the character spacing of the font in pixels. 
        /// </summary>
        public int ScreenSpacing
        {
            get { return (int)(_baseCharSpacing * Scale); }
        }


        /// <summary>
        /// Gets the height of the font in UI units. 
        /// </summary>
        public double UiHeight
        {
            get { return ScreenHeight / Screen.UiScale; }
        }

        /// <summary>
        /// Gets the char spacing of this font in UI units. 
        /// </summary>
        public double UiCharSpacing
        {
            get { return ScreenSpacing / Screen.UiScale; }
        }

        /// <summary>
        /// Gets the current scaling factor based on the screen size and the user scaling. 
        /// </summary>
        double Scale
        {
            get {  return _fontScale * Screen.FontScale; }
        }

        public readonly Texture2D Texture;

        public TextureFont(ContentManager content, string name, double scale = 1f, double characterSpacing = 2)
        {
            this._baseCharSpacing = characterSpacing;
            this._fontScale = scale;
            this.Texture = content.Load<Texture2D>(name);

            var xmlSchema = new XmlDocument();
            xmlSchema.Load(Path.Combine(Content.DefaultTexDir, name + ".xml"));

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
            this._fontScale = f._fontScale * relativeScale;
            this._baseHeight = f._baseHeight;
            this._baseCharSpacing = f._baseCharSpacing;

            this.keys = f.keys.Clone() as Rectangle[];
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
        public int DrawString(SpriteBatch sb, string text, 
            Color color, Vector pos, 
            float xAnchor, float yAnchor,
            double? maxValue = null)
        {
            var screenPos = Screen.UiToScreen(pos);
            return DrawStringScreen(sb, text, color, screenPos, xAnchor, yAnchor, (int?)(maxValue * Screen.UiScale));
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
        public int DrawStringScreen(SpriteBatch sb, string text, 
            Color color, Vector p, 
            float xAnchor, float yAnchor,
            int? maxWidth = null)
        {
            //split the string into lines, and anchor vertically
            var lines = getLines(text, maxWidth ?? int.MaxValue).ToArray();
            p.Y -= (int)(ScreenHeight * yAnchor * lines.Length);
            foreach (var ln in lines)
            {
                // get the line width and anchor horizontally
                var lnWidth = getWidth(ln);
                var x = p.X - (int)(lnWidth * xAnchor);
                // draw the lines
                foreach (var c in ln.Where(c => c > 0 && c < 256))
                {
                    DrawChar(sb, c, color, new Vector(x, p.Y));
                    x += ScreenSpacing + (int)(keys[c].Width * Scale);
                }
                // move to a new line
                p.Y += (int)ScreenHeight;
            }
            return 0;
        }

        public Point MeasureString(string text, int maxWidth = int.MaxValue)
        {
            if (string.IsNullOrEmpty(text))
                return Point.Zero;
            var lines = getLines(text, maxWidth);
            var w = lines.Max(l => getWidth(l));
            var h = ScreenHeight * lines.Count();
            return new Point(w, h);
        }

        public Vector MeasureStringUi(string text = "WOW LOOK AT THAT STRING! gj fi dat!", double maxWidth = 5000)
        {
            var p = MeasureString(text, (int)(maxWidth * Screen.UiScale));
            return new Vector(Screen.ScreenToUi(p.X), Screen.ScreenToUi(p.Y));
        }

        /// <summary>
        /// Gets the width of the given one-line (!) string in pixels. 
        /// </summary>
        /// <param name="s"></param>
        private int getWidth(string s)
        {
            return (int)(s.Where(c => c > 0 && c < 256).Sum(c => keys[c].Width) * Scale + ScreenSpacing * (s.Length - 1));
        }

        /// <summary>
        /// Returns an array specifying the position of each of the given string's characters
        /// when drawn using this font. 
        /// </summary>
        /// <param name="s"></param>
        /// <returns>The position where the i'th character is drawn. </returns>
        public int[] GetLengths(string s)
        {
            if(string.IsNullOrEmpty(s))
                return new [] { 0 };

            var ws = new int[s.Length + 1];
            ws[0] = 0;
            for (int i = 0; i < s.Length; i++)
                ws[i + 1] = ws[i] + ScreenSpacing + (int)(Scale * keys[s[i]].Width);

            return ws;
        }
        
        public double[] GetLengthsUi(string s)
        {
            if (string.IsNullOrEmpty(s))
                return new[] { 0.0 };

            var ws = new double[s.Length + 1];
            ws[0] = 0;
            for (int i = 0; i < s.Length; i++)
                ws[i + 1] = ws[i] + Screen.ScreenToUi(ScreenSpacing + (int)(Scale * keys[s[i]].Width));

            return ws;
        }

        /// <summary>
        /// Splits the given string into lines, optionally making each line shorter than the specified maximum width in pixels. 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxWidth"></param>
        private IEnumerable<string> getLines(string text, int maxWidth = int.MaxValue)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var lineWidth = 0.0;
                var lineStart = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    //grab the char and its width
                    var c = text[i];
                    var cWidth = getCharSize(c).X;
                    var newWidth = lineWidth + cWidth;
                    //determine whether to go to a new line
                    if (c == '\n' || (newWidth > maxWidth && c == ' '))
                    {
                        yield return text.Substring(lineStart, i - lineStart);
                        lineStart = i + 1;
                        lineWidth = cWidth;
                    }
                    else
                        lineWidth = newWidth;
                }
                yield return text.Substring(lineStart, text.Length - lineStart);  //ugh
            }
        }
        
        public void DrawChar(SpriteBatch sb, char c, Color col, Vector pos)
        {
            var sz = getCharSize(c);

            if (c < 0 || c > 255)
                return;

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
            int id = -1;
            int.TryParse(i, out id);
            return id;
        }
    }
}
