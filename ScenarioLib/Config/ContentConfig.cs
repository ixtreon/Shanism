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
        public List<TextureDef> Textures { get; } = new List<TextureDef>();

        public List<ModelDef> Models { get; } = new List<ModelDef>();

    }
}
