using ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Graphics = System.Drawing.Graphics;

namespace ShanoEditor.Views.Maps.Layers
{
    interface MapLayer
    {

        WorldView View { get; }

        void Draw(Graphics g);

        void Load(MapConfig map);

    }
}
