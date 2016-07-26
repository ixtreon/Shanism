using Shanism.Client.Input;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;
using Color = Microsoft.Xna.Framework.Color;
using Shanism.Client.Drawing;

namespace Shanism.Client.UI.Chat
{
    class ChatBar : Control, IChatProvider
    {
        const int CursorBlinkRate = 1000;
        const int MaxLineHistory = 256;

        private int _cursorPosition;
        private int _selectionLength;
        private string _currentText;

        private int cursorBlink;

        double[] textPositions;


        public TextureFont Font { get; set; }

        public Color ForeColor { get; set; }


        public Color CursorColor { get; set; }

        public Color SelectionColor { get; set; }

        public string CurrentText
        {
            get { return _currentText; }
            private set
            {
                if (value != _currentText)
                {
                    _currentText = value;
                    textPositions = Font.GetLineCharsUi(value);
                }
            }
        }

        //public int CursorPosition
        //{
        //    get { return _cursorPosition; }
        //    private set { _cursorPosition = value.Clamp(0, CurrentText.Length); }
        //}

        public int SelectionLength
        {
            get { return _selectionLength; }
            private set { _selectionLength = value.Clamp(0, CurrentText.Length - _cursorPosition); }
        }

        public bool IsCursorVisible
            => HasFocus && (cursorBlink < CursorBlinkRate / 2);

        Vector CursorSize
            => new Vector(Font.HeightUi / 10, Font.HeightUi);


        readonly LinkedList<string> messageHistory = new LinkedList<string>();

        LinkedListNode<string> currentMsgHistoryNode;

        readonly KeyRepeater keyRepeater = new KeyRepeater();

        public event Action<string> ChatSent;

        public ChatBar()
        {
            Font = Content.Fonts.NormalFont;
            CurrentText = string.Empty;

            CanFocus = true;
            Size = new Vector(0.5, Font.HeightUi);
            //KeyboardInfo.ChatProvider.CharPressed += ChatProvider_CharPressed;

            CursorColor = Color.LightBlue;
            ForeColor = Color.LightBlue;
            SelectionColor = Color.White.SetAlpha(150);


            keyRepeater.KeyRepeated += onKeyRepeated;
            this.KeyPressed += onKeyPressed;
        }

        private void onKeyRepeated(Keybind k, char? c)
        {
            switch (k.Key)
            {
                case Keys.Back:     // {Backspace} to delete previous char or selection
                    if (SelectionLength != 0)
                        removeSelection();
                    else
                        deletePrevCharacter();
                    break;

                case Keys.Delete:   // {Delete} to delete next char or selection
                    if (SelectionLength != 0)
                        removeSelection();
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
                    if (_cursorPosition < CurrentText.Length)
                    {
                        if (k.Shift)
                            _selectionLength--;
                        else
                            _selectionLength = 0;

                        _cursorPosition++;
                    }
                    break;

                case Keys.Up:       // {Up}, {Down} to browse line history
                    if (messageHistory.Any())
                    {
                        if (currentMsgHistoryNode == null)
                            currentMsgHistoryNode = messageHistory.First;
                        else
                            currentMsgHistoryNode = currentMsgHistoryNode.Next;

                        CurrentText = currentMsgHistoryNode?.Value ?? string.Empty;
                        selectAllText();
                    }
                    break;

                case Keys.Down:
                    if (messageHistory.Any())
                    {
                        if (currentMsgHistoryNode == null)
                            currentMsgHistoryNode = messageHistory.Last;
                        else
                            currentMsgHistoryNode = currentMsgHistoryNode.Previous;

                        CurrentText = currentMsgHistoryNode?.Value ?? string.Empty;
                        selectAllText();
                    }
                    break;

                case Keys.Enter:    // {Enter} sends the message
                    //add to line history
                    if (!string.Equals(messageHistory.First?.Value, CurrentText))
                    {
                        messageHistory.AddFirst(CurrentText);
                        if (messageHistory.Count > MaxLineHistory)
                            messageHistory.RemoveLast();
                    }
                    //send the message
                    if (!string.IsNullOrEmpty(CurrentText))
                    {
                        ChatSent?.Invoke(CurrentText);
                        currentMsgHistoryNode = null;
                        clearText();
                    }
                    ClearFocus();
                    break;

                case Keys.Escape:       // {Esc} clears the message
                    clearText();
                    ClearFocus();
                    break;

                case Keys.A:            // {Ctrl+A} selects everything
                    if (!k.Control)
                        goto default;   // but {A} is a normal char

                    selectAllText();
                    break;

                default:
                    if (c != null)
                        writeChar(c.Value);
                    break;
            }
        }

        void onKeyPressed(Keybind k)
        {
            keyRepeater.SetKey(k);
        }

        void writeChar(char c)
        {
            if (SelectionLength != 0)   //overwrite selection
                removeSelection();

            CurrentText = CurrentText.Insert(_cursorPosition, c.ToString());
            _cursorPosition++;
        }

        void selectAllText()
        {
            _cursorPosition = CurrentText.Length;
            _selectionLength = -CurrentText.Length;
        }

        void deleteNextCharacter()
        {
            //if not at last char, delete next char
            if (_cursorPosition < CurrentText.Length)
            {
                CurrentText = CurrentText.Remove(_cursorPosition, 1);
            }
        }

        void deletePrevCharacter()
        {
            //if not the first char, delete prev char
            if (_cursorPosition > 0)
            {
                _cursorPosition--;
                CurrentText = CurrentText.Remove(_cursorPosition, 1);
            }
        }

        void removeSelection()
        {
            if (SelectionLength < 0)
            {
                CurrentText = CurrentText.Remove(_cursorPosition + SelectionLength, -SelectionLength);
                _cursorPosition += SelectionLength;
            }
            else if (SelectionLength > 0)
            {
                CurrentText = CurrentText.Remove(_cursorPosition, SelectionLength);
            }
            SelectionLength = 0;
        }

        void clearText()
        {
            _currentText = string.Empty;
            _selectionLength = 0;
            _cursorPosition = 0;
        }

        public override void OnDraw(Graphics g)
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
                g.DrawString(Font, CurrentText, ForeColor, new Vector(Padding), 0, 0);
            }
            else
            {
                //draw partial string
                var diff = lastTextChar - lastPossiblePos;
                var trimmedChars = textPositions.TakeWhile(p => p < diff).Count();

                var firstCharPos = (textPositions[trimmedChars] - diff);
                g.DrawString(Font, CurrentText.Substring(trimmedChars), ForeColor, new Vector(firstCharPos, Padding), 0, 0);
            }

            //cursor
            if (IsCursorVisible)
            {
                var pos = new Vector((float)textPositions[_cursorPosition], 0) + Padding;
                g.Draw(Content.Textures.Blank, pos, CursorSize, CursorColor);
            }
        }

        protected override void OnUpdate(int msElapsed)
        {
            BackColor = Color.Black.SetAlpha(HasFocus ? 125 : 75);

            //update cursor
            if (HasFocus)
                cursorBlink = (cursorBlink + msElapsed) % CursorBlinkRate;

            //update repeated keys
            keyRepeater.Update(msElapsed);

            base.OnUpdate(msElapsed);
        }
    }
}
