using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client
{
    //Provides hacky methods for manipulating the main game - quit, exit, restart
    static class GameHelper
    {
        public static event Action QuitToTitle;
        public static event Action ExitGame;
        public static event Action RestartScenario;

        public static void Quit() => QuitToTitle?.Invoke();
        public static void Exit() => ExitGame?.Invoke();
        public static void Restart() => RestartScenario?.Invoke();
    }
}
