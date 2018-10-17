using Microsoft.Xna.Framework.Input;
using Shanism.Client.IO;
using Shanism.Client.Models.Text;
using Shanism.Common;
using System;
using System.Numerics;

namespace Shanism.Client.UI
{
    public class TextBar : Control
    {
        const int CursorBlinkRate = 1000;

        TextEditorModel textModel;

        int cursorBlink;
        Font font;
        string visibleText;

        public Font Font
        {
            get => font;
            set => textModel.Font = font = value;
        }

        public Color ForeColor { get; set; }

        public Color CursorColor { get; set; }

        public Color SelectionColor { get; set; }


        public virtual string Text
        {
            get => textModel.Text;
            set
            {
                textModel.SetText(value);
                updateVisibleText();
            }
        }

        public bool IsCursorVisible
            => IsFocusControl && (cursorBlink < CursorBlinkRate / 2);

        Vector2 CursorSize
            => new Vector2(Font.Height / 10, Font.Height);


        public TextBar()
        {
            textModel = new TextEditorModel(Content.Fonts.NormalFont);
            Font = Content.Fonts.NormalFont;

            CanFocus = true;
            Size = new Vector2(0.5f, Font.Height + 2 * Padding);

            Cursor = GameCursor.TextInput;
            CursorColor = UiColors.Text;
            ForeColor = UiColors.Text;
            BackColor = UiColors.ControlBackground;
            SelectionColor = UiColors.SelectionBackground;

        }

        /// <summary>
        /// Updates the cursor blink state.
        /// </summary>
        public override void Update(int msElapsed)
        {
            if (IsFocusControl)
                cursorBlink = (cursorBlink + msElapsed) % CursorBlinkRate;
        }

        bool isSelecting;

        protected override void OnMouseDown(MouseButtonArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                textModel.SetCursor(e.Position.X - ClientBounds.Left + offset(actualWindow.start) + actualWindow.offset);

                isSelecting = true;
                cursorBlink = 0;
            }

            base.OnMouseClick(e);
        }

        protected override void OnMouseMove(MouseArgs e)
        {
            if (isSelecting)
            {
                textModel.DragCursor(e.Position.X - ClientBounds.Left + offset(actualWindow.start) + actualWindow.offset);
                cursorBlink = 0;
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseButtonArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                isSelecting = false;
                cursorBlink = 0;
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseDoubleClick(MouseButtonArgs e)
        {
            base.OnMouseDoubleClick(e);

            textModel.SelectAll();
        }

        protected override void OnCharInput(KeyboardArgs e)
        {
            var kb = e.Keybind;
            switch (e.Key)
            {
                case Keys.Back:     // {Backspace} to delete previous char or selection
                    textModel.DeletePrev(kb.Modifiers);
                    updateVisibleText();
                    break;

                case Keys.Delete:   // {Delete} to delete next char or selection
                    textModel.DeleteNext(kb.Modifiers);
                    updateVisibleText();
                    break;

                case Keys.Left:     // {Left}, {Right} to move cursor, select text
                    textModel.MoveCursor(kb.Modifiers, -1);
                    updateVisibleText();
                    break;

                case Keys.Right:
                    textModel.MoveCursor(kb.Modifiers, 1);
                    updateVisibleText();
                    break;

                case Keys.Escape:
                    // ???
                    break;


                case Keys.A when kb.Control:
                    textModel.SelectAll();
                    break;

                default:
                    if (e.RecognizedCharacter == null || kb.Control)
                        break;

                    textModel.WriteChar(e.RecognizedCharacter.Value);
                    updateVisibleText();
                    break;
            }

            cursorBlink = 0;

            base.OnCharInput(e);
        }

        (int start, int end, float offset) idealWindow;
        (int start, int end, float offset) actualWindow;

        void focusIndexOfText(int id)
        {
            if (id <= idealWindow.start)
            {
                // cursor is left of visible window

                while (id > 0 && offset(Text.Length) - offset(id - 1) < maxTextSize)
                    id--;

                setWindowStart(id);
            }
            else if (id >= idealWindow.end)
            {
                // cursor is right of visible window

                if (id < textModel.Length && offset(id + 1) < maxTextSize)
                    id++;

                setWindowEnd(id);
            }
            else
            {
                // cursor is within the visible window
                if (idealWindow.start > 0 && idealWindow.end == textModel.Length)
                    setWindowEnd(textModel.Length);

                if (idealWindow.end < textModel.Length && idealWindow.start == 0)
                    setWindowStart(0);
            }
        }

        float maxTextSize => ClientBounds.Width;

        void setWindowStart(int id)
        {
            var startID = id;
            var startOffset = offset(startID);

            var endID = (int)(textModel.IndexOfRaw(startOffset + maxTextSize) + 1f);    // round up
            var drawOffset = 0;

            idealWindow = (startID, endID, drawOffset);
        }

        void setWindowEnd(int id)
        {
            var endID = id;
            var endOffset = offset(endID);

            var startID = (int)(textModel.IndexOfRaw(endOffset - maxTextSize));  // round down
            var startOffset = offset(startID);

            var textLength = endOffset - startOffset;
            var drawOffset = Math.Min(0, maxTextSize - textLength);

            idealWindow = (startID, endID, drawOffset);
        }

        void updateActualWindow()
        {
            actualWindow.start = idealWindow.start.Clamp(0, textModel.Length);
            actualWindow.end = idealWindow.end.Clamp(0, textModel.Length);
            actualWindow.offset = idealWindow.offset;
        }

        void updateVisibleText()
        {
            //// make sure cursor is not left of the window
            //if (CursorPosition < windowStartId)
            //    windowStartId = CursorPosition;

            //// make sure cursor is not after the window
            //while (offset(CursorPosition) - offset(windowStartId) > ClientBounds.Width)
            //    windowStartId++;

            //// get the character at the end line
            //windowEndId = textModel.IndexOf(offset(windowStartId) + ClientBounds.Width);
            //visibleText = Text.Substring(windowStartId, windowEndId - windowStartId);

            // new shit:
            focusIndexOfText(CursorPosition);
            updateActualWindow();

            visibleText = Text.Substring(actualWindow.start, actualWindow.end - actualWindow.start);

        }


        public int CursorPosition => textModel.CursorPosition;

        float offset(int i) => textModel.Offset(i);

        public void SelectAllText() => textModel.SelectAll();

        public void ClearText()
        {
            textModel.ClearText();
            updateVisibleText();
        }

        public override void Draw(Canvas c)
        {
            //background
            base.Draw(c);

            c.FillRectangle(ClientBounds, Color.Red.SetAlpha(50));

            var windowStartId = actualWindow.start;
            var windowEndId = actualWindow.end;

            //selection
            if (IsFocusControl && textModel.SelectionLength != 0)
            {
                var startX = offset(CursorPosition) - offset(windowStartId);
                var endX = offset(CursorPosition + textModel.SelectionLength) - offset(windowStartId);

                if (textModel.SelectionLength < 0)
                {
                    var tmp = startX;
                    startX = endX;
                    endX = tmp;
                }

                if (startX < 0)
                    startX = 0;

                if (endX > ClientBounds.Width)
                    endX = ClientBounds.Width;

                var drawPos = ClientBounds.Position + new Vector2(startX, 0);
                var drawSize = new Vector2(endX - startX, CursorSize.Y);

                c.FillRectangle(drawPos, drawSize, SelectionColor);
            }

            //text
            c.DrawString(Font, visibleText, ClientBounds.Position + new Vector2(actualWindow.offset, 0), ForeColor);

            //cursor
            if (IsCursorVisible)
            {
                var cPos = new Vector2(offset(CursorPosition) - offset(windowStartId) + actualWindow.offset, 0);
                c.FillRectangle(ClientBounds.Position + cPos, CursorSize, CursorColor);
            }
        }
    }
}
