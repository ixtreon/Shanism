using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client
{
    //Provides hacky methods for manipulating the main game - exit, restart.
    static class GameHelper
    {
        static ClientGame TheGame;

        public static event Action QuitToTitle;

        public static void SetGame(ClientGame theGame)
        {
            TheGame = theGame;
        }

        public static void Quit()
        {
            QuitToTitle?.Invoke();
        }

        public static void Exit()
        {
            TheGame?.Exit();
        }

        public static void Restart()
        {
            TheGame.GameScreen.RestartScenario();
        }
    }
}
