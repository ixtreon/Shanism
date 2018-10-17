using Ix.Math;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Shanism.Client.Systems
{
    struct TextureMaker
    {
        const int BytesPerPixel = 4;
        static byte[] White { get; } = { 255, 255, 255, 255 };

        public Point Size { get; }

        byte[] buffer;

        public TextureMaker(int width, int height)
        {
            Size = new Point(width, height);
            buffer = new byte[width * height * BytesPerPixel];
        }


        public void WriteTo(Texture2D texture)
        {
            if (texture.GetSize() != Size)
                throw new ArgumentException($"Can't assign image of size {Size} to texture of size {texture.GetSize()}.");

            texture.SetData(buffer);
        }

        public void HLine(int x, int y, int len)
        {
            var id = (y * Size.X + x) * BytesPerPixel;
            buffer.Copy(White, id, len);
        }

        public void VLine(int x, int y, int len)
        {
            for (int i = 0; i < len; i++)
            {
                var id = ((y + i) * Size.X + x) * BytesPerPixel;
                Array.Copy(White, 0, buffer, id, BytesPerPixel);
            }
        }
    }

    static class Ext
    { 
        /// <summary>
        /// Copies the <paramref name="src"/> array exactly <paramref name="nTimes"/>
        /// into the given array, starting at an offset of <paramref name="destIndex"/>.
        /// </summary>
        public static void Copy(this Array dest, Array src, int destIndex, int nTimes)
        {
            if (nTimes == 0)
                return;

            var lengthToWrite = src.Length * nTimes;
            if (destIndex < 0 || destIndex + lengthToWrite > dest.Length)
                throw new IndexOutOfRangeException(nameof(destIndex));

            // copy first element
            Array.Copy(src, 0, dest, destIndex, src.Length);

            // copy twice as much elements as before, as long as we can
            var timesWritten = 1;
            var elemsWritten = src.Length;
            var timesLeft = nTimes - 1;
            while (timesLeft >= timesWritten)
            {
                Array.Copy(dest, destIndex, dest, destIndex + elemsWritten, elemsWritten);

                timesLeft -= timesWritten;
                timesWritten *= 2;
                elemsWritten *= 2;
            }

            // copy the remaining elements, too
            var elemsLeft = timesLeft * src.Length;
            Array.Copy(dest, destIndex, dest, destIndex + elemsWritten, elemsLeft);
        }
    }
}
