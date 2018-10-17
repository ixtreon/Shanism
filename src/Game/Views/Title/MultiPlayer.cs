using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Shanism.Client.Views;
using Shanism.Client.UI;
using Shanism.Client.UI.Containers;

namespace Shanism.Client.Game.Views
{
    class MultiPlayer : TitleView
    {
        static readonly Vector2 panelSize = new Vector2(0.6f, 0.7f);
        static readonly Vector2 btnSize = new Vector2(panelSize.X - 2 * DefaultPadding, 0.15f);

        TitleView mpHost, mpJoin;


        protected override void OnReload()
        {
            base.OnReload();

            SubTitleText = "Multi Player";

            // create menu
            var flowPanel = new ListPanel(Direction.TopDown, sizeMode: ListSizeMode.ResizeBoth)
            {
                Left = Screen.UiSize.X / 2,
                Top = ContentStartY,
                //ControlSpacing = new Vector2(0.1f),

                ParentAnchor = AnchorMode.Top,
            };

            // create menu buttons
            Button joinGame, hostGame;
            flowPanel.Add(hostGame = new Button("Host")
            {
                Size = btnSize,
            });

            flowPanel.Add(joinGame = new Button("Join")
            {
                Size = btnSize,
            });

            Add(flowPanel);

            //create sub-screens
            mpJoin = new MultiPlayerJoin();
            mpHost = new MultiPlayerHost();

            // link buttons to screens
            hostGame.MouseClick += (o, e) => ViewStack.Push(mpHost);
            joinGame.MouseClick += (o, e) => ViewStack.Push(mpJoin);
        }

    }
}
