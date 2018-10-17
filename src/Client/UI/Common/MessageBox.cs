using Shanism.Client.UI.Containers;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Shanism.Client.UI
{
    public class MessageBox : Window
    {
        class MessageBoxButton : Button
        {
            static readonly new Vector2 DefaultSize = new Vector2(0.24f, 0.06f);

            public MessageBoxButtons ButtonType { get; }

            public MessageBoxButton(MessageBoxButtons type)
            {
                ButtonType = type;
                Text = type.ToString();
                Size = DefaultSize;
            }
        }


        static readonly Vector2 MinContentSize = new Vector2(0.5f, 0.3f);
        const float buttonWidth = 0.24f;

        static Font DefaultFont => Content.Fonts.NormalFont;



        public event Action<MessageBoxButtons> ButtonClicked;

        readonly TaskCompletionSource<MessageBoxButtons> Task;

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
            Task = new TaskCompletionSource<MessageBoxButtons>();
            CanResize = false;
            TitleText = title;
            IsVisible = true;

            // buttons panel
            var buttonPanel = new ListPanel(Direction.LeftToRight, sizeMode: ListSizeMode.ResizeBoth)
            {
                ParentAnchor = AnchorMode.Bottom,
                Bottom = ClientBounds.Bottom,
            };
            createButtons(actions.GetButtons(), buttonPanel);

            var textLabel = new Label
            {
                Font = DefaultFont,

                AutoSize = true,
                Location = ClientBounds.Position,

                Text = text,
                TextAlign = AnchorPoint.TopCenter,
            };

            Add(textLabel);
            Add(buttonPanel);

            buttonPanel.CenterX = true;

            // sizing

            var contentMinSize = new Vector2(0.5f, 0f);
            var w = Math.Max(textLabel.Width, buttonPanel.Width);
            var h = textLabel.Height + Padding + buttonPanel.Height;
            var sz = Vector2.Max(contentMinSize, new Vector2(w, h));

            ParentAnchor = AnchorMode.Top;
            Size = (Size - ClientBounds.Size) + sz;
        }

        public async Task<MessageBoxButtons> GetResultAsync()
            => await Task.Task;

        Control createButtons(IEnumerable<MessageBoxButtons> btnTypes, Control container)
        {
            foreach(var btnType in btnTypes)
            {
                var btn = new MessageBoxButton(btnType);
                btn.MouseClick += onButtonClick;
                container.Add(btn);
            }

            return container;
        }

        void onButtonClick(Control sender, MouseButtonArgs e)
        {
            var btn = (MessageBoxButton)sender;

            if(RemoveOnButtonClick && Parent != null)
                Parent.Remove(this);
            else
                Hide();

            // a button was clicked
            Task.TrySetResult(btn.ButtonType);
            ButtonClicked?.Invoke(btn.ButtonType);
        }
    }

    public static class MessageBoxActionsExt
    {
        static readonly MessageBoxButtons[] buttonTypes =
            (MessageBoxButtons[])Enum.GetValues(typeof(MessageBoxButtons));

        public static IEnumerable<MessageBoxButtons> GetButtons(this MessageBoxButtons act)
            => buttonTypes.Where(ty => (act & ty) == ty);
    }
}
