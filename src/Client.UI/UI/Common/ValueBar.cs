using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;

namespace Shanism.Client.UI
{
    /// <summary>
    /// A progress bar representing the current value of a property relative to the maximum value of that property. 
    /// </summary>
    public class ValueBar : ProgressBar
    {
        public double Value;

        public double MaxValue;

        public ValueBar()
        {
            BackColor = new Color(64, 64, 64, 64);
            CanHover = true;
        }

        protected override void OnUpdate(int msElapsed)
        {
            if (MaxValue > 0)
            {
                Text = $"{Value:0}/{MaxValue:0}";
                Progress = Value / MaxValue;
            }
            else
            {
                Text = string.Empty;
                Progress = 0;
            }
        }
    }
}
