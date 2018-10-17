using Shanism.Common;
using System;
using System.Numerics;

namespace Shanism.Client.UI
{
    public class ScrollBar : Control
    {
        const float MinHandleSize = 0.02f;

        public static readonly float DefaultWidth = 0.06f;

        readonly Control slider;

        Vector2? dragOffset;

        public float Min { get; set; } = 0;
        public float Max { get; set; } = 1;
        public float WindowSize { get; set; } = 0;
        public float Start { get; set; } = 0;

        /// <summary>
        /// Gets the span of this scroll bar, 
        /// that is the difference between the maximum and the minimum value.
        /// </summary>
        public float Span => (Max - Min);

        float _sliderSize => WindowSize / Span;

        float _sliderPos 
        {
            get => (Start - Min) / Span;
            set => Start = value * Span + Min;
        }

        public Axis Direction { get; set; } = Axis.Vertical;

        /// <summary>
        /// Gets or sets the color of the slider.
        /// </summary>
        public Color SliderColor
        {
            get => slider.BackColor;
            set => slider.BackColor = value;
        }

        public ScrollBar()
        {
            Size = new Vector2(DefaultWidth, 0.6f);
            BackColor = UiColors.ControlBackground;

            Add(slider = new Button());

            slider.MouseDown += (o, e) => dragOffset = e.Position;
            slider.MouseUp += (o, e) => dragOffset = null;
            slider.MouseMove += continueScrolling;

            this.MouseScroll += onMouseScroll;
            slider.MouseScroll += onMouseScroll;
        }

        void onMouseScroll(Control sender, MouseScrollArgs e)
        {
            Start += e.ScrollDelta * 0.1f;
        }

        public override void Update(int msElapsed)
        {
            Start = Start.Clamp(Min, Max - WindowSize);

            switch (Direction)
            {
                case Axis.Vertical:
                    slider.Size = new Vector2(1, _sliderSize) * Size;
                    slider.Location = new Vector2(0, _sliderPos) * Size;
                    break;

                case Axis.Horizontal:
                    slider.Size = new Vector2(_sliderSize, 1) * Size;
                    slider.Location = new Vector2(_sliderPos, 0) * Size;
                    break;
            }
        }

        void continueScrolling(Control sender, MouseArgs e)
        {
            if (dragOffset == null)
                return;

            var dSlide = (e.Position - dragOffset.Value) * (Max - Min) / Size;

            Start += dSlide.Get(Direction);
        }

        void endScrolling(MouseButtonArgs e)
        {
            dragOffset = null;
        }

        void startScrolling(MouseButtonArgs e)
        {
            dragOffset = e.Position;
        }
    }
}
