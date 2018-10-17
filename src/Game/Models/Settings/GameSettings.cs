using Ix.Math;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Shanism.Client.Game
{
    /// <summary>
    /// Contains settings relating to the current player saved in json format on disk.
    /// Saves keybindings, server list, game and graphics options. 
    /// </summary>
    class GameSettings
    {

        [JsonIgnore]
        public string FileName { get; private set; }

        #region Settings

        public string PlayerName { get; set; } = "Shanist";

        /* Game */
        public bool AlwaysShowHealthBars { get; set; } = true;
        public bool QuickButtonPress { get; set; } = true;
        public bool ExtendCast { get; set; } = true;

        /* Graphics */
        public bool VSync { get; set; } = false;
        public bool FullScreen { get; set; } = false;
        public float RenderScale { get; set; } = 1.0f;
        public float MaxFps { get; set; } = 60;


        public event Action<GameSettings> Saved;


        public Rectangle WindowBounds { get; set; } = new Rectangle(100, 100, 800, 600);

        public KeybindSettings Keybinds { get; set; } = new KeybindSettings(true);

        public ServerSettings Servers { get; set; } = new ServerSettings();

        #endregion


        public GameSettings(string fileName)
        {
            FileName = fileName;

            if(!TryRead(fileName, out var errors))
            {
                ClientLog.Instance.Info($"Unable to read user settings: {errors}. Using default settings instead.");
                Save();
            }
        }

        public void ReloadFromDisk()
        {
            TryRead(FileName, out var _);
        }

        /// <summary>
        /// Resets all keybindings to their default values. 
        /// </summary>
        public void ResetKeybinds()
        {
            Keybinds = new KeybindSettings(true);
            Save();
        }
        

        public bool TryRead(string fileName, out string errors)
        {
            if(!File.Exists(fileName))
            {
                errors = $"File does not exist.";
                return false;
            }

            try
            {
                JsonConvert.PopulateObject(File.ReadAllText(fileName), this);

                errors = null;
                return true;
            }
            catch(Exception e)
            {
                errors = e.Message;
                return false;
            }
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
