using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI
{
    public class SliderLabel : Control
    {
        static readonly Vector DefaultSize = new Vector(0.4, 0.07);

        public readonly Label Label;
        public readonly Slider Slider;

        public double LabelWidthRatio { get; set; } = 0.6;

        
        public string Text { get { return Label.Text; } set { Label.Text = value; } }
        public TextureFont TextFont { get { return Label.Font; } set { Label.Font = value; } }
        public Color TextColor { get { return Label.TextColor; } set { Label.TextColor = value; } }

        public double MinValue { get { return Slider.MinValue; } set { Slider.MinValue = value; } }
        public double MaxValue { get { return Slider.MaxValue; } set { Slider.MaxValue = value; } }

        public SliderLabel()
        {
            Size = DefaultSize;

            Add(Label = new Label
            {
                Size = new Vector(LabelWidthRatio * Size.X, Size.Y),
                Location = Vector.Zero,
                ParentAnchor = AnchorMode.Top | AnchorMode.Left | AnchorMode.Bottom,
            });

            Add(Slider = new Slider
            {
                Size = new Vector(Size.X - Label.Size.X, Size.Y),
                Location = Label.Bounds.TopRight,
                ParentAnchor = AnchorMode.Top | AnchorMode.Right | AnchorMode.Bottom,
            });

            this.SizeChanged += onSizeChanged;
        }

        void onSizeChanged(Control obj)
        {
            Label.Size = new Vector(LabelWidthRatio * Size.X, Size.Y);
            Slider.Location = Label.Bounds.TopRight;
            Slider.Size = new Vector(Size.X - Label.Size.X, Size.Y);
        }
    }
}
