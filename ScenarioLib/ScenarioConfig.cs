﻿using IO;
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
        public MapConfig Map { get; protected set; }

        /// <summary>
        /// Gets the content listing of the scenario. 
        /// </summary>
        [JsonProperty]
        public ContentConfig Content { get; protected set; }


        [JsonConstructor]
        protected ScenarioConfig() { }

        /// <summary>
        /// Creates a new scenario.json at the given path. 
        /// </summary>
        /// <param name="scenarioPath"></param>
        public ScenarioConfig(string scenarioPath)
        {
            if (!Directory.Exists(scenarioPath))
                Directory.CreateDirectory(scenarioPath);

            BaseDirectory = Path.GetFullPath(scenarioPath);
            FilePath = Path.Combine(scenarioPath, ScenarioFileName);

            Name = "Shano Scenario";
            Description = "Shanistic Description";

            Map = new MapConfig();
            Content = new ContentConfig();

            Save();
        }


        /// <summary>
        /// Tries to load the config file from the given path. 
        /// </summary>
        public static ScenarioConfig Load(string scenarioPath, out string errors)
        {
            var dirPath = Path.GetFullPath(scenarioPath);
            var filePath = Path.Combine(dirPath, ScenarioFileName);
            if (!File.Exists(filePath))
            {
                errors = $"Unable to find the `{ScenarioFileName}` file. ";
                return null;
            }

            string datas;
            try
            {
                datas = File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                errors = e.Message;
                return null;
            }

            errors = string.Empty;
            return LoadFromString(datas, dirPath, filePath);
        }


        /// <summary>
        /// Tries to load a scenario given the byte representation of its json config. 
        /// </summary>
        /// <param name="datas">The bytes. </param>
        /// <returns>A scenario, if possible, otherwise null. </returns>
        public static ScenarioConfig LoadFromBytes(byte[] datas)
        {
            var txt = Encoding.UTF8.GetString(datas);
            return LoadFromString(txt, null, null);
        }


        /// <summary>
        /// Tries to load a scenario given the string representation of its json config. 
        /// </summary>
        /// <param name="datas">The string of its config. </param>
        /// <returns>A scenario, if possible, otherwise null. </returns>
        static ScenarioConfig LoadFromString(string datas, string baseDir, string filePath)
        {
            try
            {
                var sc = JsonConvert.DeserializeObject<ScenarioConfig>(datas);
                sc.BaseDirectory = baseDir;
                sc.FilePath = filePath;
                sc.Map = sc.Map ?? new MapConfig();
                sc.Content = sc.Content ?? new ContentConfig();

                return sc;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Saves the scenario file to the disk. 
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
                    FileName = Path.Combine(BaseDirectory, Constants.Content.TexturesDirectory, tex.Name),
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