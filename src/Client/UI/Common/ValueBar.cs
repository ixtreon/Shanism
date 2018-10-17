using Shanism.Common;

namespace Shanism.Client.UI
{
    /// <summary>
    /// A progress bar representing the current value of a property relative to the maximum value of that property. 
    /// </summary>
    public class ValueBar : ProgressBar
    {
        public float Value;

        public float MaxValue;

        public ValueBar()
        {
            BackColor = new Color(64, 64, 64, 64);
            CanHover = true;
        }

        public override void Update(int msElapsed)
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
