using Ix.Math;
using Shanism.Common;
using Shanism.Common.Objects;
using System;
using System.Numerics;
using System.Text;

namespace Shanism.Client.UI.Tooltips
{
    /// <summary>
    /// A tooltip for them abilities.
    /// </summary>
    public class AbilityTooltip : TooltipBase
    {
        const float MaxDescWidth = 0.9f;


        (
            uint Id,
            string Description,
            int ManaCost,
            float Cooldown,
            float CastTime,
            float CastRange
        ) AbilityData;

        public Font Font { get; set; }

        float maxTextWidth;

        readonly Label txtName;
        readonly Label txtSubtitle;

        public AbilityTooltip()
        {
            Font = Content.Fonts.NormalFont;
            Size = new Vector2(0.6f, 0.15f);
            CanHover = false;
            BackColor = UiColors.ControlBackground;
            Hide();

            txtName = new Label
            {
                Location = ClientBounds.Position,
                AutoSize = true,

                CanHover = false,
                Font = Content.Fonts.FancyFont,
                TextColor = UiColors.TextTitle,
                Text = "Shano Ability",
            };

            txtSubtitle = new Label
            {
                Location = new Vector2(Padding, txtName.Bottom),
                AutoSize = true,

                CanHover = false,
                Font = Content.Fonts.NormalFont,
                TextColor = UiColors.Text,
                Text = "Shano Description",
            };


            Add(txtName);
            Add(txtSubtitle);
        }

        public override void Update(int msElapsed)
        {
            var newAbility = HoverControl?.ToolTip as IAbility;

            IsVisible = (newAbility != null);
            if (!IsVisible)
                return;

            var newData = AbilityData;
            newData.Id = newAbility.Id;
            newData.Description = newAbility.Description;
            newData.ManaCost = newAbility.ManaCost;
            newData.Cooldown = newAbility.Cooldown / 1000f;
            newData.CastTime = newAbility.CastTime / 1000f;
            newData.CastRange = newAbility.CastRange;

            if (newData.Equals(AbilityData))
            {
                base.Update(msElapsed);
                return;
            }

            //update view
            AbilityData = newData;
            txtName.Text = newAbility.Name;
            txtSubtitle.Text = getSubtitleText(newAbility);

            // update size
            var headerWidth = Math.Max(txtName.Size.X, txtSubtitle.Size.X) + LargePadding;
            maxTextWidth = Math.Max(headerWidth, MaxDescWidth);

            var textSize = Font.MeasureString(newAbility.Description, maxTextWidth);
            var w = Math.Max(headerWidth, textSize.X) + 2 * LargePadding;
            var h = txtSubtitle.Bottom + textSize.Y + 2 * LargePadding;
            Size = new Vector2(w, h);

            base.Update(msElapsed);
        }


        string getSubtitleText(IAbility ab)
        {
            if (ab.TargetType == AbilityTargetType.Passive)
                return string.Empty;

            var text = new StringBuilder();

            text.Append($"MC: {AbilityData.ManaCost:0.#} ");
            if (ab.TargetType != AbilityTargetType.NoTarget)
                text.Append($"R: {AbilityData.CastRange:0.#} ");
            text.Append($"CD: {AbilityData.Cooldown:0.#}s ");
            if (AbilityData.CastTime > 0)
                text.Append($"CT: {AbilityData.CastTime:0.#}s ");
            text.Length--;

            return text.ToString();
        }

        public override void Draw(Canvas g)
        {
            base.Draw(g);

            //description
            g.DrawString(Font, AbilityData.Description, UiColors.Text,
                new Vector2(0, txtSubtitle.Bottom).Offset(LargePadding),
                maxWidth: maxTextWidth);
        }
    }
}
