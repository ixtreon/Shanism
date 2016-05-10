using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client
{
    /// <summary>
    /// Contains settings relating to the current player. 
    /// This includes random stuff like button press, health bars n stuff. 
    /// Also keybinds. 
    /// </summary>
    class Settings
    {
        /// <summary>
        /// The file where settings are saved. 
        /// </summary>
        static readonly string DefaultSettingsFile = "config.json";

        public static Settings Current { get; private set; }



        static Settings()
        {
            Reload();
        }

        /// <summary>
        /// Reloads the settings file from disk. Useful when discarding unwanted changes. 
        /// </summary>
        public static void Reload()
        {
            var success = false;
            try
            {
                var fileData = File.ReadAllText(DefaultSettingsFile);
                Current = JsonConvert.DeserializeObject<Settings>(fileData);
                success = (Current != null);
            }
            catch { success = false; }


            if (!success)
            {
                Console.WriteLine($"Unable to load proper settings data from the '{DefaultSettingsFile}' file. ");
                Current = new Settings();
                Current.Save();
            }


            Current.Keybinds[Input.GameAction.ToggleMenus] = Microsoft.Xna.Framework.Input.Keys.Escape;
        }





        public bool AlwaysShowHealthBars { get; set; } = true;

        public bool QuickButtonPress { get; set; } = true;


        public KeybindSettings Keybinds { get; set; } = new KeybindSettings(true);

        private Settings() { }

        /// <summary>
        /// Resets all keybindings to their default values and then saves the configuration file. 
        /// </summary>
        public void ResetKeybinds()
        {
            Keybinds = new KeybindSettings(true);
            Save();
        }

        public void Save()
        {
            try
            {
                var datas = JsonConvert.SerializeObject(this);
                File.WriteAllText(DefaultSettingsFile, datas);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to write settings data to the '{DefaultSettingsFile}' file: {e.Message}");
            }
        }
    }
}
