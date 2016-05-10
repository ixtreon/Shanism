using Shanism.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Common
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
                /// Gets the name of a default model that is always defined and uses the default object texture. 
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
                public const string Animation = "";
            }

            public const string TextureExtension = ".png";

            /// <summary>
            /// The sub-directory in the scenario folder where textures are kept. 
            /// </summary>
            public const string TexturesDirectory = "Textures";

            public const string TerrainFile = "terrain";

            /* describe the size of the terrain file */
            public const int TerrainFileSplitsX = 8;
            public const int TerrainFileSplitsY = 8;

        }

        public static class Client
        {
            //5184
            public const int WindowWidth = 96;
            public const int WindowHeight = 54;

            public static readonly Point WindowSize = new Point(WindowWidth, WindowHeight);
        }

        public static class Terrain 
        {
            /// <summary>
            /// WTF?!
            /// </summary>
            [Obsolete]
            public static readonly int ChunkSize = Client.WindowHeight / 2;
        }

        public static class Engine
        {
            public const double DamageReductionPerDefense = 0.05;
        }
    }
}
