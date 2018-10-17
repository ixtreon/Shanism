using Shanism.Client.UI.Containers;
using Shanism.Common;
using System;
using System.Numerics;

namespace Shanism.Client.UI
{
    public class LabelControl<T> : SplitPanel
        where T : Control, new()
    {
        const float DefaultWidth = 0.5f;
        const float DefaultLabelWidth = 0.33f;

        protected Label Label { get; }
        public T Control { get; }


        public LabelControl() : base(Axis.Horizontal)
        {
            AllowUserResize = false;
            Padding = 0;
            SizeMode = SplitSizeMode.FixedFirst;

            Label = new Label(" ", true);
            Control = new T();

            Size = new Vector2(DefaultWidth, Label.Size.Y);
            SplitAt = DefaultLabelWidth;

            First = Label;
            Second = Control;
        }


        public string Text
        {
            get => Label.Text;
            set => Label.Text = value;
        }

        public Font Font
        {
            get => Label.Font;
            set => Label.Font = value;
        }

        public Color Color
        {
            get => Label.TextColor;
            set => Label.TextColor = value;
        }

        public override object ToolTip
        {
            get => base.ToolTip;
            set => base.ToolTip = Label.ToolTip = Control.ToolTip = value;
        }
    }
}
