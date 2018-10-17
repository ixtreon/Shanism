using Client.Game.Models;
using Shanism.Client.Game.Systems;
using Shanism.Client.UI.Scenario;
using Shanism.Client.Views;
using Shanism.ScenarioLib;
using System.Linq;
using System.Numerics;

namespace Shanism.Client.Game.Views
{
    class SinglePlayer : TitleView
    {

        EngineStarter gameStarter;

        ScenarioListControl scenarioList;

        public string StartupMap { get; set; }

        protected override void OnReload()
        {
            base.OnReload();
            SubTitleText = "Single Player";

            var listSz = new Vector2(1.6f, 1.2f);
            Add(scenarioList = new ScenarioListControl
            {
                Size = listSz,
                CenterX = true,
                Top = ContentStartY,

                ParentAnchor = AnchorMode.None,
            });
            scenarioList.ScenarioSelected += StartMap;
            scenarioList.FinishLoading += (_) => TryRunStartupMap();

            gameStarter = new EngineStarter(this, engine =>
            {
                var client = new ShanoReceptor(Settings.Current.PlayerName, engine);
                var gameContext = new ShanismGameState(Client, client);

                var gameView = new GameView(gameContext);

                ViewStack.Push(gameView);
            });
        }

        void TryRunStartupMap()
        {
            if (string.IsNullOrWhiteSpace(StartupMap))
                return;

            var scenario = scenarioList.Scenarios.FirstOrDefault(x => x.Name == StartupMap);
            if (scenario == null)
                return;

            StartMap(scenario);
        }


        void StartMap(ScenarioConfig scenario)
        {
            gameStarter.TryLoadWithUI(scenario.BaseDirectory);
        }

    }
}
