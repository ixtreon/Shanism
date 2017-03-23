using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;
using Shanism.Common.Interfaces.Objects;
using Shanism.Client.Input;

namespace Shanism.Client.UI.Tooltips
{
    /// <summary>
    /// A tooltip for them abilities.
    /// </summary>
    class AbilityTip : Control
    {
        //The size minus the height needed to write the description. 
        const double tipMargin = 2 * Padding;

        const double MaxDescWidth = 0.9;

        IAbility ability;

        public TextureFont Font { get; set; }

        readonly Label txtName;
        readonly Label txtStuff;

        public AbilityTip()
        {
            IsVisible = false;
            Font = Content.Fonts.NormalFont;
            BackColor = Color.Black.SetAlpha(25);
            Size = new Vector(0.6, 0.15);
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

        double DescriptionWidth => Size.X - 2 * tipMargin;

        protected override void OnUpdate(int msElapsed)
        {

            var newAbility = HoverControl?.ToolTip as IAbility;

            if (newAbility == null)
            {
                IsVisible = false;
                return;
            }

            if (newAbility != ability)
            {

                txtName.Text = newAbility.Name;

                if (newAbility.TargetType == AbilityTargetType.Passive)
                {
                    txtStuff.Text = string.Empty;
                }
                else
                {
                    var mc = newAbility.ManaCost;
                    var cd = newAbility.Cooldown / 1000.0;
                    var ct = newAbility.CastTime / 1000.0;
                    var cr = newAbility.CastRange;
                    if (newAbility.TargetType == AbilityTargetType.NoTarget)
                        txtStuff.Text = $"MC: {mc:0.#} CD: {cd:0.#}s CT: {ct:0.#}s";
                    else
                        txtStuff.Text = $"MC: {mc:0.#} R: {cr:0.#} CD: {cd:0.#}s CT: {ct:0.#}s";
                }



                var headerWidth = Math.Max(txtName.Size.X, txtStuff.Size.X) + tipMargin;
                var descSz = Font.MeasureString(newAbility.Description, MaxDescWidth);
                var w = Math.Max(headerWidth, descSz.X) + 2 * tipMargin;
                var h = txtStuff.Bottom + descSz.Y + 2 * tipMargin;
                Size = new Vector(w, h);

                ability = newAbility;
            }

            Location = ((MouseInfo.ScreenPosition + MouseInfo.CursorSize) / Screen.UiScale)
                .Clamp(Vector.Zero, Screen.UiSize - Size);
            IsVisible = true;
        }

        public override void OnDraw(Canvas g)
        {
            if (IsVisible)
            {
                //bg
                g.Draw(Content.Textures.Blank, Vector.Zero, Size, Color.Black.SetAlpha(150));

                //text
                g.DrawString(Font, ability.Description, Color.White, new Vector(0, txtStuff.Bottom) + tipMargin, 0, 0, maxWidth: DescriptionWidth);
            }
        }
    }
}
