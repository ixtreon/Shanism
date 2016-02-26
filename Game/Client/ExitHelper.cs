using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    static class ExitHelper
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
    }
}
