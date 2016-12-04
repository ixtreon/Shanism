using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.UI;
using Shanism.Common;

namespace Shanism.Client.GameScreens
{
    class MultiPlayer : UiScreen
    {
        static readonly Vector panelSize = new Vector(0.6, 0.7);
        static readonly Vector btnSize = new Vector(panelSize.X - 2 * Control.Padding, 0.15);

        public event Action<IShanoEngine> GameStarted;

        public MultiPlayer(GraphicsDevice device)
            : base(device)
        {
            SubTitle = "Multi Player";

            Console.WriteLine("Create MP");

            var flowPanel = new FlowPanel
            {
                Width = panelSize.X,
                ParentAnchor = AnchorMode.None,
                BackColor = Color.Black.SetAlpha(100),
            };

            Button joinGame, hostGame;

            flowPanel.Add(hostGame = new Button("Host")
            {
                Size = btnSize,
            });
            hostGame.MouseUp += HostGame_MouseUp;

            flowPanel.Add(joinGame = new Button("Join")
            {
                Size = btnSize,
            });
            joinGame.MouseUp += JoinGame_MouseUp;

            flowPanel.AutoSize = true;
            flowPanel.CenterBoth();

            Root.Add(flowPanel);
        }

        void JoinGame_MouseUp(Input.MouseButtonArgs obj)
        {
            var mpScreen = new MultiPlayerJoin(device);
            mpScreen.GameStarted += onGameStarted;

            SetScreen(mpScreen);
        }

        void HostGame_MouseUp(Input.MouseButtonArgs obj)
        {
            var mpScreen = new MultiPlayerHost(device);
            mpScreen.GameStarted += onGameStarted;

            SetScreen(mpScreen);
        }

        void onGameStarted(IShanoEngine engine)
        {
            GameStarted?.Invoke(engine);
        }
    }
}
