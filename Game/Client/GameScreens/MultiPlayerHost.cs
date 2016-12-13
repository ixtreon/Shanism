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
    class MultiPlayerHost : UiScreen
    {
        static readonly Vector panelSize = new Vector(0.6, 0.7);
        static readonly Vector btnSize = new Vector(panelSize.X - 2 * Control.Padding, 0.15);

        public MultiPlayerHost(GraphicsDevice device)
            : base(device)
        {
            SubTitle = "Host Multi Player";


            var flowPanel = new FlowPanel
            {
                Width = panelSize.X,
                ParentAnchor = AnchorMode.None,
                BackColor = Color.Black.SetAlpha(100),
            };


            var tempLabel = new Label()
            {
                Text = "For the time being, please use the provided standalone server.exe in the root game directory.",
                AutoSize = false,
                Size = new Vector(Screen.UiSize.X / 2, 0.2),
            };

            tempLabel.CenterBoth();
            Root.Add(tempLabel);
        }
    }
}
