using IO.Content;
using ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Assets
{
    /// <summary>
    /// Lists all models and textures used in a game. 
    /// </summary>
    class ContentList
    {
        /// <summary>
        /// Gets the dictionary that contains all textures keyed by their name. 
        /// </summary>
        public Dictionary<string, TextureDef> TextureDict { get; } = new Dictionary<string, TextureDef>();
        /// <summary>
        /// Gets the dictionary that contains all models keyed by their name. 
        /// </summary>
        public Dictionary<string, ModelDef> ModelDict { get; } = new Dictionary<string, ModelDef>();

        /// <summary>
        /// Adds the models and textures from the given scenario configuration to this list. 
        /// </summary>
        public void Parse(ContentConfig content)
        {
            Parse(content.Textures);
            Parse(content.Models);
        }

        /// <summary>
        /// Adds the given textures to this list. 
        /// </summary>
        public void Parse(IEnumerable<TextureDef> textures)
        {
            foreach (var tex in textures)
                TextureDict[tex.Name.ToLower()] = tex;
        }

        /// <summary>
        /// Adds the given models to this list. 
        /// </summary>
        public void Parse(IEnumerable<ModelDef> models)
        {
            foreach (var m in models)
                ModelDict[m.Name.ToLower()] = m;
        }
    }
}
