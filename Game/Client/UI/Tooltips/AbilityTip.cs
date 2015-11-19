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
using Client.Assets.Fonts;

namespace Client.UI.Tooltips
{
    class AbilityTip : Control
    {
        //The size minus the height needed to write the description. 
        static readonly Vector BaseSize = new Vector(0.6, 0.15);

        IAbility Ability;

        public TextureFont Font { get; set; }

        readonly Label txtName = new Label
        {
            Location = new Vector(0.02),
            Text = "lalalala",
        };

        readonly Label txtStuff = new Label
        {
            Location = new Vector(0.02, 0.10),
            Text = "lalalal",
            Font = Content.Fonts.MediumFont,
            TextColor = Color.White,
        };
        

        public AbilityTip()
        {
            Font = Content.Fonts.MediumFont;
            BackColor = Color.Black.SetAlpha(25);

            Add(txtName);
            Add(txtStuff);
        }
        
        public override void Update(int msElapsed)
        {
            var ability = HoverControl?.ToolTip as IAbility;

            //continue if visible
            Visible = (ability != null);
            if (!Visible)
                return;


            Ability = ability;

            txtName.Text = ability.Name;
            txtStuff.Text = "MC: {0}  R: {1}  CD: {2}  CT: {3}".F(ability.ManaCost, ability.CastRange, ability.Cooldown, ability.CastTime);

            var descriptionMaxWidth = BaseSize.X - Padding * 4;
            var desc = Font.MeasureStringUi(ability.Description, descriptionMaxWidth);

            Size = new Vector(Math.Max(BaseSize.X, desc.X), BaseSize.Y + 4 * Padding + desc.Y);
            var screenPos =
                (mouseState.Position.ToPoint() + new Point(14, 26))
                .Clamp(Point.Zero, Screen.Size - this.ScreenSize);
            AbsolutePosition = Screen.ScreenToUi(screenPos);
        }

        public override void Draw(Graphics g)
        {
            if (Visible)
            {
                //bg
                g.Draw(Content.Textures.Blank, Vector.Zero, Size, Color.Black.SetAlpha(150));

                //text
                g.DrawString(Font, Ability.Description, Color.White, new Vector(Padding, BaseSize.Y + Padding) , 0, 0, txtMaxWidth: BaseSize.X - 2 * Padding);
            }
        }
    }
}
