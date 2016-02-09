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
            public static class DefaultValues
            {
                public const string UnitName = "Dummy Unit";

                /// <summary>
                /// Gets the name of a default object texture that is always defined. 
                /// </summary>
                public const string ObjectTexture = "dummy";

                /// <summary>
                /// Gets the name of a default model that is always defined. 
                /// </summary>
                public const string ModelName = "dummy";

                /// <summary>
                /// Gets the path to the default model's texture on the client. 
                /// </summary>
                public const string ModelTexture = "objects\\dummy.png";


                public const string Icon = "default";

                /// <summary>
                /// The name of the animation used by default with all models. 
                /// </summary>
                public const string Animation = "stand";
            }

            public const string TextureExtension = ".png";

            public const string TerrainFile = "terrain";

            public const int TerrainFileSplitsX = 8;
            public const int TerrainFileSplitsY = 8;

        }

        public static class Client
        {
            //144x90
            public const int WindowWidth = 20;
            public const int WindowHeight = 12;

            public static readonly Point WindowSize = new Point(WindowWidth, WindowHeight);
        }

        public static class Terrain
        {
            public static readonly int ChunkSize = Client.WindowHeight / 2;
        }

        public static class Engine
        {
            public const double DamageReductionPerDefense = 0.05;

            public static readonly double MaximumObjectSize = 3;
        }
    }
}
