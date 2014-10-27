using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using IO.Common;

namespace Engine
{
    class Constants
    {
        public static class ClientParams
        {
            public const int WindowWidth = 16;
            public const int WindowHeight = 10;

            public static readonly Point WindowSize = new Point(WindowWidth, WindowHeight);
        }
    }
}
