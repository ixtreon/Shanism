using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Client.Textures;
using IO.Common;
using Color = Microsoft.Xna.Framework.Color;
using IO.Objects;
using Client.UI.Common;
using IO;
using Client.Assets;

namespace Client.UI.Tooltips
{
    class AbilityTip : Control
    {
        //The size minus the height needed to write the description. 
        static readonly Vector BaseSize = new Vector(0.6, 0.15);

        IAbility Ability;

        public TextureFont Font { get; set; }

        readonly Label txtName;
        readonly Label txtStuff;

        public AbilityTip()
        {
            Font = Content.Fonts.NormalFont;
            BackColor = Color.Black.SetAlpha(25);
            Size = BaseSize;
            CanHover = false;

            txtName = new Label
            {

                Location = new Vector(Padding),
                Text = "lalalala",
                
                CanHover = false,
                Font = Content.Fonts.FancyFont,
                TextColor = Color.Goldenrod,
            };

            txtStuff = new Label
            {
                Location = new Vector(Padding, txtName.Bottom),
                Text = "lalalal",

                CanHover = false,
                Font = Content.Fonts.NormalFont,
                TextColor = Color.White,
            };


            Add(txtName);
            Add(txtStuff);
        }
        
        protected override void OnUpdate(int msElapsed)
        {
            var ability = HoverControl?.ToolTip as IAbility;

            //continue if visible
            IsVisible = (ability != null);
            if (!IsVisible)
                return;


            Ability = ability;

            txtName.Text = ability.Name;
            txtStuff.Text = $"MC: {ability.ManaCost} " 
                + $"R: {ability.CastRange} "
                + $"CD: {(ability.Cooldown / 1000.0).ToString("0.0")}s "
                + $"CT: {(ability.CastTime / 1000.0).ToString("0.0")}s"; 

            var headerSz = Math.Max(txtName.Font.MeasureStringUi(txtName.Text).X, txtStuff.Font.MeasureStringUi(txtStuff.Text).X) + 2 * Padding;
            var descriptionMaxWidth = Size.X - Padding * 2;
            var descSz = Font.MeasureStringUi(ability.Description, descriptionMaxWidth);
            var screenPos = (mouseState.Position.ToPoint() + new Point(14, 26))
                .Clamp(Point.Zero, Screen.Size - ScreenSize);

            Size = new Vector(Math.Max(headerSz, descSz.X) + 2 * Padding, BaseSize.Y + 4 * Padding + descSz.Y);
            AbsolutePosition = Screen.ScreenToUi(screenPos);

            BringToFront();
        }

        public override void OnDraw(Graphics g)
        {
            if (IsVisible)
            {
                //bg
                g.Draw(Content.Textures.Blank, Vector.Zero, Size, Color.Black.SetAlpha(150));

                //text
                g.DrawString(Font, Ability?.Description, Color.White, new Vector(Padding, txtStuff.Bottom + Padding) , 0, 0, txtMaxWidth: Size.X - 2 * Padding);
            }
        }
    }
}
