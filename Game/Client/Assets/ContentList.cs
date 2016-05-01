using IO.Content;
using IO.Util;
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
    public class ContentList
    {
        /// <summary>
        /// Gets the dictionary that contains all textures keyed by their name. 
        /// </summary>
        public Dictionary<string, TextureDef> Textures { get; } = new Dictionary<string, TextureDef>();
        
        /// <summary>
        /// Gets the dictionary that contains all models keyed by their name. 
        /// </summary>
        public Dictionary<string, AnimationDef> Animations { get; } = new Dictionary<string, AnimationDef>();

        /// <summary>
        /// Adds the models and textures from the given scenario configuration to this list. 
        /// </summary>
        public void Parse(ContentConfig content)
        {
            Parse(content.Textures);
            Parse(content.Animations);
        }

        /// <summary>
        /// Adds the given textures to this list. 
        /// </summary>
        public void Parse(IEnumerable<TextureDef> textures)
        {
            foreach (var tex in textures)
                Textures[tex.Name.ToLower()] = tex;
        }

        /// <summary>
        /// Adds the given models to this list. 
        /// </summary>
        public void Parse(IEnumerable<AnimationDef> animations)
        {
            foreach (var a in animations)
                Animations[AnimPath.Normalize(a.Name)] = a;
        }
    }
}
