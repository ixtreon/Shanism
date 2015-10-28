using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilityIDE.ScenarioViews.Models
{
    class AnimationView
    {
        public int FirstCellX { get; set; }
        public int FirstCellY { get; set; }

        public int LengthX { get; set; } = 1;
        public int LengthY { get; set; } = 1;

        public int Frames {  get { return LengthX * LengthY; } }
    }
}
