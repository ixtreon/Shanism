using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client;
using Shanism.Client.UI.Scenario;
using System.Numerics;
using Shanism.Client.Views;
using Shanism.ScenarioLib;
using Shanism.Editor.Game;
using Shanism.Client.Assets;
using Shanism.Editor.Controllers;

namespace Shanism.Editor.Views
{
    class MainMenu : TitleView
    {
        Vector2 ScenarioListSize = new Vector2(1.6f, 1.2f);


        ScenarioListControl scenarioList;

        public string StartupMap { get; set; }

        protected override void OnReload()
        {
            base.OnReload();

            TitleText = "ShanoEditor";
            SubTitleText = "ggnore";

            Add(scenarioList = new ScenarioListControl
            {
                Size = ScenarioListSize,
                CenterX = true,
                Top = ContentStartY,

                ParentAnchor = AnchorMode.None,
            });

            scenarioList.ScenarioSelected += onScenarioSelected;
            scenarioList.FinishLoading += onMapsLoaded;
        }

        void onMapsLoaded(ScenarioListControl obj)
        {
            // attempt to run the startup scenario
            if (StartupMap != null)
            {
                var sc = scenarioList.Scenarios.FirstOrDefault(x => x.Name == StartupMap);
                if (sc != null)
                    onScenarioSelected(sc);
            }
        }

        void onScenarioSelected(ScenarioConfig config)
        {
            // load the map content
            var mapContent = new ContentList(Client.Screen, Client.DefaultContent.Fonts, Client.GraphicsDevice, Client.ContentLoader, config);



            // controller & events
            var game = new Controllers.EditorGameState(Client, mapContent, config);

            var view = new EditorView(game);
            ViewStack.Push(view);
        }
    }
}
