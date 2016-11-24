using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shanism.Client.Input;
using Shanism.Client.UI;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.GameScreens
{
    abstract class UiScreen : GameScreen
    {
        public virtual Control Root { get; } = new Control
        {
            BackColor = new Color(48, 24, 48),
        };

        readonly Label title;
        readonly Label subTitle;

        protected readonly GraphicsDevice device;
        readonly Graphics g;

        public event Action Closed;

        public string SubTitle
        {
            get { return subTitle.Text; }
            set { subTitle.Text = value; }
        }

        public UiScreen(GraphicsDevice device)
        {
            this.device = device;
            g = new Graphics(device);

            Root.Maximize();
            var titleFont = Control.Content.Fonts.ShanoFont;
            Root.Add(title = new Label
            {
                Font = titleFont,
                Text = "Shanism",

                ParentAnchor = AnchorMode.Left | AnchorMode.Right,

                Size = new Vector(Screen.UiSize.X, titleFont.HeightUi),
                Location = new Vector(0, 0.3),
                TextXAlign = 0.5f,

            });

            var subTitleFont = Control.Content.Fonts.FancyFont;
            Root.Add(subTitle = new Label
            {
                //BackColor = Color.Gray,

                Text = string.Empty,
                Font = subTitleFont,

                ParentAnchor = AnchorMode.Left | AnchorMode.Right,

                Size = new Vector(Screen.UiSize.X, subTitleFont.HeightUi),
                Location = new Vector(0, title.Bottom + Control.LargePadding),
                TextXAlign = 0.5f,

                AutoSize = false,
            });

            Root.Add(new UI.Tooltips.SimpleTip());

            Root.KeyPressed += root_KeyActivated;
        }

        void root_KeyActivated(Input.Keybind k)
        {
            if (k.Key == Keys.Escape)
            {
                Closed?.Invoke();
            }
        }

        public override void Draw()
        {
            g.Begin();

            Root.Draw(g);

            g.End();
        }

        public override void Update(int msElapsed)
        {
            g.Bounds = new RectangleF(Vector.Zero, Screen.UiSize);

            Root.Maximize();
            Root.UpdateMain(msElapsed);
            Root.Update(msElapsed);
        }
    }
}
