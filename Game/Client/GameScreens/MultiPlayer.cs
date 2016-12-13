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


        public MultiPlayer(GraphicsDevice device)
            : base(device)
        {
            SubTitle = "Multi Player";

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

            //create sub-screens
            mpJoin = new MultiPlayerJoin(device);
            mpJoin.GameStarted += onGameStarted;

            mpHost = new MultiPlayerHost(device);
            mpHost.GameStarted += onGameStarted;
        }

        UiScreen mpHost, mpJoin;

        void JoinGame_MouseUp(Input.MouseButtonArgs obj)
        {
            SetScreen(mpJoin);
        }

        void HostGame_MouseUp(Input.MouseButtonArgs obj)
        {
            SetScreen(mpHost);
        }

        void onGameStarted(IShanoEngine engine)
        {
            StartGame(engine);
        }
    }
}
