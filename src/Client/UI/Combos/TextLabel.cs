namespace Shanism.Client.UI
{
    public class TextLabel : LabelControl<TextBar>
    {
        const float defaultTextWidth = 0.3f;

        public string ValueText
        {
            get => Control.Text;
            set => Control.Text = value;
        }


        public event UiEventHandler<KeyboardArgs> TextChanged
        {
            add => Control.CharacterInput += value;
            remove => Control.CharacterInput -= value;
        }

        public Font ControlFont
        {
            get => Control.Font;
            set => Control.Font = value;
        }

    }
}
