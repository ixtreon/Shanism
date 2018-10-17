namespace Shanism.Client.UI
{
    /// <summary>
    /// A combo of a label and an IntBox.
    /// </summary>
    public class IntLabel : LabelControl<IntBox>
    {

        public int MinValue
        {
            get => Control.MinValue;
            set => Control.MinValue = value;
        }

        public int MaxValue
        {
            get => Control.MaxValue;
            set => Control.MaxValue = value;
        }

        public int Value
        {
            get => Control.Value;
            set => Control.Value = value;
        }
    }
}
