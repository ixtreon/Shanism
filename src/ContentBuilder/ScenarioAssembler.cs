using Shanism.ScenarioLib;
using System.Collections.Generic;
using System.Linq;

namespace IconPacker
{
    class ScenarioAssembler : List<ContentBlob>
    {

        public ScenarioConfig CreateConfig(string baseDirectory,
            string author = "Ix",
            string name = "ShanoContent",
            string description = "Default content for the Shanism game")
            => new ScenarioConfig(baseDirectory)
            {
                Author = author,
                Name = name,
                Description = description,

                Map = new MapConfig
                {
                    Terrain = new Shanism.Common.TerrainType[0, 0],
                },

                Content = this.Aggregate(new ContentConfig(), ImportBlob),
            };

        ContentConfig ImportBlob(ContentConfig config, ContentBlob blob)
        {
            config.Textures.Add(blob.Texture);
            config.Animations.AddRange(blob.Animations);
            return config;
        }
    }
}
