using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO
{
    public static class Constants
    {
        public static class Content
        {
            /// <summary>
            /// Gets the name of a default object texture that is always present. 
            /// </summary>
            public const string DefaultObjectTexture = "dummy";

            /// <summary>
            /// Gets the name of a default model that is always defined. 
            /// </summary>
            public const string DefaultModelName = "dummy";

            /// <summary>
            /// Gets the path to the default model's texture on the client. 
            /// </summary>
            public const string DefaultModelTexture = "objects\\dummy.png";


            public const string DefaultIcon = "default";

            public const string TextureExtension = ".png";

            /// <summary>
            /// The name of the animation used by default with all models. 
            /// </summary>
            public const string DefaultAnimation = "stand";

            public const string TerrainFile = "terrain";

            public const int TerrainFileSplitsX = 8;
            public const int TerrainFileSplitsY = 8;

        }

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

            public static readonly double MaximumObjectSize = 3;
        }
    }
}
