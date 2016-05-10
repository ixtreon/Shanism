using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.Textures;
using Color = Microsoft.Xna.Framework.Color;
using Shanism.Common.Game;
using Shanism.Client.Assets;
using Shanism.Common;

namespace Shanism.Client.UI.Common
{
    /// <summary>
    /// A button that shows an image and/or text and can be clicked. 
    /// </summary>
    class Button : Control
    {
        bool _isSelected = false;

        /// <summary>
        /// Gets or sets the texture that is drawn stretched on the button. 
        /// </summary>
        public Texture2D Texture { get; set; }


        public event Action<Button> Selected;

        /// <summary>
        /// Gets the texture color (tint). 
        /// </summary>
        public Color TextureColor { get; set; } = Color.White;

        public string Text { get; set; }

        public TextureFont Font { get; set; } = Content.Fonts.NormalFont;

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
        public bool IsSelected
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

        public Button(string text = null, Texture2D texture = null)
        {
            var sz = Font.MeasureStringUi("WOWyglj");
            Size = new Vector(0.05f, sz.Y);

            Text = text;
            Texture = texture;

            BackColor = Color.YellowGreen;

            MouseDown += Button_MouseDown;
        }

        void Button_MouseDown(Input.MouseButtonArgs obj)
        {
            if (CanSelect && !IsSelected)
                select(true);
        }

        void select(bool val)
        {
            _isSelected = val;
            Selected?.Invoke(this);
        }

        public override void OnDraw(Graphics g)
        {
            //draw background
            g.Draw(Content.Textures.Blank, Vector.Zero, Size, BackColor);
            if(MouseOver)
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
                var border = Content.Textures.TryGetIcon("border_hover");
                var tint = MouseOver ? Color.White : new Color(32, 32, 32);
                tint = (IsSelected && CanSelect) ? Color.Gold.Brighten(20) : tint;

                g.Draw(border, Vector.Zero, Size, tint);
            }
        }

    }
}
