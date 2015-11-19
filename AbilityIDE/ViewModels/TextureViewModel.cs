using IO.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanoEditor.ViewModels
{
    public class TextureViewModel
    {
        [ReadOnly(true)]
        public string Path { get; set; }

        [Browsable(false)]
        public Bitmap Image { get; set; }

        /// <summary>
        /// The definition of the texture. 
        /// Can be null if texture is not selected in the UI. 
        /// </summary>
        [Browsable(false)]
        public TextureDef Data { get; set; }

        [Browsable(false)]
        public string FullPath { get; set; }

        public int? ImageWidth { get { return Image?.Width ?? null; } }

        public int? ImageHeight { get { return Image?.Width ?? 0; } }


        TextureDef oldData;

        [ReadOnly(true)]
        public bool Included
        {
            get { return Data != null; }
            set
            {
                if (value != Included)
                {
                    if (value)
                        Data = oldData ?? new TextureDef(Path);
                    else
                    {
                        oldData = Data;
                        Data = null;
                    }
                }
            }
        }


        public int? LogicalWidth
        {
            get { return Data?.Splits.X; }
            set
            {
                if (Data != null && value.HasValue
                    && value.Value >= 0 && value.Value < ImageWidth)
                    Data.Splits = new IO.Common.Point(value.Value, Data.Splits.Y);
            }
        }

        public int? LogicalHeight
        {
            get { return Data?.Splits.Y; }
            set
            {
                if (Data != null && value.HasValue
                    && value.Value >= 0 && value.Value < ImageHeight)
                    Data.Splits = new IO.Common.Point(Data.Splits.X, value.Value);
            }
        }

        public override string ToString()
        {
            return Path;
        }
    }

}
