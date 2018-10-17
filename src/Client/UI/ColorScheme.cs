using Shanism.Common;

namespace Shanism.Client
{
    public class ColorScheme
    {
        public static ColorScheme Current { get; set; } = Fantasy;

        public static ColorScheme Fantasy { get; } = new ColorScheme
        {
            ViewBackground = new Color(48, 24, 48),
            ControlBackground = Color.Black.SetAlpha(100),
            HoverOverlay = Color.Black.SetAlpha(32),
            Border = Color.Black,

            WindowBackground = Color.SaddleBrown,
            WindowInactiveTitle = Color.SaddleBrown.Darken(30),
            WindowActiveTitle = Color.SaddleBrown.Darken(40),
            ButtonDanger = Color.DarkRed,

            TextTitle = Color.Goldenrod,
            Text = Color.White,
            DisabledText = Color.FromGrayscale(192),
            SelectionBackground = Color.AliceBlue.SetAlpha(150),

            ButtonText = Color.Black,
            Button = Color.YellowGreen,
            ButtonConfirm = Color.Red,

        };

        public static ColorScheme Dark { get; } = new ColorScheme
        {
            ViewBackground = new Color(32, 16, 16),
            ControlBackground = Color.Black.SetAlpha(200),
            HoverOverlay = Color.White.SetAlpha(32),
            Border = Color.White.Darken(10),

            WindowBackground = new Color(48, 48, 48, 212),
            WindowInactiveTitle = new Color(139, 24, 24),
            WindowActiveTitle = Color.DarkRed,
            ButtonDanger = Color.Transparent,

            TextTitle = Color.AliceBlue,
            Text = Color.White.Darken(10),
            DisabledText = Color.FromGrayscale(192),
            SelectionBackground = Color.White.Darken(20),

            ButtonText = Color.LightGray,
            //ButtonBackground    = Color.DarkRed.SetAlpha(200),
            Button = new Color(225, 32, 32, 200),
            ButtonConfirm = Color.DarkRed,
        };


        public Color ViewBackground { get; set; }

        /// <summary>
        /// The color of a control's background.
        /// </summary>
        public Color ControlBackground { get; set; }

        /// <summary>
        /// A default background color when a control is hovered.
        /// </summary>
        public Color HoverOverlay { get; set; }

        /// <summary>
        /// Gets the color of borders, e.g. of windows or checkboxes.
        /// </summary>
        public Color Border { get; set; }

        /// <summary>
        /// Gets the background color of a UI window.
        /// </summary>
        public Color WindowBackground { get; set; }

        /// <summary>
        /// Gets the color of the inactive windows' title bar.
        /// </summary>
        public Color WindowInactiveTitle { get; set; }

        /// <summary>
        /// Gets the color of the active window's title bar.
        /// </summary>
        public Color WindowActiveTitle { get; set; }


        /// <summary>
        /// The color of the main text.
        /// </summary>
        public Color Text { get; set; }
        public Color DisabledText { get; set; }

        /// <summary>
        /// The color of title text.
        /// </summary>
        public Color TextTitle { get; set; }

        /// <summary>
        /// The color of a text selection.
        /// </summary>
        public Color SelectionBackground { get; set; }

        public Color ButtonText { get; set; }
        public Color Button { get; set; }
        public Color DisabledButton { get; set; }
        public Color ButtonConfirm { get; set; }

        /// <summary>
        /// Gets the background color of the close button of a window.
        /// </summary>
        public Color ButtonDanger { get; set; }
    }
}
