using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Settingsz
{
    class ApplicationSettings
    {

        public bool AlwaysShowHealthBars { get; set; } = true;

        public bool QuickButtonPress { get; set; } = true;


        public KeybindSettings Keybinds { get; private set; } = new KeybindSettings(true);
    }
}
