using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO
{
    public static class Constants
    {
        public static class Client
        {
            public const int WindowWidth = 24;
            public const int WindowHeight = 15;

            public static readonly Point WindowSize = new Point(WindowWidth, WindowHeight);
        }

        public static class Map
        {
            public static readonly Point ChunkSize = new Point(Client.WindowHeight / 2, Client.WindowHeight / 2);
        }

        public static class Engine
        {
            public const double DamageReductionPerDefense = 0.05;
        }
    }
}
