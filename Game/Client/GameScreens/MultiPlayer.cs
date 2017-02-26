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


        public MultiPlayer(GraphicsDevice device, ContentList content)
            : base(device, content)
        {
            SubTitle = "Multi Player";

            var flowPanel = new FlowPanel
            {
                Top = ContentStartY,

                AutoSize = true,

                ParentAnchor = AnchorMode.None,
                BackColor = Color.Black.SetAlpha(100),
            };

            Button joinGame, hostGame;

            flowPanel.Add(hostGame = new Button("Host")
            {
                Size = btnSize,
            });
            hostGame.MouseClick += HostGame_MouseClick;

            flowPanel.Add(joinGame = new Button("Join")
            {
                Size = btnSize,
            });
            joinGame.MouseClick += JoinGame_MouseClick;

            flowPanel.CenterX();

            Root.Add(flowPanel);

            //create sub-screens
            mpJoin = new MultiPlayerJoin(device, Content);
            mpJoin.GameStarted += StartGame;

            mpHost = new MultiPlayerHost(device, Content);
            mpHost.GameStarted += StartGame;
        }

        UiScreen mpHost, mpJoin;

        void JoinGame_MouseClick(Input.MouseButtonArgs obj)
        {
            SetScreen(mpJoin);
        }

        void HostGame_MouseClick(Input.MouseButtonArgs obj)
        {
            SetScreen(mpHost);
        }
    }
}
