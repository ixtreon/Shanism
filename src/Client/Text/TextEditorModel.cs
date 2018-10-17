using Shanism.Client.IO;
using System;

namespace Shanism.Client.Models.Text
{
    class TextEditorModel
    {
        readonly ICharBuffer<char> text;

        StringOffsetBuffer offsets;
        Font font;

        public string Text
        {
            get => text.ToString();
        }

        public Font Font
        {
            get => font;
            set
            {
                font = value;

                // reset offsets
                offsets = new StringOffsetBuffer(font);
                offsets.Reset(Text);
            }
        }

        public int CursorPosition { get; private set; }

        public int SelectionLength { get; private set; }
        public int Length => text.Length;

        public TextEditorModel(Font font)
        {
            text = new LameStringBuffer();
            Font = font;
        }

        public void SetText(string value)
        {
            text.Reset(value);
            offsets.Reset(value);

            CursorPosition = value?.Length ?? 0;
        }

        /// <summary>
        /// Gets the horizontal offset of the character at the given position.
        /// </summary>
        public float Offset(int i) => offsets[i];

        /// <summary>
        /// Gets the index of the character at the given horizontal offset.
        /// </summary>
        public int IndexOf(float x)
            => offsets.IndexOf(x);

        public float IndexOfRaw(float x)
            => offsets.IndexOfRaw(x);

        public void SetCursor(float x)
        {
            CursorPosition = offsets.IndexOf(x);
            SelectionLength = 0;
        }

        public void DragCursor(float x)
        {
            var oldCursor = CursorPosition + SelectionLength;
            CursorPosition = offsets.IndexOf(x);
            SelectionLength = oldCursor - CursorPosition;
        }

        public void MoveCursor(ModifierKeys mods, int baseStep)
        {
            if(!cursorOK(CursorPosition + baseStep))
                return;

            // account for Ctrl
            var step = baseStep;
            if(mods.HasControl())
                step = findMoveStep(step);

            // update cursor + selection
            CursorPosition += step;

            if(mods.HasShift())
                SelectionLength -= step;
            else
                SelectionLength = 0;
        }

        public void DeletePrev(ModifierKeys mods)
            => delete(mods, -1);

        public void DeleteNext(ModifierKeys mods)
            => delete(mods, 1);

        public void SelectAll()
        {
            CursorPosition = text.Length;
            SelectionLength = -text.Length;
        }

        public void SelectWord()
        {
            var start = CursorPosition + findMoveStep(-1);
            var end = CursorPosition + findMoveStep(1);

            CursorPosition = end;
            SelectionLength = end - start;
        }

        public void WriteChar(char c)
        {
            if(SelectionLength != 0)   //overwrite selection
                deleteSelection();

            text.Insert(CursorPosition, c);
            offsets.Insert(CursorPosition, c);
            CursorPosition++;
        }

        public void ClearText()
        {
            text.Reset();
            offsets.Reset();
            CursorPosition = 0;
            SelectionLength = 0;
        }

        void deleteSelection()
        {
            if(SelectionLength == 0)
                return;

            // move cursor to start of selection
            if(SelectionLength < 0)
            {
                CursorPosition = CursorPosition + SelectionLength;
                SelectionLength = -SelectionLength;
            }

            // delete selection
            text.Delete(CursorPosition, SelectionLength);
            offsets.Delete(CursorPosition, SelectionLength);

            SelectionLength = 0;
        }

        int findMoveStep(int baseStep)
        {
            var endPos = CursorPosition + baseStep;
            if(!cursorOK(endPos))
                return 0;

            while(textOK(endPos)
                && cursorOK(endPos + baseStep)
                && char.IsLetterOrDigit(Text[endPos]))
                endPos += baseStep;

            return endPos - CursorPosition;
        }

        void delete(ModifierKeys mods, int baseStep)
        {
            if(SelectionLength != 0)
            {
                deleteSelection();
                return;
            }

            if(!cursorOK(CursorPosition + baseStep))
                return;

            // account for Ctrl
            var step = baseStep;
            if(mods.HasControl())
                step = findMoveStep(step);

            // move cursor to start
            if(step < 0)
            {
                step = -step;
                CursorPosition -= step;
            }

            // delete
            text.Delete(CursorPosition, step);
            offsets.Delete(CursorPosition, step);
        }


        bool textOK(int pos) => pos >= 0 && pos < Text.Length;
        bool cursorOK(int pos) => pos >= 0 && pos <= Text.Length;

    }
}
