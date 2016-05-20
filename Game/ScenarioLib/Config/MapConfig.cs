﻿using Shanism.Common.Game;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;

namespace Shanism.ScenarioLib
{
    /// <summary>
    /// Contains the configuration for the map layer. 
    /// Specifies whether the map is procedural or fixed
    /// and if the latter, also its size and terrain. 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class MapConfig
    {
        /// <summary>
        /// Gets or sets whether the map is infinite. 
        /// </summary>
        [JsonProperty]
        public bool IsInfinite { get; set; }

        /// <summary>
        /// Gets or sets the terrain data for a finite map. 
        /// </summary>
        [JsonProperty]
        public TerrainType[,] Terrain { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="ObjectConstructor"/>s that are created. 
        /// </summary>
        [JsonProperty]
        public List<ObjectConstructor> Objects { get; } = new List<ObjectConstructor>();


        /// <summary>
        /// Gets or sets the width of the map, if it is finite. 
        /// </summary>
        public int Width => Terrain.GetLength(0);

        /// <summary>
        /// Gets or sets the height of the map, if it is finite. 
        /// </summary>
        public int Height => Terrain.GetLength(1);

        /// <summary>
        /// Gets the size of the map. 
        /// </summary>
        public Point Size => new Point(Width, Height);


        public MapConfig()
        {
            IsInfinite = false;
            Terrain = new TerrainType[64, 64];
        }

        /// <summary>
        /// Resizes the map to the given size, trying to carry over
        /// as much terrain data as possible. 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void ResizeMap(int w, int h)
        {
            var newMap = new TerrainType[w, h];

            if (Terrain != null)
            {
                //copy old map over
                var mw = Math.Min(Width, w);
                var mh = Math.Min(Height, h);
                foreach (var x in Enumerable.Range(0, mw))
                    foreach (var y in Enumerable.Range(0, mh))
                        newMap[x, y] = Terrain[x, y];
            }

            Terrain = newMap;
        }
    }
}