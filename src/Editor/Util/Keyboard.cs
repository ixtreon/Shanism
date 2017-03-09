using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shanism.Editor
{
    class Keyboard
    {

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetKeyboardState(byte[] lpKeyState);


        static bool[] keyStates;

        public static void Update()
        {
            byte[] keys = new byte[256];

            //Get pressed keys
            if (!GetKeyboardState(keys))
                throw new Exception("GetKeyboardState failed!");

            keyStates = keys
                .Select(key => (key & 0x80) != 0)
                .ToArray();

        }

        public static bool GetKeyState(Keys k)
        {
            return keyStates[(int)k];
        }
    }
}
