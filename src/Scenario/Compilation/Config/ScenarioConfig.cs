using Shanism.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.IO.Compression;

namespace Shanism.ScenarioLib
{
    /// <summary>
    /// The core of a scenario. Serialized into json. 
    /// Contains the name, description, content and model data for the scenario. 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ScenarioConfig
    {
        protected const string ScenarioFileName = "scenario.json";

        /// <summary>
        /// Gets the path to this scenario's directory. 
        /// </summary>
        public string BaseDirectory { get; protected set; }

        /// <summary>
        /// Gets the path to this scenario file. 
        /// </summary>
        public string FilePath => Path.Combine(BaseDirectory, ScenarioFileName);

        /// <summary>
        /// Gets the name of the scenario. 
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Author { get; set; }

        /// <summary>
        /// Gets the description of the scenario. 
        /// </summary>
        [JsonProperty]
        public string Description { get; set; }

        /// <summary>
        /// Gets the map configuration of the scenario. 
        /// </summary>
        [JsonProperty]
        public MapConfig Map { get; set; }

        /// <summary>
        /// Gets the content listing of the scenario. 
        /// </summary>
        [JsonProperty]
        public ContentConfig Content { get; set; }



        [JsonConstructor]
        [Obsolete("", true)]
        public ScenarioConfig() { }

        public ScenarioConfig(string baseDirectory)
        {
            BaseDirectory = baseDirectory;
        }

        ///// <summary>
        ///// Creates a new scenario.json at the given path. 
        ///// </summary>
        ///// <param name="scenarioPath"></param>
        //public ScenarioConfig(string scenarioPath)
        //{
        //    if(!Directory.Exists(scenarioPath))
        //        Directory.CreateDirectory(scenarioPath);

        //    BaseDirectory = Path.GetFullPath(scenarioPath);

        //    Name = "Shano Scenario";
        //    Description = "Shanistic Description";

        //    Map = new MapConfig();
        //    Content = new ContentConfig();

        //    SaveToDisk();
        //}

        public void SetLocation(string scenarioDir)
        {
            BaseDirectory = Path.GetFullPath(scenarioDir);
        }

        /// <summary>
        /// Saves the scenario file to the disk. 
        /// </summary>
        public void SaveToDisk()
        {
            var txt = SaveToString();
            File.WriteAllText(FilePath, txt);
        }

        /// <summary>
        /// Serializes this scenario to a json string. 
        /// </summary>
        public string SaveToString(Formatting format = Formatting.None)
        {
            return JsonConvert.SerializeObject(this, format);
        }

        /// <summary>
        /// Serializes this scenario then puts it in a byte array. 
        /// </summary>
        public byte[] SaveToBytes()
        {
            return Encoding.UTF8.GetBytes(SaveToString());
        }

        byte[] zippedContent;

        /// <summary>
        /// Compresses all scenario assets to a byte array. 
        /// </summary>
        public byte[] ZipContent(bool forceRebuild = false)
        {
            if (zippedContent != null && !forceRebuild)
                return zippedContent;

            using (var ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                    foreach (var tex in Content.Textures)
                    {
                        var fileName = Path.Combine(BaseDirectory, Constants.Content.TexturesDirectory, tex.Name);
                        if (File.Exists(fileName))
                            archive.CreateEntryFromFile(fileName, tex.Name);
                    }

                zippedContent = ms.ToArray();
            }

            return zippedContent;
        }

    }
}
