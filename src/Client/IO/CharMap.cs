using Microsoft.Xna.Framework.Input;
using System;

namespace Shanism.Client.IO
{
    /// <summary>
    /// Provides mapping from <see cref="Keys"/> objects to the characters that show on the keyboard. 
    /// </summary>
    class CharMap
    {
        public static CharMap Default { get; } = new CharMap();


        const int KeyCount = 256;

        readonly char?[] lowerCharMap = new char?[KeyCount];
        readonly char?[] upperCharMap = new char?[KeyCount];


        /// <summary>
        /// Gets the character that is written when the given key is held down. 
        /// Returns null in case there is no recognized character for this <see cref="Keys"/> instance. 
        /// </summary>
        /// <param name="k">The key whose character is needed. </param>
        /// <param name="isCaps">Whether to use the normal or HIGH caps character. </param>
        public char? GetChar(Keys k, bool isCaps)
            => isCaps ? upperCharMap[(int)k] : lowerCharMap[(int)k];

        CharMap()
        {
            addSimpleMap(Keys.Space, ' ');
            //addSimpleMap(Keys.Back, '\b');

            addSequentialMap(Keys.A, 'a', 'A', 26);
            addSequentialMap(Keys.NumPad0, '0', '0', 10);

            addCustomMap(Keys.D0, "0123456789", ")!@#$%^&*(");
            addCustomMap(Keys.OemSemicolon, ";=,-./`", ":+<_>?~");
            addCustomMap(Keys.OemOpenBrackets, "[\\]'", "{|}\"");
        }

        void addSimpleMap(Keys k, char c)
        {
            lowerCharMap[(int)k] = c;
            upperCharMap[(int)k] = c;
        }

        void addSequentialMap(Keys from, char cFrom, char cFromCaps, int count)
        {
            for (var j = 0; j < count; j++)
            {
                lowerCharMap[(int)from + j] = (char)(cFrom + j);
                upperCharMap[(int)from + j] = (char)(cFromCaps + j);
            }
        }

        void addCustomMap(Keys from, string sNormal, string sCaps)
        {
            if (sNormal.Length != sCaps.Length)
                throw new ArgumentException($"Input strings must be of same size: '{sNormal}' and '{sCaps}'.");

            var count = sCaps.Length;
            for (var i = 0; i < count; i++)
            {
                lowerCharMap[(int)from + i] = sNormal[i];
                upperCharMap[(int)from + i] = sCaps[i];
            }
        }
    }
}
