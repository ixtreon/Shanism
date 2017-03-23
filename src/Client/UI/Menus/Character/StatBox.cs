using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;

namespace Shanism.Client.UI.Menus.Character
{
    class StatBox : Control
    {
        static readonly Vector DefaultSize = new Vector(0.4, 0.15);

        TextureFont mainLabelFont => Content.Fonts.FancyFont;
        TextureFont childLabelFont => Content.Fonts.NormalFont;

        TextureFont valueFont => Content.Fonts.NormalFont;


        Color TextColor = Color.Gold;
        Color ValueColor = Color.White;


        readonly ValueLabel mainLabel;
        readonly List<ValueLabel> childLabels = new List<ValueLabel>();


        public string MainText
        {
            get { return mainLabel.Text; }
            set { mainLabel.Text = value; }
        }

        public string MainTooltip
        {
            get { return mainLabel.TextToolTip as string; }
            set { mainLabel.TextToolTip = value; }
        }



        public StatBox()
        {
            Size = DefaultSize;
            BackColor = Color.Black.SetAlpha(50);

            var font = mainLabelFont;
            mainLabel = new ValueLabel
            {
                CanHover = true,

                Location = Vector.Zero,
                Size = new Vector(Size.X, font.HeightUi + 2 * Padding),
                ParentAnchor = AnchorMode.Top | AnchorMode.Left | AnchorMode.Right,


                TextFont = font,
                ValueFont = valueFont,
            };
            Add(mainLabel);
        }

        public void AddLabel(string name, string description)
        {
            const double SecondaryIndent = 0.05;

            var font = childLabelFont;
            var id = childLabels.Count;
            var lbl = new ValueLabel
            {
                CanHover = true,

                Location = new Vector(SecondaryIndent, mainLabel.Bottom + id * font.HeightUi),
                Size = new Vector(Size.X - SecondaryIndent, font.HeightUi),
                ParentAnchor = AnchorMode.Top | AnchorMode.Left | AnchorMode.Right,


                TextFont = font,
                ValueFont = valueFont,

                Text = name,
                TextToolTip = description,
            };

            Add(lbl);
            childLabels.Add(lbl);
        }

        public void Resize()
            => Size = new Vector(Size.X, (childLabels.LastOrDefault() ?? mainLabel).Bottom + Padding);

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
