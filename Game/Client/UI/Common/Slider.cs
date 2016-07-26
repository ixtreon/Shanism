using Shanism.Client.Input;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI.Common
{
    class Slider : ProgressBar
    {
        public static readonly Vector DefaultSize = new Vector(0.4, 0.07);
        bool sliding;
        double value;

        public double MinValue { get; set; } = 0;
        public double MaxValue { get; set; } = 1;

        public event Action<Slider> ValueChanged;

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
            if (!MaxValue.Equals(0))
            {
                value = newVal.Clamp(MinValue, MaxValue);
                Progress = ((value - MinValue) / (MaxValue - MinValue));
                Text = $"{value:0.0}";

                ValueChanged?.Invoke(this);
            }
        }

    }
}
