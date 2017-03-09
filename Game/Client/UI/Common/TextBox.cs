using Shanism.Client.Input;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;
using Shanism.Client.Drawing;

namespace Shanism.Client.UI
{
    class TextBox : Control
    {
        const int CursorBlinkRate = 1000;


        int _cursorPosition;
        int _selectionLength;
        string _currentText;
        int cursorBlink;
        double[] textPositions;


        public TextureFont Font { get; set; }

        public Color ForeColor { get; set; }

        public Color CursorColor { get; set; }

        public Color SelectionColor { get; set; }


        public string Text
        {
            get { return _currentText; }
            set
            {
                if (value != _currentText)
                {
                    if (value == null)
                        value = string.Empty;

                    setText(value);

                    _cursorPosition = value.Length;
                    _selectionLength = 0;
                }
            }
        }

        public int SelectionLength
        {
            get { return _selectionLength; }
            private set { _selectionLength = value.Clamp(0, Text.Length - _cursorPosition); }
        }

        public bool IsCursorVisible
            => HasFocus && (cursorBlink < CursorBlinkRate / 2);

        Vector CursorSize
            => new Vector(Font.HeightUi / 10, Font.HeightUi);



        public TextBox()
        {
            Font = Content.Fonts.NormalFont;
            Text = string.Empty;
            CanFocus = true;
            Size = new Vector(0.5, Font.HeightUi + 2 * Padding);
            CursorColor = Color.LightBlue;
            ForeColor = Color.LightBlue;
            SelectionColor = Color.White.SetAlpha(150);


            KeyboardInfo.CharacterTyped += OnCharTyped;
        }

        protected override void OnUpdate(int msElapsed)
        {
            //update cursor
            if (HasFocus)
                cursorBlink = (cursorBlink + msElapsed) % CursorBlinkRate;

            base.OnUpdate(msElapsed);
        }

        protected virtual void OnCharTyped(Keybind k, char? c)
        {
            switch (k.Key)
            {
                case Keys.Back:     // {Backspace} to delete previous char or selection
                    if (SelectionLength != 0)
                        DeleteSelection();
                    else
                        deletePrevCharacter();
                    break;

                case Keys.Delete:   // {Delete} to delete next char or selection
                    if (SelectionLength != 0)
                        DeleteSelection();
                    else
                        deleteNextCharacter();
                    break;

                case Keys.Left:     // {Left}, {Right} to move cursor, select text
                    if (_cursorPosition > 0)
                    {
                        if (k.Shift)
                            _selectionLength++;
                        else
                            _selectionLength = 0;

                        _cursorPosition--;
                    }
                    break;

                case Keys.Right:
                    if (_cursorPosition < Text.Length)
                    {
                        if (k.Shift)
                            _selectionLength--;
                        else
                            _selectionLength = 0;

                        _cursorPosition++;
                    }
                    break;


                case Keys.A:            // {Ctrl+A} selects everything
                    if (!k.Control)
                        goto default;   // but {A} is a normal char

                    SelectAllText();
                    break;


                case Keys.Escape:
                    ClearFocus();
                    break;

                default:
                    if (c != null)
                        writeChar(c.Value);
                    break;
            }
        }

        void writeChar(char c)
        {
            if (SelectionLength != 0)   //overwrite selection
                DeleteSelection();

            setText(Text.Insert(_cursorPosition, c.ToString()));
            _cursorPosition++;
        }

        public void SelectAllText()
        {
            _cursorPosition = Text.Length;
            _selectionLength = -Text.Length;
        }

        void deleteNextCharacter()
        {
            //if not at last char, delete next char
            if (_cursorPosition < Text.Length)
            {
                setText(Text.Remove(_cursorPosition, 1));
            }
        }

        void deletePrevCharacter()
        {
            //if not the first char, delete prev char
            if (_cursorPosition > 0)
            {
                _cursorPosition--;
                setText(Text.Remove(_cursorPosition, 1));
            }
        }

        public void DeleteSelection()
        {
            if (SelectionLength < 0)
            {
                setText(Text.Remove(_cursorPosition + SelectionLength, -SelectionLength));
                _cursorPosition += SelectionLength;
            }
            else if (SelectionLength > 0)
            {
                setText(Text.Remove(_cursorPosition, SelectionLength));
            }
            SelectionLength = 0;
        }

        public void ClearText()
        {
            _currentText = string.Empty;
            _selectionLength = 0;
            _cursorPosition = 0;
        }

        void setText(string value)
        {
            _currentText = value;
            textPositions = Font.GetCharPositions(value);
        }

        public override void OnDraw(Canvas g)
        {
            //background
            base.OnDraw(g);

            //selection
            if (SelectionLength != 0)
            {
                var startX = textPositions[_cursorPosition];
                var endX = textPositions[_cursorPosition + SelectionLength];

                if (SelectionLength < 0)
                {
                    var tmp = startX;
                    startX = endX;
                    endX = tmp;
                }

                var drawPos = new Vector(startX + Padding, Size.Y - CursorSize.Y- Padding);
                var drawSize = new Vector(endX - startX, CursorSize.Y);

                g.Draw(Content.Textures.Blank, drawPos, drawSize, SelectionColor);
            }

            //text
            var lastTextChar = Padding + textPositions.Last();
            var lastPossiblePos = Size.X - 2 * Padding;
            if (lastTextChar < lastPossiblePos)
            {
                //draw the whole string
                g.DrawString(Font, Text, ForeColor, new Vector(Padding), 0, 0);
            }
            else
            {
                //draw partial string
                var diff = lastTextChar - lastPossiblePos;
                var trimmedChars = textPositions.TakeWhile(p => p < diff).Count();

                var firstCharPos = (textPositions[trimmedChars] - diff);
                g.DrawString(Font, Text.Substring(trimmedChars), ForeColor, new Vector(firstCharPos, Padding), 0, 0);
            }

            //cursor
            if (IsCursorVisible)
            {
                var pos = new Vector((float)textPositions[_cursorPosition], 0) + Padding;
                g.Draw(Content.Textures.Blank, pos, CursorSize, CursorColor);
            }
        }
    }
}
