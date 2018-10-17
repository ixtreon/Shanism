using Shanism.Common;
using System;
using System.Numerics;

namespace Shanism.Client.UI
{
    /// <summary>
    /// A UI slider that can be dragged by the user.
    /// </summary>
    public class Slider : ProgressBar
    {
        public static readonly Vector2 DefaultSize = new Vector2(0.4f, 0.07f);
        bool sliding;
        float value;

        float _step;
        float _decimals;

        /// <summary>
        /// Gets or sets the minimum change in this slider's value.
        /// </summary>
        public float Step
        {
            get => _step;
            set
            {
                _step = value;
                _decimals = getDecimals(_step);
            }
        }


        static string getDecimals2(double val)
        {
            const double min = 1E-4;
            const double max = 1E+4;
            if (val < min || val > max)
                return "0.####E0";

            for (int i = 0; i < 4; i++)
                if (Math.Abs((val + 1e-5 % Math.Pow(10, -i))) < 1E-5)
                    return $"N{i}";

            return "N0";
        }

        static int getDecimals(double val)
        {
            var str = val.ToString("#.########");
            var dot = str.IndexOf('.');
            if (dot < 0)
                return 0;
            return str.Length - dot - 1;
        }

        public float MinValue { get; set; } = 0;
        public float MaxValue { get; set; } = 1;

        public event Action<Slider> ValueChanged;

        public new Color ForeColor { get; set; }

        public float Value
        {
            get => value;
            set => SetValue(value);
        }

        public Slider()
        {
            ForeColor = base.ForeColor;
            Size = DefaultSize;

            MouseDown += (o, e) => UpdateSlider(true, e.Position);
            MouseMove += (o, e) => UpdateSlider(e.Position);
            MouseUp += (o, e) => UpdateSlider(false, e.Position);
            MouseScroll += (o, e) => SetValue(value - e.ScrollDelta * Step);
        }

        void UpdateSlider(Vector2 mousePos)
        {
            if (sliding)
            {
                var ratio = mousePos.X / Size.X;
                var newValue = MinValue + ratio * (MaxValue - MinValue);

                SetValue(newValue);
            }
        }

        void UpdateSlider(bool isSliding, Vector2 mousePos)
        {
            sliding = isSliding;
            UpdateSlider(mousePos);
        }


        void SetValue(float newVal)
        {
            newVal = (float)Math.Round(newVal / Step) * Step;
            newVal = newVal.Clamp(MinValue, MaxValue);
            if (!MaxValue.Equals(0) && !newVal.Equals(value))
            {
                value = newVal;
                Progress = ((value - MinValue + 1) / (MaxValue - MinValue + 1));
                Text = value.ToString($"N{_decimals}");

                ValueChanged?.Invoke(this);
            }
        }

        public override void Update(int msElapsed)
        {
            if (IsHoverControl)
                base.ForeColor = ForeColor.Darken();
            else
                base.ForeColor = ForeColor;
        }

        public override void Draw(Canvas c)
        {
            base.Draw(c);
        }

    }
}
