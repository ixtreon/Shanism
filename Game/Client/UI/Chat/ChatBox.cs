using Client.Input;
using Client.Textures;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using IO.Common;
using Color = Microsoft.Xna.Framework.Color;
using Client.Assets;

namespace Client.UI
{
    class ChatBox : Control
    {

        const int CursorBlinkRate = 1000;

        private int _cursorPosition;
        private int _selectionLength;
        private string _currentText;
        
        private int cursorBlink;

        double[] textPositions;


        public TextureFont Font { get; set; }

        public Color ForeColor { get; set; }
        public Color CursorColor { get; set; }
        public Color SelectionColor { get; set; }
        
        public bool IsWriting { get; private set; }

        public string CurrentText
        {
            get { return _currentText; }
            set
            {
                if(value != _currentText)
                {

                    _currentText = value;
                    textPositions = Font.GetLengthsUi(value);
                }
            }
        }

        public bool CursorVisible
        {
            get { return IsWriting && (cursorBlink < CursorBlinkRate / 2); }
        }

        public int CursorPosition
        {
            get
            {
                return _cursorPosition;
            }
            set
            {
                _cursorPosition = value.Clamp(0, CurrentText.Length);
            }
        }

        public int SelectionLength
        {
            get
            {
                return _selectionLength;
            }
            set
            {
                _selectionLength = value.Clamp(0, CurrentText.Length - CursorPosition);
            }
        }


        public ChatBox()
        {
            Font = Content.Fonts.NormalFont;
            CurrentText = string.Empty;
            KeyboardInfo.ChatProvider.CharPressed += ChatProvider_CharPressed;

            BackColor = Color.Black.SetAlpha(50);
            CursorColor = Color.White;
            ForeColor = Color.White;
            SelectionColor = Color.LightBlue.SetAlpha(5);
        }

        void ChatProvider_CharPressed(Keys k, char c)
        {
            if (!IsWriting)
                return;

            switch(k)
            {
                case Keys.Back:
                    if(SelectionLength != 0)
                        removeSelection();
                    else if(CursorPosition > 0)
                    {
                        CursorPosition--;
                        CurrentText = CurrentText.CutOut(CursorPosition, 1);
                    }
                    break;

                case Keys.Delete:
                    if (SelectionLength != 0)
                        removeSelection();
                    else if (CursorPosition < CurrentText.Length)
                        CurrentText = CurrentText.CutOut(CursorPosition, 1);
                    break;

                case Keys.Left:
                    if (KeyboardInfo.IsShiftDown)
                    {
                        if (CursorPosition > 0)
                            _selectionLength++;
                    }
                    else
                    {
                        _selectionLength = 0;
                    }
                    CursorPosition--;
                    break;

                case Keys.Right:
                    if (KeyboardInfo.IsShiftDown)
                    {
                        if (CursorPosition < CurrentText.Length)
                            _selectionLength--;
                    }
                    else
                    {
                        _selectionLength = 0;
                    }
                    CursorPosition++;
                    break;

                case Keys.Up:

                    break;

                case Keys.Down:

                    break;

                case Keys.A:
                    if (!KeyboardInfo.IsControlDown)
                        goto default;

                    _cursorPosition = 0;
                    _selectionLength = CurrentText.Length;
                    break;

                default:
                    if (c == '\0')
                        break;
                    if (SelectionLength != 0)
                        removeSelection();
                    CurrentText = CurrentText.Insert(CursorPosition, c.ToString());
                    CursorPosition++;                    
                    break;
            }
        }

        void removeSelection()
        {
            if (SelectionLength < 0)
            {
                CurrentText = CurrentText.CutOut(CursorPosition - SelectionLength, -SelectionLength);
                CursorPosition -= SelectionLength;
            }
            else if(SelectionLength > 0)
            {
                CurrentText = CurrentText.CutOut(CursorPosition, SelectionLength);
            }
            SelectionLength = 0;
        }

        Vector CursorSize
        {
            get { return new Vector(Font.UiCharSpacing, Font.UiHeight); }
        }

        public override void OnDraw(Graphics g)
        {
            base.OnDraw(g);
            
            if(CursorVisible)           // cursor
            {
                var pos = Location + new Vector((float)textPositions[CursorPosition], Size.Y - CursorSize.Y);
                g.Draw(Content.Textures.Blank, pos, CursorSize, CursorColor);
            }

            //text
            g.DrawString(Font, CurrentText, ForeColor, Location + new Vector(0, Size.Y), 0, 1);

            if (SelectionLength != 0)    // selection
            {
                var start = textPositions[CursorPosition];
                var end = textPositions[CursorPosition + SelectionLength];
                g.Draw(Content.Textures.Blank,
                    Location + new Vector(start, Size.Y - CursorSize.Y), 
                    new Vector(end - start, CursorSize.Y), 
                    SelectionColor);
            }
        }

        protected override void OnUpdate(int msElapsed)
        {
            if(KeyboardInfo.IsActivated(GameAction.Chat))
            {
                if(IsWriting)
                {
                    //send message
                    _currentText = string.Empty;
                    _selectionLength = 0;
                    _cursorPosition = 0;
                }

                IsWriting = !IsWriting;
            }
            if(IsWriting)
            {
                //update cursor
                cursorBlink = (cursorBlink + msElapsed) % CursorBlinkRate;

            }
            base.OnUpdate(msElapsed);
        }

    }
}
