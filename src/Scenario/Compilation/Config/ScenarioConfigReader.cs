using Client.Common.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.ScenarioLib
{
    /// <summary>
    /// Reads scenario configuration files
    /// and outputs <see cref="ScenarioConfig"/> objects.
    /// </summary>
    public class ScenarioConfigReader
    {

        protected const string ScenarioFileName = "scenario.json";

        static ScenarioConfigReader()
        {
            JsonConfig.Initialize();
        }

        /// <summary>
        /// Tries to load the config file from the given path. 
        /// </summary>
        public LoadResult<ScenarioConfig> TryReadFromDisk(string scenarioDir)
        {
            var configPath = Path.GetFullPath(Path.Combine(scenarioDir, ScenarioFileName));

            string fileContent;
            try
            {
                fileContent = File.ReadAllText(configPath);
            }
            catch(Exception e)
            {
                return LoadResult<ScenarioConfig>.Fail(e.Message);
            }

            return tryParseString(fileContent, scenarioDir);
        }

        /// <summary>
        /// Tries to load a scenario given the byte representation of its json config. 
        /// </summary>
        /// <param name="bytes">The bytes. </param>
        /// <returns>A scenario, if possible, otherwise null. </returns>
        public LoadResult<ScenarioConfig> TryReadBytes(byte[] bytes, string scenarioDir)
        {
            var text = Encoding.UTF8.GetString(bytes);
            return tryParseString(text, scenarioDir);
        }

        public bool TryExtractContent(byte[] content, string outDir)
        {
            outDir = Path.GetFullPath(outDir);

            // recreate the content directory
            if(Directory.Exists(outDir))
                Directory.Delete(outDir, true);
            Directory.CreateDirectory(outDir);

            // unzip the content
            try
            {
                using(var ms = new MemoryStream(content))
                using(var archive = new ZipArchive(ms, ZipArchiveMode.Read, true))
                    archive.ExtractToDirectory(outDir);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Tries to load a scenario given the string representation of its json config. 
        /// </summary>
        /// <param name="fileContent">The string of its config. </param>
        /// <returns>A scenario, if possible, otherwise null. </returns>
        static LoadResult<ScenarioConfig> tryParseString(string fileContent, string scenarioDir)
        {
            try
            {
                var scenario = JsonConvert.DeserializeObject<ScenarioConfig>(fileContent);
                scenario.SetLocation(scenarioDir);
                scenario.Map = scenario.Map ?? new MapConfig();
                scenario.Content = scenario.Content ?? new ContentConfig();

                return LoadResult<ScenarioConfig>.Success(scenario);
            }
            catch(Exception e)
            {
                return LoadResult<ScenarioConfig>.Fail(e.Message);
            }
        }
    }
}
