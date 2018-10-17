using System;
using System.Collections.Generic;

namespace Shanism.Client.Text
{

    class StringSplitter
    {

        readonly GlyphMap glyphs;

        readonly List<CharInfo> wordBuffer = new List<CharInfo>(16);
        readonly List<CharInfo> lineBuffer = new List<CharInfo>(256);

        readonly List<CharInfo> characters = new List<CharInfo>();
        readonly List<LineInfo> lines = new List<LineInfo>();

        public int TabWidth { get; set; } = 4;

        /// <summary>
        /// Gets the resulting character buffer.
        /// </summary>
        public IReadOnlyList<CharInfo> Characters => characters;

        /// <summary>
        /// Gets the resulting line buffer.
        /// </summary>
        public IReadOnlyList<LineInfo> Lines => lines;


        public StringSplitter(GlyphMap glyphs)
        {
            this.glyphs = glyphs;
        }

        public void GetCharacterOffsets(float pixelToUiScale, string inputText, float? maxWidthPx)
        {
            GetCharacterOffsetsPx(inputText, maxWidthPx / pixelToUiScale);

            for (int i = 0; i < Lines.Count; i++)
                lines[i] = lines[i].WithScale(pixelToUiScale);

            for (int i = 0; i < characters.Count; i++)
                characters[i] = characters[i].WithScale(pixelToUiScale);
        }

        /// <summary>
        /// Splits a string into lines for drawing.
        /// </summary>
        public void GetCharacterOffsetsPx(string inputText, float? maxWidthPx)
        {
            characters.Clear();
            lines.Clear();

            if (string.IsNullOrEmpty(inputText))
                return;

            // consts
            var charSpacing = glyphs.CharSpacing;
            var wordSpacing = 2 * charSpacing + glyphs.Get(' ').TotalWidth;

            // state
            var curWord = new LineBuffer { Chars = wordBuffer };
            var curLine = new LineBuffer { Chars = lineBuffer };
            bool lastWasSpace = false;

            // FSM much?
            for (int i = 0; i < inputText.Length; i++)
            {
                var c = inputText[i];
                switch (c)
                {
                    case '\r':
                        //ignored
                        break;

                    case '\n':

                        if (curWord.Chars.Count > 0)
                            appendCurWord();

                        appendCurLine();
                        break;

                    case '\t':
                        appendCurWord();
                        while (curLine.Chars.Count % TabWidth != 0)
                            append(ref curLine, ' ');
                        break;


                    case ' ':
                        appendCurWord();
                        break;

                    default:
                        append(ref curWord, c);
                        break;
                }
            }

            // finally, copy the remainder over
            if (curWord.Chars.Count > 0)
                appendCurWord();

            if (curLine.Chars.Count > 0)
                appendCurLine();


            void appendCurWord()
            {
                // if adding a word breaks max-width, start a new line
                var isNewLine = curLine.Chars.Count == 0;
                var makeNewLine = !isNewLine && (curLine.Width + wordSpacing + curWord.Width > maxWidthPx);
                if (makeNewLine)
                    appendCurLine();

                // add a spacebar unless its a first word (but not spaces-only)
                isNewLine = isNewLine || makeNewLine;
                var spaceOnly = curWord.Chars.Count == 0;
                if (spaceOnly || (!isNewLine && !lastWasSpace))
                    append(ref curLine, ' ');
                lastWasSpace = spaceOnly;

                // add pending word to pending line
                for (int i = 0; i < curWord.Chars.Count; i++)
                    append(ref curLine, curWord.Chars[i].Character);

                // then clear pending word
                curWord.Chars.Clear();
                curWord.Width = 0;
            }

            void appendCurLine()
            {

                AppendLine(curLine.Chars, curLine.Width);

                curLine.Chars.Clear();
                curLine.Width = 0;
            }

            void append(ref LineBuffer l, char c)
            {
                var charInfo = new CharInfo(c, glyphs.Get(c).TotalWidth);
                l.Chars.Add(charInfo);
                l.Width += charSpacing + charInfo.Width;
            }
        }

        void AppendLine(List<CharInfo> chars, float width)
        {
            var start = characters.Count;
            {
                characters.AddRange(chars);
            }
            var end = characters.Count;

            lines.Add(new LineInfo(start, end, width));
        }

        struct LineBuffer
        {
            public List<CharInfo> Chars;
            public float Width;
        }

    }

    struct LineInfo
    { 
        public LineInfo(int start, int count, float width)
        {
            Start = start;
            End = count;
            Width = width;
        }

        public int Start { get; }
        public int End { get; }
        public float Width { get; }

        public LineInfo WithScale(float scalingFactor)
            => new LineInfo(Start, End, Width * scalingFactor);
    }

    struct TextRenderInfo
    {

        public List<CharInfo> CharBuffer { get; }

        public List<LineInfo> Lines { get; }


        public TextRenderInfo(List<CharInfo> charBuffer, List<LineInfo> lines)
        {
            CharBuffer = charBuffer;
            Lines = lines;
        }


        public float GetMaxLineWidth()
        {
            var max = 0f;
            for (int i = 0; i < Lines.Count; i++)
                max = Math.Max(max, Lines[i].Width);
            return max;
        }

        internal void AppendLine(List<CharInfo> chars, float width)
        {
            var start = CharBuffer.Count;
            {
                CharBuffer.AddRange(chars);
            }
            var end = CharBuffer.Count;

            Lines.Add(new LineInfo(start, end, width));
        }

        public void RescaleContents(float scalingFactor)
        {
            for (int i = 0; i < Lines.Count; i++)
                Lines[i] = Lines[i].WithScale(scalingFactor);

            for (int i = 0; i < CharBuffer.Count; i++)
                CharBuffer[i] = CharBuffer[i].WithScale(scalingFactor);
        }

        public void Clear()
        {
            CharBuffer.Clear();
            Lines.Clear();
        }

        //public IEnumerable<(float Width, IEnumerable<CharInfo> Characters)> EnumerateLines()
        //{
        //    var charID = 0;
        //    var chars = CharBuffer;

        //    IEnumerable<CharInfo> nextLine()
        //    {
        //        for (; charID < chars.Count; charID++)
        //        {
        //            yield return chars[charID];

        //            if (chars[charID].Character == '\n')
        //            {
        //                charID++;
        //                yield break;
        //            }
        //        }
        //    }

        //    for (int l = 0; l < LineWidths.Count; l++)
        //        yield return (LineWidths[l], nextLine());
        //}
    }

    public struct CharInfo
    {
        public static CharInfo LineBreak { get; } = new CharInfo { Character = '\n' };

        public char Character { get; set; }

        public float Width { get; set; }

        public CharInfo(char character, float width)
        {
            Character = character;
            Width = width;
        }

        public CharInfo WithScale(float scalingFactor)
            => new CharInfo(Character, Width * scalingFactor);

    }

}
