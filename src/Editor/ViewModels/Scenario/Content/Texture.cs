using Shanism.Common.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bitmap = System.Drawing.Bitmap;
using Shanism.Common;
using Shanism.Editor.Util;
using System.Drawing.Design;
using Shanism.Common.Util;

namespace Shanism.Editor.ViewModels
{
    public class TextureViewModel
    {
        [Browsable(false)]
        public string ContentDirectory { get; }

        [Browsable(false)]
        public Bitmap Bitmap { get; }

        /// <summary>
        /// The definition of the texture. 
        /// </summary>
        [Browsable(false)]
        public TextureDef Data { get; }

        /// <summary>
        /// Gets the full path to the texture. 
        /// </summary>
        //[Browsable(false)]
        public string FullPath { get; private set; }


        /// <summary>
        /// Gets whether this texture is included in the scenario. 
        /// </summary>
        //[ReadOnly(true)]
        [Description("Whether to include this texture with the scenario."
            + "\n" + "Use the checkboxes to the right to change this property.")]
        public bool Included { get; set; }

        /// <summary>
        /// Gets or sets the relative path to the texture. 
        /// </summary>
        [Browsable(true)]
        public string Name
        {
            get { return Data.Name; }
        }


        [Category("Cells")]
        [DisplayName("Horizontal")]
        [Description("The number of logical divisions (cells) along this texture's X coordinate.")]
        [Range(1, 100)]
        public int LogicalWidth
        {
            get { return Data.Cells.X; }
            set
            {
                if (value >= 0 && value < ImageSize.X)
                    Data.Cells = new Point(value, Data.Cells.Y);
            }
        }

        [Category("Cells")]
        [DisplayName("Vertical")]
        [Description("The number of logical divisions (cells) along this texture's Y coordinate.")]
        [Range(1, 100)]
        public int LogicalHeight
        {
            get { return Data.Cells.Y; }
            set
            {
                if (value >= 0 && value < ImageSize.Y)
                    Data.Cells = new Point(Data.Cells.X, value);
            }
        }

        /// <summary>
        /// Gets the size of a single cell in this texture. 
        /// </summary>
        [Browsable(false)]
        public Point CellSize => Bitmap.Size.ToPoint() / Data.Cells;

        [Description("The full size of the texture, in pixels.")]
        [DisplayName("Dimensions")]
        public Point ImageSize => new Point(Bitmap.Width, Bitmap.Height);


        public TextureViewModel(string contentDir, string fullPath, Bitmap bmp, TextureDef data)
        {
            if (bmp == null) throw new ArgumentNullException(nameof(bmp));
            if (data == null) throw new ArgumentNullException(nameof(data));

            ContentDirectory = contentDir;
            FullPath = fullPath;
            Data = data;
            Bitmap = bmp;
        }

        public void SetNameAndPath(string fullPath)
        {
            var name = ShanoPath.GetRelativePath(fullPath, ContentDirectory);

            Data.Name = name;
            FullPath = fullPath;
        }

        public override string ToString() => Name;
    }

}
