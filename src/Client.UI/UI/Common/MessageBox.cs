using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI
{
    public class MessageBox : Window
    {
        const double buttonWidth = 0.24;

        readonly Label textLabel;

        public event Action<MessageBoxButtons> ButtonClicked;

        /// <summary>
        /// Gets or sets whether the box is removed from its parent 
        /// (rather than simply being hidden)
        /// when a button is clicked.
        /// True by default.
        /// </summary>
        public bool RemoveOnButtonClick { get; set; } = true;

        public MessageBox(string title, string text,
            MessageBoxButtons actions = MessageBoxButtons.Ok)
        {
            var textFont = Content.Fonts.NormalFont;

            //calc box size
            var btnTypes = actions.GetButtons();
            var btnSpace = btnTypes.Length * (buttonWidth + Padding) - Padding;

            var boxMinWidth = Math.Max(0.6, btnSpace + 2 * LargePadding);
            var boxMinHeight = 0.3;

            var txtSize = textFont.MeasureString(text, Screen.UiSize.X - 2 * LargePadding);

            var boxWidth = Math.Max(txtSize.X + 2 * LargePadding, boxMinWidth);
            var boxHeight = Math.Max(txtSize.Y + TitleHeight + 2 * LargePadding + Button.DefaultSize.Y, boxMinHeight);


            //set box window props
            IsVisible = true;
            CanResize = false;
            TitleText = title;
            Size = new Vector(boxWidth, boxHeight);
            Top = (Screen.UiSize.Y - boxHeight).Clamp(0, 0.3);
            CenterX();
            ParentAnchor = AnchorMode.Top;

            //text
            Add(textLabel = new Label
            {
                Font = textFont,

                AutoSize = false,
                Location = new Vector(LargePadding, TitleHeight + LargePadding),
                Size = new Vector(Size.X - 2 * LargePadding, txtSize.Y),

                Text = text,
                TextXAlign = 0.5f,
            });

            //buttons
            var buttonSpacing = (boxWidth - 2 * LargePadding - buttonWidth * btnTypes.Length)
                / (btnTypes.Length - 1);
            for (int i = 0; i < btnTypes.Length; i++)
            {
                var btnType = btnTypes[i];
                var btn = new Button
                {
                    Text = btnType.ToString(),

                    Width = buttonWidth,
                    Bottom = Size.Y - LargePadding,
                };

                if (btnTypes.Length > 1)
                    btn.Left = LargePadding + i * (buttonWidth + buttonSpacing);
                else
                    btn.Left = (Size.X - buttonWidth) / 2;

                btn.MouseClick += (e) =>
                {
                    if (RemoveOnButtonClick && Parent != null)
                        Parent.Remove(this);
                    else
                        IsVisible = false;

                    ButtonClicked?.Invoke(btnType);
                };

                Add(btn);
            }
        }
    }

    [Flags]
    public enum MessageBoxButtons
    {
        Ok = 1 << 0,
        Cancel = 1 << 1,
        Yes = 1 << 2,
        No = 1 << 3,
    }

    public static class MessageBoxActionsExt
    {
        static MessageBoxButtons[] buttonTypes =
            (MessageBoxButtons[])Enum.GetValues(typeof(MessageBoxButtons));

        public static MessageBoxButtons[] GetButtons(this MessageBoxButtons act)
        {
            return buttonTypes
                .Where(ty => (act & ty) == ty)
                .ToArray();
        }

        public static void ShowMessageBox(this Control parent, string caption, string text)
        {
            foreach(var c in parent.Controls.OfType<MessageBox>().ToList())
                parent.Remove(c);
            parent.Add(new MessageBox(caption, text));
        }
    }
}
