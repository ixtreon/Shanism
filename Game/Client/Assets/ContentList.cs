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
    /// Lists all models and textures found in one or more <see cref="ContentConfig"/> files. 
    /// </summary>
    class ContentList
    {
        public Dictionary<string, TextureDef> TextureDict { get; } = new Dictionary<string, TextureDef>();
        public Dictionary<string, ModelDef> ModelDict { get; } = new Dictionary<string, ModelDef>();

        /// <summary>
        /// Loads the models and textures from the lists to the dictionaries,
        /// to allow for easier look-up of objects. 
        /// </summary>
        public void Parse(ContentConfig content)
        {
            Parse(content.Textures);
            Parse(content.Models);
        }

        public void Parse(IEnumerable<TextureDef> textures)
        {
            foreach (var tex in textures)
                TextureDict[tex.Name.ToLower()] = tex;
        }

        public void Parse(IEnumerable<ModelDef> models)
        {
            foreach (var m in models)
                ModelDict[m.Name.ToLower()] = m;
        }
    }
}
