using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;
using System.Numerics;

namespace Shanism.Client.UI.Menus.Character
{
    class StatBox : Control
    {
        static readonly Vector2 DefaultSize = new Vector2(0.4f, 0.15f);

        Font mainLabelFont => Content.Fonts.FancyFont;
        Font childLabelFont => Content.Fonts.NormalFont;
        Font valueFont => Content.Fonts.NormalFont;

        

        readonly ValueLabel mainLabel;
        readonly List<ValueLabel> childLabels = new List<ValueLabel>();


        public string MainText
        {
            get => mainLabel.Text;
            set => mainLabel.Text = value;
        }

        public string MainTooltip
        {
            get => mainLabel.TextToolTip as string;
            set => mainLabel.TextToolTip = value;
        }



        public StatBox()
        {
            Size = DefaultSize;
            BackColor = UiColors.ControlBackground;

            var font = mainLabelFont;
            mainLabel = new ValueLabel
            {
                CanHover = true,

                Location = Vector2.Zero,
                Size = new Vector2(Size.X, font.Height + 2 * Padding),
                ParentAnchor = AnchorMode.Top | AnchorMode.Left | AnchorMode.Right,


                Font = font,
                ValueFont = valueFont,
            };
            Add(mainLabel);
        }

        public void AddLabel(string name, string description)
        {
            const float SecondaryIndent = 0.05f;

            var font = childLabelFont;
            var id = childLabels.Count;
            var lbl = new ValueLabel
            {
                CanHover = true,

                Location = new Vector2(SecondaryIndent, mainLabel.Bottom + id * font.Height),
                Size = new Vector2(Size.X - SecondaryIndent, font.Height),
                ParentAnchor = AnchorMode.Top | AnchorMode.Left | AnchorMode.Right,


                Font = font,
                ValueFont = valueFont,

                Text = name,
                TextToolTip = description,
            };

            Add(lbl);
            childLabels.Add(lbl);
        }

        public void Resize()
            => Size = new Vector2(Size.X, (childLabels.LastOrDefault() ?? mainLabel).Bottom + Padding);

        public void SetMainValue(double baseVal, double curVal, string suffix = "")
            => setStat(mainLabel, baseVal, curVal, suffix);

        public void SetChildValue(int id, double baseVal, double curVal, string suffix = "")
            => setStat(childLabels[id], baseVal, curVal, suffix);

        public void SetChildValue(int id, string text, string tooltip)
        {
            childLabels[id].Value = text;
            childLabels[id].ValueToolTip = tooltip;
        }

        static void setStat(ValueLabel lbl, double baseVal, double curVal, string suffix)
        {
            var dVal = curVal - baseVal;
            lbl.Value = curVal.ToString("0");
            if (baseVal > 0)
                lbl.ValueToolTip = $"{baseVal} + {dVal}{suffix}";
            else
                lbl.ValueToolTip = $"{curVal}{suffix}";
        }
    }
}
