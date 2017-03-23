using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client;
using Shanism.Client.UI;
using Shanism.Client.UI.Scenario;
using Shanism.Common;

namespace Shanism.Editor.Screens
{
    class MainScreen : TitleScreen
    {
        readonly ScenarioListControl scenarioList;

        public MainScreen(IShanoComponent game)
            : base(game)
        {
            TitleText = "ShanoEditor";
            SubTitleText = "GG kurvi";

            var listSz = new Vector(1.6, 1.2);
            Root.Add(scenarioList = new ScenarioListControl
            {
                Size = listSz,
                Top = ContentStartY,

                ParentAnchor = AnchorMode.None,
                BackColor = Color.Black.SetAlpha(100),
            });
        }
    }
}
