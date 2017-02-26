using Shanism.Client.Input;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.Client.UI
{
    class Slider : ProgressBar
    {
        public static readonly Vector DefaultSize = new Vector(0.4, 0.07);
        bool sliding;
        double value;

        public int Decimals { get; set; } = 2;

        public double MinValue { get; set; } = 0;
        public double MaxValue { get; set; } = 1;

        public event Action<Slider> ValueChanged;

        public new Color ForeColor { get; set; } = Color.Goldenrod;

        public double Value
        {
            get { return value; }
            set { setVal(value); }
        }

        public Slider()
        {
            Size = DefaultSize;

            MouseDown += (e) => { sliding = true; updateVal(e); };
            MouseMove += (e) => { if (sliding) updateVal(e); };
            MouseUp += (e) => sliding = false;
        }

        void updateVal(MouseArgs e)
        {
            var newVal = MinValue + e.Position.X / Size.X * (MaxValue - MinValue);
            setVal(newVal);
        }

        void setVal(double newVal)
        {
            if (!MaxValue.Equals(0) && !newVal.Equals(value))
            {
                value = newVal.Clamp(MinValue, MaxValue);
                Progress = ((value - MinValue) / (MaxValue - MinValue));
                Text = value.ToString($"N{Decimals}");

                ValueChanged?.Invoke(this);
            }
        }

        protected override void OnUpdate(int msElapsed)
        {
            if (HoverControl == this)
                base.ForeColor = ForeColor.Darken();
            else
                base.ForeColor = ForeColor;
        }

        public override void OnDraw(Canvas g)
        {
            base.OnDraw(g);
        }

    }
}
