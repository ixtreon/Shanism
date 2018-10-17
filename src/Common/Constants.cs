using Ix.Math;

namespace Shanism.Common
{
    public static class Constants
    {
        /// <summary>
        /// Info about terrain, textures, icon, animation files.
        /// </summary>
        public static class Content
        {
            public static class DefaultValues
            {

                /// <summary>
                /// Gets the name of a default object texture that is always defined. 
                /// </summary>
                public static string ObjectTexture { get; } = "dummy";

                /// <summary>
                /// Gets the name of a default model that is always defined and uses the default object texture. 
                /// </summary>
                public static string ModelName { get; } = "dummy";

                /// <summary>
                /// Gets the path to the default model's texture on the client. 
                /// </summary>
                public static string ModelTexture { get; } = "objects/dummy.png";

                /// <summary>
                /// Gets the name of the placeholder icon.
                /// </summary>
                public static string Icon { get; } = "default";

                /// <summary>
                /// The name of the animation used by default with all models. 
                /// </summary>
                public static string Animation { get; } = "";
            }

            public static string TextureExtension { get; } = ".png";

            /// <summary>
            /// The sub-directory in the scenario folder where textures are kept. 
            /// </summary>
            public static string TexturesDirectory { get; } = "Textures";
        }

        public static class Terrain
        {
            public static string FileName { get; } = "terrain";
            public static Point LogicalSize { get; } = new Point(8, 8);
        }

        public static class Animations
        {
            public static string Move { get; } = "move";
            public static string Cast { get; } = "cast";
        }

        public static class Client
        {
            //TODO: dependant on default zoom?

            /// <summary>
            /// The size of the terrain chunks requested by the client. 
            /// </summary>
            public static int TerrainChunkSize { get; } = 32;
        }

        public static class Entities
        {
            /// <summary>
            /// The default size of a unit. 
            /// </summary>
            public static float DefaultScale { get; } = 2.5f;

            /// <summary>
            /// The minimum size of an entity. 
            /// </summary>
            public static float MinScale { get; } = 0.1f;

            /// <summary>
            /// The maximum size of an entity. 
            /// </summary>
            public static float MaxScale { get; } = 20;
        }
    }
}
