using Client.Settingsz;
using IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    static class ShanoSettings
    {
        /// <summary>
        /// The file where settings are saved. 
        /// </summary>
        static string SettingsFile = "config.json";

        public static ApplicationSettings Current { get; private set; }

        static ShanoSettings()
        {
            Reload();
            Current.Keybinds[Input.GameAction.ToggleMainMenu] = Microsoft.Xna.Framework.Input.Keys.Escape;
        }

        public static void Reload()
        {
            var success = false;
            try
            {
                var fileData = File.ReadAllText(SettingsFile);
                Current = JsonConvert.DeserializeObject<ApplicationSettings>(fileData);
                success = (Current != null);
            }
            catch { success = false; }


            if (!success)
            {
                Console.WriteLine("Unable to load proper settings data from the '{0}' file. ".F(SettingsFile));
                Current = new ApplicationSettings();
                Save();
            }
        }

        public static void Save()
        {
            try
            {
                var datas = JsonConvert.SerializeObject(Current);
                File.WriteAllText(SettingsFile, datas);
            }
            catch(Exception e)
            {
                Console.WriteLine("Unable to write settings data to the '{0}' file: {1}".F(SettingsFile, e.Message));
            }
        }
    }
}
