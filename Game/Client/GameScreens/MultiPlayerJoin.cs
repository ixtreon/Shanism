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
                BackColor = Color.Transparent,
            };

            flowPanel.Add(new Label
            {
                Text = "Host Address:",
            });

            flowPanel.Add(hostAddress = new TextBox
            {
                BackColor = Color.Black.SetAlpha(100),
                Width = btnSize.X,
            });
            hostAddress.Text = Settings.Current.Servers.LastPlayed;

            flowPanel.Add(connectButton = new Button("Connect")
            {
                Size = btnSize,
            });
            connectButton.MouseClick += ConnectButton_MouseClick;


            flowPanel.AutoSize = true;
            flowPanel.CenterBoth();
            Root.Add(flowPanel);

        }

        void ConnectButton_MouseClick(Input.MouseButtonArgs obj)
        {
            var server = hostAddress.Text;

            if (string.IsNullOrEmpty(server))
            {
                Root.ShowMessageBox("Host", "Please enter a host address.");
                return;
            }

            Settings.Current.Servers.Add(server);
            Settings.Current.Servers.SetLastPlayed(server);
            Settings.Current.Save();

            NClient client;
            if (!NClient.TryConnect(server, out client))
            {
                Root.ShowMessageBox("Host", "The selected host is unreachable.");
                return;
            }
                
            StartGame(client);
        }
    }
}
