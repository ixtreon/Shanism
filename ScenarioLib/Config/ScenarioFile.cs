using IO;
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
    /// The core of a scenario. Serialized into pure json. 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ScenarioFile
    {
        const string ScenarioFileName = "scenario.json";

        /// <summary>
        /// Gets the path to this scenario's directory. 
        /// </summary>
        public string BaseDirectory { get; protected set; }

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
        public ObjectsConfig ModelConfig { get; protected set; }

        [JsonConstructor]
        protected ScenarioFile() { }

        internal ScenarioFile(string scenarioPath)
        {
            if (!Directory.Exists(scenarioPath))
                throw new ArgumentException(nameof(scenarioPath), "The given directory does not exist: '{0}'".F(scenarioPath));

            BaseDirectory = Path.GetFullPath(scenarioPath);
            FilePath = Path.Combine(scenarioPath, ScenarioFileName);
            Name = "Shano Scenario";
            Description = "Shanistic Description";
            MapConfig = new MapConfig();
            ModelConfig = new ObjectsConfig();

            var onlyDirt = MapConfig.Map
                .ToEnumerable()
                .All(ty => ty == IO.Common.TerrainType.Dirt);

            Save();
        }


        public static ScenarioFile Load(string scenarioPath)
        {
            return Load<ScenarioFile>(scenarioPath);
        }

        /// <summary>
        /// Tries to load the config file from the given path. 
        /// </summary>
        public static T Load<T>(string scenarioPath)
            where T : ScenarioFile
        {
            var dirPath = Path.GetFullPath(scenarioPath);
            var filePath = Path.Combine(dirPath, ScenarioFileName);
            if (!File.Exists(filePath))
                return null;

            try
            {
                var datas = File.ReadAllText(filePath);
            
                var sc = JsonConvert.DeserializeObject<T>(datas);
                sc.BaseDirectory = dirPath;
                sc.FilePath = filePath;
                sc.MapConfig = sc.MapConfig ?? new MapConfig();
                sc.ModelConfig = sc.ModelConfig ?? new ObjectsConfig();

                return sc;
            }
            catch(Exception e)
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
