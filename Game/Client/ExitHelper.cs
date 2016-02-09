using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    static class ExitHelper
    {
        static MainGame TheGame;

        public static void SetGame(MainGame theGame)
        {
            TheGame = theGame;
        }

        public static void Exit()
        {
            TheGame.Exit();
        }
    }
}
