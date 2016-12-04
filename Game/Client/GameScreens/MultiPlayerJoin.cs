using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.UI;
using Shanism.Common;
using Shanism.Network.Client;

namespace Shanism.Client.GameScreens
{
    class MultiPlayerJoin : UiScreen
    {
        static readonly Vector panelSize = new Vector(0.6, 0.7);
        static readonly Vector btnSize = new Vector(panelSize.X - 2 * Control.Padding, 0.06);

        public event Action<IShanoEngine> GameStarted;

        readonly TextBox hostAddress;
        readonly Button connectButton;

        public MultiPlayerJoin(GraphicsDevice device)
            : base(device)
        {
            SubTitle = "Join Multi Player";


            var flowPanel = new FlowPanel
            {
                Width = panelSize.X,
                ParentAnchor = AnchorMode.None,
                BackColor = Color.Black.SetAlpha(100),
            };

            flowPanel.Add(new Label
            {
                Text = "Host Address:",
            });

            flowPanel.Add(hostAddress = new TextBox
            {
                BackColor = Color.White.SetAlpha(50),
                Width = btnSize.X,
            });

            flowPanel.Add(connectButton = new Button("Connect")
            {
                Size = btnSize,
            });
            connectButton.MouseUp += ConnectButton_MouseUp;


            flowPanel.AutoSize = true;
            flowPanel.CenterBoth();
            Root.Add(flowPanel);

        }

        void ConnectButton_MouseUp(Input.MouseButtonArgs obj)
        {
            if (string.IsNullOrEmpty(hostAddress.Text))
            {
                Root.Add(new MessageBox("Host", "Please enter a host address!"));
                return;
            }

            var client = new NClient(hostAddress.Text);
            GameStarted?.Invoke(client);
        }
    }
}
