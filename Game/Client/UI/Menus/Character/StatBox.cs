using Shanism.Client.Drawing;
using Shanism.Client.UI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;

namespace Shanism.Client.UI.Menus.Character
{
    class StatBox : Control
    {
        static readonly Vector DefaultSize = new Vector(0.4, 0.15);

        TextureFont statNameFont => Content.Fonts.FancyFont;
        TextureFont labelNameFont => Content.Fonts.NormalFont;

        TextureFont valueFont => Content.Fonts.NormalFont;


        Color TextColor = Color.Gold;
        Color ValueColor = Color.White;


        readonly ValueLabel mainStatLabel;
        readonly List<ValueLabel> secondaryStatLabels = new List<ValueLabel>();


        public string StatText
        {
            get { return mainStatLabel.Text; }
            set { mainStatLabel.Text = value; }
        }

        public string StatTooltip
        {
            get { return mainStatLabel.TextToolTip as string; }
            set { mainStatLabel.TextToolTip = value; }
        }



        public StatBox()
        {
            Size = DefaultSize;
            BackColor = Color.Black.SetAlpha(50);

            mainStatLabel = new ValueLabel
            {
                CanHover = true,

                Location = Vector.Zero,
                Size = new Vector(Size.X, statNameFont.HeightUi + 2 * Padding),
                ParentAnchor = AnchorMode.Top | AnchorMode.Left | AnchorMode.Right,


                TextFont = statNameFont,
                ValueFont = valueFont,
            };
            Add(mainStatLabel);
        }

        public void AddLabel(string name, string description)
        {
            double SecondaryIndent = 0.05;

            var id = secondaryStatLabels.Count;
            var lblHeight = statNameFont.HeightUi;
            var lbl = new ValueLabel
            {
                CanHover = true,

                Location = new Vector(SecondaryIndent, mainStatLabel.Bottom + id * lblHeight),
                Size = new Vector(Size.X - SecondaryIndent, lblHeight),
                ParentAnchor = AnchorMode.Top | AnchorMode.Left | AnchorMode.Right,


                TextFont = statNameFont,
                ValueFont = valueFont,

                Text = name,
                TextToolTip = description,
            };

            Add(lbl);
            secondaryStatLabels.Add(lbl);
        }

        public void Resize()
            => Size = new Vector(Size.X, (secondaryStatLabels.LastOrDefault() ?? mainStatLabel).Bottom + Padding);

        public void SetStatValue(double baseVal, double curVal, string suffix = "")
            => setVal(mainStatLabel, baseVal, curVal, suffix);

        public void SetLabelValue(int id, double baseVal, double curVal, string suffix = "")
            => setVal(secondaryStatLabels[id], baseVal, curVal, suffix);

        public void SetLabelValue(int id, string text, string tooltip)
        {
            secondaryStatLabels[id].Value = text;
            secondaryStatLabels[id].ValueToolTip = tooltip;
        }

        static void setVal(ValueLabel lbl, double baseVal, double curVal, string suffix)
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
