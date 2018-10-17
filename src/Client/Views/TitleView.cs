using Microsoft.Xna.Framework.Input;
using Shanism.Client.IO;
using Shanism.Client.UI;
using System.Numerics;

namespace Shanism.Client.Views
{

    /// <summary>
    /// A view with a title and a subtitle.
    /// </summary>
    public class TitleView : View
    {
        static readonly Vector2 TitleLocation = new Vector2(0, 0.2f);

        readonly bool titleClickReturnsToMain;
        Label title, subTitle;

        public float ContentStartY => subTitle.Bottom + Control.LargePadding * 5;

        /// <summary>
        /// Gets or sets the text of the title.
        /// </summary>
        public string TitleText
        {
            get => title.Text;
            set => title.Text = value;
        }

        /// <summary>
        /// Gets or sets the text of the subtitle.
        /// </summary>
        public string SubTitleText
        {
            get => subTitle.Text;
            set => subTitle.Text = value;
        }

        public TitleView(bool titleClickReturnsToMain = false)
        {
            this.titleClickReturnsToMain = titleClickReturnsToMain;
        }


        protected override void OnReload()
        {
            RemoveAll();

            var titleFont = Content.Fonts.ShanoFont;
            var subTitleFont = Content.Fonts.FancyFont;

            BackColor = ColorScheme.Current.ViewBackground;

            // add a tooltip
            AddTooltip<UI.Tooltips.TextTooltip>();

            // add title, subtitle
            Add(title = new Label
            {
                ParentAnchor = AnchorMode.Top,
                Top = TitleLocation.Y,
                //CanHover = false,

                Cursor = GameCursor.ClickMe,
                AutoSize = true,

                Text = "Shanism",
                TextAlign = AnchorPoint.Center,
                Font = titleFont,

            });
            Add(subTitle = new Label
            {
                ParentAnchor = AnchorMode.Top,
                Top = title.Bottom + Control.LargePadding,
                CanHover = false,

                AutoSize = true,

                Text = string.Empty,
                TextAlign = AnchorPoint.Center,
                Font = subTitleFont,
            });

            title.CenterX = true;
            subTitle.CenterX = true;

            if (!ViewStack.IsMainView)
            {
                title.MouseClick += (o, e) => ViewStack.ResetToMain();
                title.Cursor = GameCursor.ClickMe;
            }
            else
                title.Cursor = GameCursor.Default;

            // esc closees the view
            KeyPress += (o, e) => onRootKeyPress(e.Keybind);
        }
        
        void onRootKeyPress(Keybind kb)
        {
            switch (kb.Key)
            {
                case Keys.Escape when kb.Modifiers == ModifierKeys.None:
                    if (!ViewStack.IsMainView)
                        ViewStack.Pop();
                    break;
            }
        }
    }
}
