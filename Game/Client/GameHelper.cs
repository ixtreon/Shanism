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

        public static void SetGame(ClientGame theGame)
        {
            TheGame = theGame;
        }

        public static void Exit()
        {
            if(TheGame != null)
                TheGame.Exit();
        }

        public static void Restart()
        {
            TheGame.Engine.RestartScenario();
        }
    }
}
