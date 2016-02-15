using IO;
using Ionic.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
        protected const string ScenarioFileName = "scenario.json";

        /// <summary>
        /// Gets the path to this scenario's directory. 
        /// </summary>
        public string BaseDirectory { get; protected set; }

        /// <summary>
        /// Gets the path to this scenario file. 
        /// </summary>
        public string FilePath { get; protected set; }


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

        /// <summary>
        /// Gets the map configuration of the scenario. 
        /// </summary>
        [JsonProperty]
        public MapConfig MapConfig { get; protected set; }

        /// <summary>
        /// Gets the content listing of the scenario. 
        /// </summary>
        [JsonProperty]
        public ContentConfig Content { get; protected set; }
        

        [JsonConstructor]
        protected ScenarioFile() { }

        public ScenarioFile(string scenarioPath)
        {
            if (!Directory.Exists(scenarioPath))
                Directory.CreateDirectory(scenarioPath);

            BaseDirectory = Path.GetFullPath(scenarioPath);
            FilePath = Path.Combine(scenarioPath, ScenarioFileName);

            Name = "Shano Scenario";
            Description = "Shanistic Description";

            MapConfig = new MapConfig();
            Content = new ContentConfig();

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

            string datas;
            try
            {
                datas = File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                return null;
            }

            return LoadString<T>(datas, dirPath, filePath);
        }


        /// <summary>
        /// Tries to load a scenario given the byte representation of its json config. 
        /// </summary>
        /// <param name="datas">The bytes. </param>
        /// <returns>A scenario, if possible, otherwise null. </returns>
        public static ScenarioFile LoadBytes(byte[] datas)
        {
            var txt = Encoding.UTF8.GetString(datas);
            return LoadString<ScenarioFile>(txt, null, null);
        }


        /// <summary>
        /// Tries to load a scenario given the string representation of its json config. 
        /// </summary>
        /// <param name="datas">The string of its config. </param>
        /// <returns>A scenario, if possible, otherwise null. </returns>
        static T LoadString<T>(string datas, string baseDir, string filePath)
            where T : ScenarioFile
        {
            try
            {
                var sc = JsonConvert.DeserializeObject<T>(datas);
                sc.BaseDirectory = baseDir;
                sc.FilePath = filePath;
                sc.MapConfig = sc.MapConfig ?? new MapConfig();
                sc.Content = sc.Content ?? new ContentConfig();

                return sc;
            }
            catch //(Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Saves the scenario file to disk. 
        /// </summary>
        public void Save()
        {
            File.WriteAllText(FilePath, GetText(Formatting.Indented));
        }

        /// <summary>
        /// Serializes this scenario to pure json. 
        /// </summary>
        public string GetText(Formatting format = Formatting.None)
        {
            return JsonConvert.SerializeObject(this, format);
        }

        /// <summary>
        /// Gets a serialized json version of the scenario. 
        /// </summary>
        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(GetText());
        }

        byte[] zippedContent;

        /// <summary>
        /// Compresses all textures in the content to a byte array. 
        /// </summary>
        public byte[] ZipContent()
        {
            if (zippedContent == null)
            {
                var files = Content.Textures.Select(tex => new
                {
                    FileName = Path.Combine(BaseDirectory, "Content", tex.Name),
                    ArchiveName = Path.GetDirectoryName(tex.Name),
                })
                .Where(f => File.Exists(f.FileName))
                .ToArray();

                using (var zip = new ZipFile())
                {
                    foreach (var f in files)
                        zip.AddFile(f.FileName, f.ArchiveName);

                    using (var ms = new MemoryStream())
                    {
                        zip.Save(ms);
                        zippedContent = ms.ToArray();
                    }
                }
            }
            return zippedContent;
        }

        public static void UnzipContent(byte[] content, string folder)
        {
            Directory.CreateDirectory(folder);

            using (var ms = new MemoryStream(content))
            using (var zip = ZipFile.Read(ms))
            {
                foreach (var zipEntry in zip)
                    zipEntry.Extract(folder, ExtractExistingFileAction.OverwriteSilently);
            }
        }

    }
}
