using Shanism.Client.UI;
using Shanism.Client.UI.Containers;
using Shanism.Client.Views;
using System.Numerics;

namespace Shanism.Client.Game.Views
{
    class MultiPlayerHost : TitleView
    {
        static readonly Vector2 panelSize = new Vector2(0.6f, 0.7f);
        static readonly Vector2 btnSize = new Vector2(panelSize.X - 2 * Control.DefaultPadding, 0.15f);

        protected override void OnReload()
        {
            base.OnReload();

            SubTitleText = "Host Multi Player";

            var flowPanel = new ListPanel
            {
                Width = panelSize.X,
                ParentAnchor = AnchorMode.None,
            };


            var tempLabel = new Label
            {
                Font = Content.Fonts.LargeFont,
                Text = "For the time being, please use the provided standalone server.exe in the root game directory.",
                
                Size = new Vector2(Screen.UiSize.X / 2, 0.2f),
                
                CenterX = true,
                Top = 0.5f,
            };

            Add(tempLabel);
        }
    }
}
