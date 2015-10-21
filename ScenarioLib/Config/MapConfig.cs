using IO.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScenarioLib
{
    /// <summary>
    /// Contains the configuration for the map layer. 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class MapConfig
    {
        /// <summary>
        /// Gets or sets whether the map is infinite. 
        /// </summary>
        [JsonProperty]
        public bool Infinite { get; set; }


        /// <summary>
        /// Gets or sets the width of the map, if it is finite. 
        /// </summary>
        public int Width { get { return Map.GetLength(0); } }

        /// <summary>
        /// Gets or sets the height of the map, if it is finite. 
        /// </summary>
        public int Height { get { return Map.GetLength(1); } }

        [JsonProperty]
        public TerrainType[,] Map { get; set; }

        public Point Size {  get { return new Point(Width, Height); } }


        public MapConfig()
        {
            Infinite = false;

            Map = new TerrainType[64, 64];
        }

        public void ResizeMap(int w, int h)
        {
            var newMap = new TerrainType[w, h];

            if (Map != null)
            {
                var mw = Math.Min(Width, w);
                var mh = Math.Min(Height, h);
                foreach (var x in Enumerable.Range(0, mw))
                    foreach (var y in Enumerable.Range(0, mh))
                        newMap[x, y] = Map[x, y];
            }

            Map = newMap;
        }
    }
}
