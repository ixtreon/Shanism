namespace Shanism.Client.UI
{
    /// <summary>
    /// A label with another label, lolol.
    /// </summary>
    public class ValueLabel : LabelControl<Label>
    {

        public string Value
        {
            get => Control.Text;
            set => Control.Text = value;
        }

        public object TextToolTip
        {
            get => Label.ToolTip;
            set => Label.ToolTip = value;
        }

        public object ValueToolTip
        {
            get => Control.ToolTip;
            set => Control.ToolTip = value;
        }

        public Font ValueFont
        {
            get => Control.Font;
            set => Control.Font = value;
        }

        public ValueLabel()
        {
            Label.CanHover = true;

            Control.CanHover = true;
            Control.TextAlign = AnchorPoint.CenterRight;
        }
    }
}
