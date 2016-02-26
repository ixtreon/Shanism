using Client.Assets;
using Client.UI.Common;
using IO.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.Menus.Character
{
    class StatBox : Control
    {
        /*static readonly*/ Vector DefaultSize = new Vector(0.4, 0.15);

        TextureFont statNameFont = Content.Fonts.FancyFont;
        TextureFont labelNameFont = Content.Fonts.NormalFont;

        TextureFont valueFont = Content.Fonts.NormalFont;

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
            get { return mainStatLabel.ToolTip as string; }
            set { mainStatLabel.ToolTip = value; }
        }



        public StatBox()
        {
            Size = DefaultSize;
            mainStatLabel = new ValueLabel
            {
                Location = Vector.Zero,
                Size = new Vector(Size.X, statNameFont.UiHeight),
                ParentAnchor = AnchorMode.Top | AnchorMode.Left | AnchorMode.Right,

                TextFont = statNameFont,
                ValueFont = valueFont,
            };
        }

        public void AddLabel(string name, string description)
        {
            var id = secondaryStatLabels.Count;
            var lbl = new ValueLabel
            {
                AbsolutePosition = new Vector(0, mainStatLabel.Bottom + id * valueFont.UiHeight),

                Text = name,
                ToolTip = description
            };

            Add(lbl);
            secondaryStatLabels.Add(lbl);
        }


        public void SetStatValue(double baseVal, double curVal)
        {

        }

        public void SetLabelValue(int id, double baseVal, double curVal)
        {

        }
    }
}
