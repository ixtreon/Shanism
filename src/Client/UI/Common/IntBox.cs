using System;

namespace Shanism.Client.UI
{
    public class IntBox : TextBar
    {
        int value = -1;
        int minValue = 0;
        int maxValue = int.MaxValue;

        /// <summary>
        /// Gets or sets the numerical value of the int-box.
        /// </summary>
        public int Value
        {
            get => value;
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    Text = value.ToString();
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }


        public int MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                if (Value > maxValue)
                    Value = maxValue;
            }
        }

        public int MinValue
        {
            get => minValue;
            set
            {
                minValue = value;
                if (Value < minValue)
                    Value = minValue;
            }
        }

        public event UiEventHandler<EventArgs> ValueChanged;

        protected override void OnCharInput(KeyboardArgs e)
        {
            // only let digits through..
            if (e.RecognizedCharacter != null && !e.Keybind.Control && !char.IsDigit(e.RecognizedCharacter.Value))
                return;

            base.OnCharInput(e);
        }

        protected override void OnMouseScroll(MouseScrollArgs e)
        {
            base.OnMouseScroll(e);

            var newValue = value - (int)Math.Round(e.ScrollDelta);
            if (newValue >= minValue && newValue <= maxValue)
            {
                Text = newValue.ToString();
                SetFocus();
            }
        }

        int getValueFromText()
        {
            if (string.IsNullOrEmpty(Text))
                return 0;

            if (!int.TryParse(Text, out var intText))
                return value;

            if (intText < minValue || intText > MaxValue)
                return value;

            return intText;
        }

        protected override void OnFocusLost(EventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
                Text = "0";

            value = getValueFromText();
            ValueChanged?.Invoke(this, EventArgs.Empty);

            base.OnFocusLost(e);
        }

    }
}
