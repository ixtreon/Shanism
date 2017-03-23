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
    /// Contains settings relating to the current player saved in json format on disk.
    /// Saves keybindings, server list, game and graphics options. 
    /// </summary>
    class Settings
    {

        #region Static Members

        /// <summary>
        /// The file where settings are saved. 
        /// </summary>
        const string DefaultSettingsFile = "config.json";

        public static Settings Current { get; private set; }


        public static event Action<Settings> Saved;

        static Settings()
        {
            Reload();
        }

        public static void Reload()
        {
            Current = Open(DefaultSettingsFile);
        }

        /// <summary>
        /// Reloads the settings file from disk. Useful when discarding unwanted changes.
        /// </summary>
        public static Settings Open(string fileName)
        {
            var settings = tryRead(fileName);
            if(settings == null)
            {
                Console.WriteLine($"Can't find user settings file. Loading default settings instead.");
                settings = new Settings(fileName);
                settings.Save();
            }

            //why put it there if it can't be changed?
            settings.Keybinds[Input.ClientAction.ToggleMenus] = Microsoft.Xna.Framework.Input.Keys.Escape;

            return settings;
        }

        static Settings tryRead(string fileName)
        {
            try
            {
                var fileData = File.ReadAllText(fileName);

                var settings = JsonConvert.DeserializeObject<Settings>(fileData);
                settings.FileName = fileName;

                return settings;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        [JsonIgnore]
        public string FileName { get; private set; }

        public bool PlayerName { get; set; }

        /* Game */
        public bool AlwaysShowHealthBars { get; set; } = true;
        public bool QuickButtonPress { get; set; } = true;
        public bool ExtendCast { get; set; } = true;

        /* Graphics */
        public bool VSync { get; set; } = false;
        public bool FullScreen { get; set; } = false;
        public float RenderScale { get; set; } = 1.0f;


        public KeybindSettings Keybinds { get; set; } = new KeybindSettings(true);

        public ServerSettings Servers { get; set; } = new ServerSettings();

        Settings() { }

        Settings(string fileName)
        {
            FileName = fileName;
        }

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
                File.WriteAllText(FileName, datas);
                Saved?.Invoke(this);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to write settings data to the '{FileName}' file: {e.Message}");
            }
        }
    }
}
