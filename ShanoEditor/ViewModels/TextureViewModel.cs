using IO.Common;
using IO.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bitmap = System.Drawing.Bitmap;

namespace ShanoEditor.ViewModels
{
    public class TextureViewModel
    {
        [ReadOnly(true)]
        public bool Included { get; set; }


        public string FullPath { get; }

        public string Path { get; }


        [Browsable(false)]
        public Bitmap Image { get; }

        /// <summary>
        /// The definition of the texture. 
        /// </summary>
        [Browsable(false)]
        public TextureDef Data { get; }

        public Point FullSize { get { return new Point(Image.Width, Image.Height); } }



        public TextureViewModel(string fullPath, string relPath, Bitmap bmp, TextureDef data)
        {
            if (bmp == null) throw new ArgumentNullException(nameof(bmp));
            if (data == null) throw new ArgumentNullException(nameof(data));

            FullPath = fullPath;
            Path = relPath;
            Image = bmp;
            Data = data;
        }
        
        /// <summary>
        /// Gets the size of a single cell in this texture. 
        /// </summary>
        [Browsable(false)]
        public Point CellSize
        {
            get { return Image.Size.ToPoint() / Data.Splits; }
        }

        //Used by the UI
        public int LogicalWidth
        {
            get { return Data.Splits.X; }
            set
            {
                if (value >= 0 && value < FullSize.X)
                    Data.Splits = new Point(value, Data.Splits.Y);
            }
        }

        //Used by the UI
        public int LogicalHeight
        {
            get { return Data.Splits.Y; }
            set
            {
                if (value >= 0 && value < FullSize.Y)
                    Data.Splits = new Point(Data.Splits.X, value);
            }
        }

        public override string ToString()
        {
            return Path;
        }
    }

}
