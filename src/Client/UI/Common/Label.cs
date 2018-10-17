using Shanism.Common;
using System;
using System.Numerics;

namespace Shanism.Client.UI
{

    /// <summary>
    /// Displays a simple label. 
    /// </summary>
    public class Label : Control
    {
        public static Font DefaultFont => Content.Fonts.FancyFont;

        string _text = string.Empty;
        Font _font;
        LabelSizeMode _sizeMode = LabelSizeMode.Manual;
        int _lineHeight = 0;

        public Color TextColor { get; set; }

        public string Text
        {
            get => _text;
            set { _text = value; resize(); }
        }

        public Font Font
        {
            get => _font;
            set { _font = value; resize(); }
        }

        /// <summary>
        /// Gets or sets whether this label automatically fits to the text inside it. 
        /// </summary>
        public bool AutoSize
        {
            get => _sizeMode == LabelSizeMode.Automatic;
            set
            {
                _sizeMode = value ? LabelSizeMode.Automatic : LabelSizeMode.Manual;
                resize();
            }
        }

        /// <summary>
        /// When <see cref="AutoSize"/> is set to true, sets the maximum width of the label.
        /// </summary>
        public float? MaxWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the labels in terms of its font size.
        /// <para>
        /// Throws an exception if the <see cref="Font"/> property is null.
        /// </para>
        /// </summary>
        public int LineHeight
        {
            get => _lineHeight;
            set
            {
                _lineHeight = value;
                _sizeMode = LabelSizeMode.FixedLineCount;
                resize();
            }
        }

        /// <summary>
        /// Gets or sets the position of the text within the control.
        /// </summary>
        public AnchorPoint TextAlign { get; set; } = AnchorPoint.CenterLeft;


        float? MaxTextWidth;

        public Label()
        {
            TextColor = UiColors.TextTitle;

            Size = new Vector2(0.4f, 0.1f);
            Font = DefaultFont;

            CanHover = true;
        }

        public Label(string text, bool autoSize = true)
        {
            Text = text;
            TextColor = UiColors.TextTitle;
            Font = DefaultFont;

            CanHover = true;
            Size = new Vector2(0.4f, 0.1f);
            AutoSize = autoSize;
        }

        void resize()
        {
            switch(_sizeMode)
            {
                case LabelSizeMode.Manual:
                    MaxTextWidth = ClientBounds.Width;
                    return;

                case LabelSizeMode.FixedLineCount:
                    MaxTextWidth = ClientBounds.Width;
                    Height = LineHeight * _font.Height + 2 * Padding;
                    return;

                case LabelSizeMode.Automatic:
                    ResizeAnchor = ParentAnchor;
                    MaxTextWidth = null;

                    // account for MaxWidth when type = Auto
                    Vector2 stringSize;
                    if(MaxWidth != null)
                        stringSize = Font.MeasureString(_text, MaxWidth.Value);
                    else
                        stringSize = Font.MeasureString(_text);

                    Size = stringSize + new Vector2(2 * Padding + 1E-5f);
                    return;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Draw(Canvas c)
        {
            base.Draw(c);

            if(_text == null)
                return;

            c.DrawString(Font, _text, TextColor, ClientBounds, TextAlign);
        }
    }
}
