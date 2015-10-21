using IO.Content;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScenarioLib
{
    public class ModelManager
    {
        public const string ModelsFileName = "models.json";
        
        public HashSet<ModelDef> Models { get; private set; }

        public string FilePath { get; private set; }

        private ModelManager() { }


        public static ModelManager FromScenarioDir(string scenarioPath)
        {
            var fullPath = Path.GetFullPath(Path.Combine(scenarioPath, ModelsFileName));
            if (!File.Exists(fullPath))
                return null;

            var datas = File.ReadAllText(fullPath);
            var modelDefs = JsonConvert.DeserializeObject<HashSet<ModelDef>>(datas);

            return new ModelManager
            {
                Models = modelDefs,
                FilePath = fullPath,
            };
        }


        public void Save()
        {
            var datas = JsonConvert.SerializeObject(Models);
            File.WriteAllText(FilePath, datas);
        }

    }
}
