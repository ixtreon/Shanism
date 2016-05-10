using Shanism.Common.Game;
using Shanism.Common.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bitmap = System.Drawing.Bitmap;
using Shanism.Common;

namespace Shanism.Editor.ViewModels
{
    public class TextureViewModel
    {
        /// <summary>
        /// Gets whether this texture is included in the scenario. 
        /// </summary>
        [ReadOnly(true)]
        public bool Included { get; set; }

        /// <summary>
        /// Gets the full path to the texture. 
        /// </summary>
        public string FullPath => System.IO.Path.Combine(ContentDirectory, Path);


        public string ContentDirectory { get; }


        /// <summary>
        /// Gets the relative path to the texture. 
        /// </summary>
        public string Path => Data.Name;


        [Browsable(false)]
        public Bitmap Image { get; }

        /// <summary>
        /// The definition of the texture. 
        /// </summary>
        [Browsable(false)]
        public TextureDef Data { get; }


        //Used by the UI
        public int LogicalWidth
        {
            get { return Data.Splits.X; }
            set
            {
                if (value >= 0 && value < Size.X)
                    Data.Splits = new Point(value, Data.Splits.Y);
            }
        }

        //Used by the UI
        public int LogicalHeight
        {
            get { return Data.Splits.Y; }
            set
            {
                if (value >= 0 && value < Size.Y)
                    Data.Splits = new Point(Data.Splits.X, value);
            }
        }

        /// <summary>
        /// Gets the size of a single cell in this texture. 
        /// </summary>
        [Browsable(false)]
        public Point CellSize => Image.Size.ToPoint() / Data.Splits;


        public Point Size => new Point(Image.Width, Image.Height);


        public TextureViewModel(string contentDir, Bitmap bmp, TextureDef data)
        {
            if (bmp == null) throw new ArgumentNullException(nameof(bmp));
            if (data == null) throw new ArgumentNullException(nameof(data));

            ContentDirectory = contentDir;
            Data = data;
            Image = bmp;
        }

        public override string ToString() => Path;
    }

}
