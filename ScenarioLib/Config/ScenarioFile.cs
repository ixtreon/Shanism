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
    /// The core of a scenario. 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ScenarioBase
    {
        const string ScenarioFileName = "scenario.json";

        /// <summary>
        /// Gets the path to this scenario's directory. 
        /// </summary>
        public string ScenarioDirectory { get; protected set; }

        /// <summary>
        /// Gets the path to this scenario file. 
        /// </summary>
        public string FilePath { get; protected set; }

        /// <summary>
        /// A custom flag indicating whether the scenario has changed. 
        /// </summary>
        public bool IsDirty { get; set; }

        /// <summary>
        /// Gets the name of the scenario. 
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// Gets the description of the scenario. 
        /// </summary>
        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public MapConfig MapConfig { get; protected set; }

        [JsonProperty]
        public ContentConfig ModelConfig { get; protected set; }


        protected ScenarioBase() { }

        public ScenarioBase CreateNew(string scenarioPath)
        {
            var sc = new ScenarioBase
            {
                ScenarioDirectory = Path.GetFullPath(scenarioPath),
                FilePath = Path.Combine(ScenarioDirectory, ScenarioFileName),
                Name = "Shano Scenario",
                Description = "Shanistic Description",
                MapConfig = new MapConfig(),
                ModelConfig = new ContentConfig()
            };
            sc.Save();
            return sc;
        }


        public static ScenarioBase Load(string scenarioPath)
        {
            return Load<ScenarioBase>(scenarioPath);
        }

        /// <summary>
        /// Tries to load the config file from the given path. 
        /// </summary>
        public static T Load<T>(string scenarioPath)
            where T : ScenarioBase
        {
            var dirPath = Path.GetFullPath(scenarioPath);
            var filePath = Path.Combine(dirPath, ScenarioFileName);
            if (!File.Exists(filePath))
                return null;

            try
            {
                var datas = File.ReadAllText(filePath);
            
                var sc = JsonConvert.DeserializeObject<T>(datas);
                sc.ScenarioDirectory = dirPath;
                sc.FilePath = filePath;
                sc.MapConfig = sc.MapConfig ?? new MapConfig();
                sc.ModelConfig = sc.ModelConfig ?? new ContentConfig();

                return sc;
            }
            catch
            {
                return null;
            }
        }

        public void Save()
        {
            var datas = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(FilePath, datas);
        }
    }
}
