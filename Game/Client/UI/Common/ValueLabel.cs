using Client.Assets;
using Client.Input;
using Client.UI.Common;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace Client.UI.Common
{
    class ValueLabel : Control
    {
        readonly Label lblText;
        readonly Label lblValue;

        public string Text
        {
            get { return lblText.Text; }
            set { lblText.Text = value; }
        }

        public string Value
        {
            get { return lblValue.Text; }
            set { lblValue.Text = value; }
        }

        public object TextToolTip
        {
            get { return lblText.ToolTip; }
            set { lblText.ToolTip = value; }
        }

        public object ValueToolTip
        {
            get { return lblValue.ToolTip; }
            set { lblValue.ToolTip = value; }
        }

        public TextureFont TextFont
        {
            get { return lblText.Font; }
            set { lblText.Font = value; }
        }

        public TextureFont ValueFont
        {
            get { return lblValue.Font; }
            set { lblValue.Font = value; }
        }

        public new Color BackColor
        {
            get { return lblText.BackColor; }
            set { lblText.BackColor = lblValue.BackColor = value; }
        }

        public ValueLabel()
        {
            const double ValueWidth = 0.2;
            Size = new Vector(0.5, 0.10);
            lblText = new Label
            {
                AutoSize = false,
                Location = Vector.Zero,
                Size = Size,
                ParentAnchor = AnchorMode.All,

                CanHover = true,
                TextXAlign = 0,
            };

            lblValue = new Label
            {
                AutoSize = false,
                Location = new Vector(Size.X - ValueWidth, 0),
                Size = new Vector(ValueWidth, Size.Y),
                ParentAnchor = AnchorMode.Top | AnchorMode.Right | AnchorMode.Bottom,

                CanHover = true,
                TextXAlign = 1,
            };

            Add(lblText);
            Add(lblValue);
        }
    }
}
