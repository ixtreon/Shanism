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

        public event Action<IShanoEngine> GameStarted;

        public SinglePlayer(GraphicsDevice device) 
            : base(device)
        {
            SubTitle = "Single Player";

            var listSz = new Vector(1.6, 1.2);
            Root.Add(scenarioList = new ScenarioListControl
            {
                Size = listSz,
                Location = new Vector((Root.Size.X - listSz.X) / 2, 0.7),
                ParentAnchor = UI.AnchorMode.None,
                BackColor = Color.Black.SetAlpha(100),
            });

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
                Root.Add(new MessageBox("Error",
                    $"There was an error compiling the scenario:\n\n{errors}"));
                return;
            }

            GameStarted?.Invoke(eng);
        }
    }
}
