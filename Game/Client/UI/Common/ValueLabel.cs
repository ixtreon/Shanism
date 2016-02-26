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

        public ValueLabel()
        {
            lblText = new Label
            {
                AutoSize = false,
                Location = Vector.Zero,
                Size = Size,
                ParentAnchor = AnchorMode.Top | AnchorMode.Bottom | AnchorMode.Left,

                CanHover = false,
                TextXAlign = 0,
            };

            lblValue = new Label
            {
                AutoSize = false,
                Location = Vector.Zero,
                Size = Size,
                ParentAnchor = AnchorMode.Top | AnchorMode.Bottom | AnchorMode.Right,

                CanHover = false,
                TextXAlign = 1,
            };

            Add(lblText);
            Add(lblValue);
        }
    }
}
