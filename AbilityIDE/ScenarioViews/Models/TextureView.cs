using IO.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilityIDE.ScenarioViews
{
    public class TextureView
    {
        [ReadOnly(true)]
        public string Path { get; set; }

        [Browsable(false)]
        public Bitmap Image { get; set; }

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
                        Data = oldData ?? new TextureDef(FullPath);
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
    }

}
