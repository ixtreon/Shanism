using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shanism.Client.Drawing;
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
        static readonly Color BackColor = new Color(48, 24, 48);
        static readonly Vector TitleLocation = new Vector(0, 0.2);


        public virtual Control Root { get; } = new Control
        {
            BackColor = BackColor,
        };


        readonly Label title;
        readonly Label subTitle;

        UiScreen subScreen;

        Control actualRoot
            => subScreen?.actualRoot ?? Root;

        public event Action Closed;

        public event Action<IShanoEngine> GameStarted;

        public double ContentStartY { get; }

        public string SubTitle
        {
            get { return subTitle.Text; }
            set { subTitle.Text = value; }
        }

        public UiScreen(GraphicsDevice device, ContentList content)
            : base(device, content)
        {
            var titleFont = Content.Fonts.ShanoFont;
            var SubTitleFont = Content.Fonts.FancyFont;

            Root.Maximize();
            Root.Add(title = new Label
            {
                Font = titleFont,
                Text = "Shanism",

                ParentAnchor = AnchorMode.Left | AnchorMode.Right,

                Size = new Vector(Screen.UiSize.X, titleFont.HeightUi),
                Location = TitleLocation,
                TextXAlign = 0.5f,

            });

            Root.Add(subTitle = new Label
            {
                Text = string.Empty,
                Font = SubTitleFont,

                ParentAnchor = AnchorMode.Left | AnchorMode.Right,

                Size = new Vector(Screen.UiSize.X, SubTitleFont.HeightUi),
                Location = new Vector(0, title.Bottom + Control.LargePadding),
                TextXAlign = 0.5f,

                AutoSize = false,
            });

            Root.Add(new UI.Tooltips.SimpleTip());

            Root.KeyPressed += root_KeyActivated;

            ContentStartY = subTitle.Bottom + Control.LargePadding * 5;
        }

        public void StartGame(IShanoEngine game) 
            => GameStarted?.Invoke(game);

        void root_KeyActivated(Keybind k)
        {
            if (k.Key == Keys.Escape)
            {
                Closed?.Invoke();
            }
        }

        public override void Draw()
        {
            Canvas.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.PointClamp, DepthStencilState.DepthRead,
                RasterizerState.CullNone);
            actualRoot.Draw(Canvas);
            Canvas.End();

            base.Draw();
        }

        public override void Update(int msElapsed)
        {
            actualRoot.Maximize();
            actualRoot.UpdateMain(msElapsed);
            actualRoot.Update(msElapsed);

            base.Update(msElapsed);
        }

        /// <summary>
        /// Sets the child screen.
        /// </summary>
        protected virtual void SetScreen(UiScreen newScreen)
        {
            subScreen = newScreen;
            subScreen.Closed += ResetSubScreen;
        }

        /// <summary>
        /// Resets the child screen.
        /// </summary>
        protected void ResetSubScreen()
            => subScreen = null;
    }
}
