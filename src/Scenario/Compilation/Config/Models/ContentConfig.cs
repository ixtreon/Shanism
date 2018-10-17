using Ix.Math;
using Newtonsoft.Json;
using Shanism.Common.Content;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.ScenarioLib
{
    /// <summary>
    /// Contains the models and textures declared in a scenario. 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)] // TODO: remove this shit & test
    public class ContentConfig
    {
        [JsonProperty]
        public List<TextureDef> Textures { get; set; } = new List<TextureDef>();

        [JsonProperty]
        public List<AnimationDef> Animations { get; set; } = new List<AnimationDef>();

    }
}
