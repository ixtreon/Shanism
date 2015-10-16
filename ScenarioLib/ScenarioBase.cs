using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScenarioLib
{
    /// <summary>
    /// A stub structure used to locate and identify scenarios. 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ScenarioBase
    {
        const string ScenarioFileName = "scenario.json";

        /// <summary>
        /// Gets the name of the scenario. 
        /// </summary>
        [JsonProperty]
        public string Name { get; protected set; }

        /// <summary>
        /// Gets the description of the scenario. 
        /// </summary>
        [JsonProperty]
        public string Description { get; protected set; }


        [JsonProperty]
        public HashSet<string> IncludedFiles { get; private set; }

        public string FilePath { get; private set; }


        public static ScenarioBase Load(string scenarioPath)
        {
            return LoadAs<ScenarioBase>(scenarioPath);
        }

        public static T LoadAs<T>(string scenarioPath)
            where T : ScenarioBase
        {
            var fullPath = Path.GetFullPath(Path.Combine(scenarioPath, ScenarioFileName));
            if (!File.Exists(fullPath))
                return null;

            var datas = File.ReadAllText(fullPath);
            var sc = JsonConvert.DeserializeObject<T>(datas);
            sc.FilePath = fullPath;
            return sc;
        }

        public void Save()
        {
            var datas = JsonConvert.SerializeObject(this);
            File.WriteAllText(FilePath, datas);
        }
    }
}
