using System;
using System.Collections.Generic;
using System.Text;

namespace Shanism.Client.Game
{
    static class Settings
    {
        public static GameSettings Current { get; } = new GameSettings("shanism.client.json");


    }
}
