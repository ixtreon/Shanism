using Shanism.Client.UI;
using Shanism.Client.UI.Containers;
using Shanism.Client.Views;
using Shanism.Common;
using Shanism.Network.Client;
using System;
using System.Numerics;

namespace Shanism.Client.Game.Views
{
    class MultiPlayerJoin : TitleView
    {
        static readonly Vector2 panelSize = new Vector2(0.6f, 0.7f);
        static readonly Vector2 btnSize = new Vector2(panelSize.X - 2 * Control.DefaultPadding, 0.06f);

        TextBar hostAddress;
        Button connectButton;

        protected override void OnReload()
        {
            base.OnReload();

            SubTitleText = "Join Multi Player";

            var flowPanel = new ListPanel(Direction.TopDown, sizeMode: ListSizeMode.ResizeBoth)
            {
                ParentAnchor = AnchorMode.None,
                CenterBoth = true,
            };

            flowPanel.Add(new Label
            {
                Font = Content.Fonts.NormalFont,
                Text = "Host:",
                AutoSize = true,
            });

            flowPanel.Add(hostAddress = new TextBar
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

            Add(flowPanel);
        }

        void ConnectButton_MouseClick(Control sender, MouseButtonArgs args)
        {
            var target = hostAddress.Text;

            if (string.IsNullOrEmpty(target))
            {
                ShowMessageBox("Host", "Please enter a host address.", true);
                return;
            }

            // update the user settings
            Settings.Current.Servers.Add(target);
            Settings.Current.Servers.SetLastPlayed(target);
            Settings.Current.Save();

            //// try connecting..
            //var engine = new NetworkEngine(target);
            //Game.Context.StartPlaying(engine);

            throw new NotImplementedException();
        }
    }
}
