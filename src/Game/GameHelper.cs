using System;

namespace Shanism.Client
{
    //Provides hacky methods for manipulating the main game - quit, exit, restart
    static class GameHelper
    {
        public static Action QuitToTitle;
        public static Action ExitGame;

        public static void Quit() => QuitToTitle?.Invoke();
        public static void Exit() => ExitGame?.Invoke();
    }
}
