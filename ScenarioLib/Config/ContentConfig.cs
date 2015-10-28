using IO.Content;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScenarioLib
{
    /// <summary>
    /// Contains the configuration for all the content files. 
    /// </summary>
    public class ContentConfig
    {
        public HashSet<TextureDef> Textures { get; set; } = new HashSet<TextureDef>();

        public HashSet<ModelDef> Models { get; } = new HashSet<ModelDef>();

    }
}
