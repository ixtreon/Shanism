using Shanism.Common;
using System;
using System.Numerics;

namespace Shanism.Client.UI
{
    /// <summary>
    /// A label plus a slider.
    /// </summary>
    public class SliderLabel : LabelControl<Slider>
    {
        static readonly Vector2 DefaultSize = new Vector2(0.4f, 0.07f);


        public SliderLabel()
        {
            Control.Padding = 0;
        }

        public event Action<Slider> ValueChanged
        { 
            add => Control.ValueChanged += value;
            remove => Control.ValueChanged -= value;
        }

        public float Value
        {
            get => Control.Value;
            set => Control.Value = value;
        }

        public float ValueStep
        {
            get => Control.Step;
            set => Control.Step = value;
        }

        public float MinValue
        {
            get => Control.MinValue;
            set => Control.MinValue = value;
        }
        public float MaxValue
        {
            get => Control.MaxValue;
            set => Control.MaxValue = value;
        }
    }
}
