using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace ShanoRpgWinGl
{
    class TextureFont
    {
        Rectangle[] keys = new Rectangle[256];

        /// <summary>
        /// The base height of the font. 
        /// </summary>
        private readonly int _baseHeight;

        /// <summary>
        /// The user-defined scale of this font. 
        /// </summary>
        private readonly double _baseScale;

        /// <summary>
        /// The base character spacing of this font. 
        /// </summary>
        private readonly double _charSpacing = 2;

        /// <summary>
        /// Gets the height of the font in pixels. 
        /// </summary>
        public double Height
        {
            get { return _baseHeight * Scale; }
        }

        /// <summary>
        /// Gets the current scaling factor based on the screen size and the user scaling. 
        /// </summary>
        private double Scale
        {
            get {  return _baseScale * Screen.FontScale; }
        }

        /// <summary>
        /// Gets the character spacing of the font in pixels. 
        /// </summary>
        public int CharacterSpacing
        {
            get { return (int)(_charSpacing * Scale); }
        }

        public readonly Texture2D Texture;

        public TextureFont(ContentManager content, string name, double scale = 1f, double characterSpacing = 2)
        {
            this._charSpacing = characterSpacing;
            this._baseScale = scale;
            this.Texture = content.Load<Texture2D>(name);

            var xmlSchema = new XmlDocument();
            xmlSchema.Load("Content\\" + name + ".xml");

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

        public TextureFont(TextureFont f, float relativeScale = 1f)
        {
            this.Texture = f.Texture;
            this._baseScale = f._baseScale * relativeScale;

            this.keys = f.keys.Clone() as Rectangle[];
        }


        /// <summary>
        /// Draws the given multi-line string. 
        /// </summary>
        /// <param name="sb">The SpriteBatch instance to draw on. </param>
        /// <param name="text">The text to write. </param>
        /// <param name="col">The color of the text. </param>
        /// <param name="p">The position to draw the text on. </param>
        /// <param name="xAnchor">The X anchor of the text. </param>
        /// <param name="yAnchor">The Y anchor of the text. </param>
        /// <param name="maxWidth">The maximum width the string is allowed to be. </param>
        /// <returns>The height of the string. </returns>
        public int DrawString(SpriteBatch sb, string text, 
            Color color, Point p, 
            float xAnchor = 0.0f, float yAnchor = 0.5f,
            int maxWidth = int.MaxValue)
        {
            //split the string into lines, and anchor vertically
            var lines = getLines(text, maxWidth).ToArray();
            p.Y -= (int)(Height * yAnchor * lines.Length);
            foreach (var ln in lines)
            {
                // get the line width and anchor horizontally
                var lnWidth = getWidth(ln);
                var x = p.X - (int)(lnWidth * xAnchor);
                // draw the lines
                foreach (var c in ln)
                {
                    DrawChar(sb, c, color, x, p.Y);
                    x += this.CharacterSpacing + (int)(keys[c].Width * Scale);
                }
                // move to a new line
                p.Y += (int)Height;
            }
            return 0;
        }

        public Point MeasureString(string text, int maxWidth = int.MaxValue)
        {
            var lines = getLines(text, maxWidth);
            return new Point(lines.Max(l => getWidth(l)), (int)(Height * lines.Count()));
        }

        /// <summary>
        /// Gets the width of the given one-line (!) string in pixels. 
        /// </summary>
        /// <param name="s"></param>
        private int getWidth(string s)
        {
            var len = 0;
            foreach (var c in s)
                len += keys[c].Width;
            return (int)(len * Scale) + CharacterSpacing * (s.Length - 1);
        }

        /// <summary>
        /// Splits the given string into lines, optionally making each line shorter than the specified maximum width in pixels. 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxWidth"></param>
        private IEnumerable<string> getLines(string text, int maxWidth = int.MaxValue)
        {
            var width = 0;
            var lineStart = 0;
            for (int i = 0; i < text.Length; i++)
            {
                //grab the char and its width
                var currentChar = text[i];
                var charWidth = getCharSize(currentChar).X;
                //determine whether to go to a new line
                if (currentChar == '\n' || (width + charWidth > maxWidth && currentChar == ' '))
                {
                    yield return text.Substring(lineStart, i - lineStart); //tricky substrings!
                    //update them counters
                    //if (currentChar == '\n')
                    //{
                        lineStart = i + 1;
                        width = 0;
                    //}
                    //else
                    //{
                    //    lineStart = i;
                    //    width = charWidth;
                    //}
                }
                else
                    width += charWidth;
            }
            yield return text.Substring(lineStart, text.Length - lineStart);  //ugh
        }
        
        public void DrawChar(SpriteBatch sb, char c, Color col, int x, int y)
        {
            var sz = getCharSize(c);

            sb.Draw(Texture, new Rectangle(x, y, sz.X, sz.Y), keys[c], col);
        }

        /// <summary>
        /// Unspecified behaviour for '\n'!
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private Point getCharSize(char c)
        {
            if (keys[c] == Rectangle.Empty && c != '\n')
                throw new ArgumentOutOfRangeException("Character '" + c + "' does not exist in the current font. ");
            return new Point((int)(Scale * keys[c].Width), (int)(Scale * keys[c].Height));
        }

        private int parseInt(string i)
        {
            int id = -1;
            int.TryParse(i, out id);
            return id;
        }
    }
}
