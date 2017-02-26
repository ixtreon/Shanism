using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Client.Drawing;
using Shanism.Common;

namespace Shanism.Client.UI
{
    /// <summary>
    /// A button that shows an image and/or text and can be clicked. 
    /// </summary>
    class Button : Control
    {
        public static readonly Vector DefaultSize = new Vector(0.15, 0.06);

        bool _isSelected = false;

        /// <summary>
        /// Gets or sets the texture that is drawn stretched on the button. 
        /// </summary>
        public IconSprite Texture { get; set; }


        public event Action<Button> Selected;
        public event Action<Button> Deselected;

        /// <summary>
        /// Gets the texture color (tint). 
        /// </summary>
        public Color TextureColor { get; set; } = Color.White;

        public string Text { get; set; }

        public TextureFont Font { get; set; }

        public Color FontColor { get; set; } = Color.Black;

        /// <summary>
        /// The color painted on top of the background color when the mouse is over the button. 
        /// </summary>
        public Color HoverOverlayColor { get; set; } = Color.Black.SetAlpha(32);

        /// <summary>
        /// Gets or sets whether this button has a border drawn around it. 
        /// </summary>
        public bool HasBorder { get; set; }

        /// <summary>
        /// Gets or sets whether this button is currently selected (toggled). 
        /// </summary>
        public virtual bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                    select(value);
            }
        }

        /// <summary>
        /// Gets or sets whether this button can be selected (toggled). 
        /// </summary>
        public bool CanSelect { get; set; }

        public Button(string text = null, IconSprite icon = null)
        {
            Font = Content.Fonts.NormalFont;
            Size = DefaultSize;

            Text = text;
            Texture = icon;

            BackColor = Color.YellowGreen;

            MouseDown += Button_MouseDown;
        }

        void Button_MouseDown(Input.MouseButtonArgs obj)
        {
            if (CanSelect && !IsSelected)
                IsSelected = true;
        }

        void select(bool val)
        {
            _isSelected = val;
            if(val)
                Selected?.Invoke(this);
            else
                Deselected?.Invoke(this);
        }

        public override void OnDraw(Canvas g)
        {
            //draw background
            g.Draw(Content.Textures.Blank, Vector.Zero, Size, BackColor);
            if (HasHover)
                g.Draw(Content.Textures.Blank, Vector.Zero, Size, HoverOverlayColor);

            //draw texture
            if (Texture != null)
                g.Draw(Texture, Vector.Zero, Size, TextureColor);

            //draw text
            if (!string.IsNullOrEmpty(Text))
                g.DrawString(Font, Text, FontColor, Size / 2, 0.5f, 0.5f);

            //draw border
            if (HasBorder)
            {
                var border = Content.Icons.IconBorderHover;
                var tint = HasHover ? Color.White : new Color(32, 32, 32);
                tint = (CanSelect && IsSelected) ? Color.Gold.Brighten(20) : tint;

                g.Draw(border, Vector.Zero, Size, tint);
            }
        }

    }
}
