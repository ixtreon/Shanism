using IO.Common;
using ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Graphics = System.Drawing.Graphics;
using Bitmap = System.Drawing.Bitmap;
using Color = System.Drawing.Color;
using SolidBrush = System.Drawing.SolidBrush;

namespace ShanoEditor.Views.Maps.Layers
{
    class TerrainLayer : MapLayer
    {
        static readonly Dictionary<TerrainType, Color> terrainColors = new Dictionary<TerrainType, Color>
        {
            { TerrainType.DeepWater, Color.DarkBlue },
            { TerrainType.Dirt, Color.SaddleBrown },
            { TerrainType.Grass, Color.LightGreen },
            { TerrainType.None, Color.Black },
            { TerrainType.Sand, Color.Yellow },
            { TerrainType.Snow, Color.White },
            { TerrainType.Stone, Color.Gray },
            { TerrainType.Water, Color.Blue },
        };

        Bitmap MapTerrain;
        public WorldView View { get; private set; }

        public TerrainLayer(WorldView view)
        {
            View = view;
        }

        public void Draw(Graphics g)
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            var destPos = -View.LowLeftPoint * View.UnitSize;
            var destSz = (Vector)View.MapSize * View.UnitSize;
            destPos += (View.UnitSize / 2); //fix rendering?

            g.DrawImage(MapTerrain, new RectangleF(destPos, destSz).ToNetRectangle());
        }

        /// <summary>
        /// Rebuilds the terrain bitmap out of the given map. 
        /// </summary>
        public void Load(MapConfig map)
        {
            MapTerrain = new Bitmap(map.Width, map.Height);
            using (var g = Graphics.FromImage(MapTerrain))
            {
                foreach (var ix in Enumerable.Range(0, map.Width))
                    foreach (var iy in Enumerable.Range(0, map.Height))
                    {
                        var tType = map.Terrain[ix, iy];
                        var tColor = terrainColors[tType];

                        MapTerrain.SetPixel(ix, iy, tColor);
                    }
            }
        }

        public void SetPixels(TerrainType ty, Point pos, Point sz)
        {
            var tColor = terrainColors[ty];
            var dest = new Rectangle(pos, sz).ToNetRectangle();

            using (var g = Graphics.FromImage(MapTerrain))
            using (var br = new SolidBrush(tColor))
                g.FillRectangle(br, dest);
        }
    }
}
