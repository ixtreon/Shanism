using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Common;
using Shanism.ScenarioLib;
using Shanism.Client.UI;

namespace Shanism.Client.GameScreens
{
    class SinglePlayer : UiScreen
    {
        readonly ScenarioListControl scenarioList;

        public SinglePlayer(GameComponent game) 
            : base(game)
        {
            SubTitle = "Single Player";

            var listSz = new Vector(1.6, 1.2);
            Root.Add(scenarioList = new ScenarioListControl
            {
                Size = listSz,
                Top = ContentStartY,

                ParentAnchor = AnchorMode.None,
                BackColor = Color.Black.SetAlpha(100),
            });
            scenarioList.CenterX();
            scenarioList.ScenarioSelected += ScenarioList_ScenarioSelected;

            Root.MouseDown += Root_MouseDown;
        }

        void Root_MouseDown(Input.MouseButtonArgs obj)
        {
            //var newSz = obj.Position - scenarioList.Location;
            //scenarioList.Size = newSz;
        }

        void ScenarioList_ScenarioSelected(ScenarioConfig sc)
        {
            var eng = new Engine.ShanoEngine();

            string errors;
            if (!eng.TryLoadScenario(sc.BaseDirectory, 0, out errors))
            {
                Root.ShowMessageBox("Compilation Error", errors);
                return;
            }

            StartGame(eng);
        }
    }
}
