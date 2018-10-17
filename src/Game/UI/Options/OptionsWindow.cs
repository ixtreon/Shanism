using Shanism.Client.Game;
using Shanism.Client.UI.Containers;
using Shanism.Common;
using System;
using System.Numerics;

namespace Shanism.Client.UI.Menus
{
    class OptionsWindow : Window
    {

        // view
        readonly ListPanel mainPanel;
        readonly Button btnApply;

        // view: graphics
        readonly CheckBox vSync;
        readonly CheckBox fullScreen;
        readonly SliderLabel renderSize;
        readonly SliderLabel maxFps;

        // view: game
        readonly CheckBox extendCast;

        public OptionsWindow()
        {

            Size = new Vector2(0.72f, 1f);
            MinimumSize = new Vector2(0.5f, 0.85f);

            Padding = 2 * DefaultPadding;
            CanFocus = false;
            Location = (Screen.UiSize - Size) / 2;
            ParentAnchor = AnchorMode.None;
            TitleText = "Options";
            Shown += onShown;

            Add(btnApply = new Button
            {
                Text = "Apply",

                Right = ClientBounds.Right,
                Bottom = ClientBounds.Bottom,
                ParentAnchor = AnchorMode.Right | AnchorMode.Bottom,
            });

            Add(mainPanel = new ListPanel(Direction.TopDown)
            {
                BackColor = Color.Transparent,
                Padding = DefaultPadding,

                Location = ClientBounds.Position,
                Size = ClientBounds.Size - new Vector2(0, btnApply.Height + Padding),
                ParentAnchor = AnchorMode.All,
            });

            btnApply.MouseClick += saveAndHide;

            var rowSize = new Vector2(mainPanel.ClientBounds.Width, 0.07f);

            addHeader("Graphics");
            {
                mainPanel.Add(vSync = new CheckBox
                {
                    Text = "VSync",
                    ToolTip = "Synchronizes redraws with the screen refresh rate.",

                    BackColor = Color.Red,

                    Size = rowSize,
                    ParentAnchor = AnchorMode.Horizontal | AnchorMode.Top,
                });
                mainPanel.Add(fullScreen = new CheckBox
                {
                    Text = "Full Screen",
                    ToolTip = "Makes the game span the whole screen.",

                    Size = rowSize,
                    ParentAnchor = AnchorMode.Horizontal | AnchorMode.Top,
                });
                mainPanel.Add(renderSize = new SliderLabel
                {
                    Text = "Render Scale",
                    ToolTip = "The ratio between the draw buffer size and the window.",

                    BackColor = Color.Red,

                    Size = rowSize,
                    ParentAnchor = AnchorMode.Horizontal | AnchorMode.Top,

                    SizeMode = SplitSizeMode.FixedFirst,
                    SplitAt = 0.5f,

                    Font = Content.Fonts.NormalFont,
                    Color = UiColors.Text,

                    MinValue = 0.05f,
                    MaxValue = 1.00f,
                    ValueStep = 0.05f,
                });
                mainPanel.Add(maxFps = new SliderLabel
                {
                    Text = "FPS Limit",
                    ToolTip = "The maximum FPS the game could run at.",

                    Size = rowSize,
                    ParentAnchor = AnchorMode.Horizontal | AnchorMode.Top,

                    SizeMode = SplitSizeMode.FixedFirst,
                    SplitAt = 0.5f,

                    Font = Content.Fonts.NormalFont,
                    Color = UiColors.Text,

                    MinValue = 30,
                    MaxValue = 300,
                    ValueStep = 5,
                });

            }

            addHeader("Game");
            {
                mainPanel.Add(extendCast = new CheckBox
                {
                    Text = "Extend Cast",
                    ToolTip = "Allow casting even if the mouse cursor is out of range.",

                    Size = rowSize,
                    ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,
                });

                //mainPanel.Add(quickPress = new CheckBox
                //{
                //    Size = new Vector(mainPanel.Size.X - 2 * Padding, 0.07),
                //    ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,

                //    Text = "Quick Press",
                //    ToolTip = "Cast spells on key press, rather than key release",
                //});

            }

            vSync.CheckedChanged += (_) => Settings.Current.VSync = vSync.IsChecked;
            fullScreen.CheckedChanged += (_) => Settings.Current.FullScreen = fullScreen.IsChecked;
            renderSize.ValueChanged += (_) => Settings.Current.RenderScale = renderSize.Value.RoundToNearest(0.05f);
            maxFps.ValueChanged += (_) => Settings.Current.MaxFps = maxFps.Value.RoundToNearest(1);
            extendCast.CheckedChanged += e => Settings.Current.ExtendCast = extendCast.IsChecked;
            //quickPress.CheckedChanged += e => Settings.Current.QuickButtonPress = quickPress.IsChecked;
        }

        void addHeader(string name)
        {
            mainPanel.Add(new Label
            {
                Size = new Vector2(mainPanel.Size.X - 2 * Padding, 0.1f),
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,

                TextAlign = AnchorPoint.Center,
                Text = name,
            });
        }

        void onShown(Control sender, EventArgs e)
        {
            Settings.Current.ReloadFromDisk();
            loadSettings();
        }

        void loadSettings()
        {
            maxFps.Value         = Settings.Current.MaxFps;
            renderSize.Value     = Settings.Current.RenderScale;
            vSync.IsChecked      = Settings.Current.VSync;
            fullScreen.IsChecked = Settings.Current.FullScreen;
            extendCast.IsChecked = Settings.Current.ExtendCast;
        }

        void saveAndHide(Control sender, MouseButtonArgs obj)
        {
            Settings.Current.Save();
            IsVisible = false;
        }

    }
}
