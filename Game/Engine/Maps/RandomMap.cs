using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO.Common;
using SharpNoise.Modules;
using System.Diagnostics;
using Engine.Objects.Game;

namespace Engine.Maps
{
    public enum RandomObject
    {
        Tree, Rock
    }

    /// <summary>
    /// Generates infinite procedural terrain and doodads using noise primitives. 
    /// </summary>
    public class RandomMap
    {
        public readonly int Seed;

        private Module terrainModule;
        private Module humidityModule;

        readonly Random Rnd;

        internal RandomMap(int seed)
        {
            this.Seed = seed;
            this.Rnd = new Random(seed);
            initTerrain();
            innitHumidity();
        }


        /// <summary>
        /// Gets the terrain map for the given in-game rectangle and writes it to the second parameter. 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outMap"></param>
        internal void GetMap(Rectangle rect, ref TerrainType[,] outMap)
        {
            foreach (var p in rect.Iterate())
                outMap[p.X - rect.X, p.Y - rect.Y] = GetTile(p);
        }

        /// <summary>
        /// Gets the doodads for the given in-game rectangle. 
        /// NYI
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        internal IEnumerable<Doodad> GenerateNativeDoodads(Rectangle rect)
        {
            foreach(var pt in rect.Iterate())
            {
                if (!GetTile(pt).IsWater())
                {
                    var intPt = pointTransform(pt);
                    var v = humidityModule.GetValue(intPt.X, intPt.Y, 0);

                    var n_trees = ((int)(v * 2)).Clamp(0, 1);
                    for (int i = 0; i < n_trees; i++)
                    {
                        var dx = pt.GetInt(i, 1).GetDouble();
                        var dy = pt.GetInt(i, 2).GetDouble();
                        var loc = pt + new Vector(dx, dy);
                        var tree = new Doodad("tree", loc);
                        yield return tree;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the map tile for the given x/y pair. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public TerrainType GetTile(Point p)
        {
            var internalPt = pointTransform(p);
            
            var tileHeight = terrainModule.GetValue(internalPt.X, internalPt.Y, 0);

            return getMapTile(p, tileHeight);
        }

        private Vector pointTransform(Point p)
        {
            const float scale = 100;
            return (Vector)p / scale;
        }

        struct DoodadSettings
        {
            public Doodad Doodad;
            public double PreferredHumidity;
            public double PreferredTerrain;
            public double HumiditySpread;
            public double TerrainSpread;
        }


        struct TileSettings
        {
            public TerrainType Tile;
            public double Min, Max;

        }

        static readonly TileSettings[] tileSettings = new[] 
        {
            new TileSettings { Tile = TerrainType.DeepWater, Min = -1.0, Max = -0.5},
            new TileSettings { Tile = TerrainType.Water, Min = -0.75, Max = -0.1},
            new TileSettings { Tile = TerrainType.Sand, Min = -0.15, Max = -0},
            new TileSettings{ Tile = TerrainType.Dirt, Min = -0.05, Max = 0.2},
            new TileSettings{ Tile = TerrainType.Grass, Min = 0.10, Max = 0.4},
            new TileSettings{ Tile = TerrainType.Stone, Min = 0.25, Max = 0.75},
            new TileSettings { Tile = TerrainType.Snow, Min = 0.4, Max = 1.0},
        };

        /// <summary>
        /// Gets the tile for the given position
        /// </summary>
        /// <param name="p"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private TerrainType getMapTile(Point p, double height)
        {
            height = height.Clamp(-1, 1);

            var tiles = tileSettings.Where(s => s.Min <= height && height <= s.Max).ToArray();
            Debug.Assert(tiles.Any());

            if (tiles.Length == 1) 
                return tiles[0].Tile;
            if(tiles.Length == 2)
            {
                var heightChance = (double)(height - tiles[1].Min) / (tiles[0].Max - tiles[1].Min);

                var roll = Hash.GetDouble(p.X, p.Y);

                roll = 0.5;

                if (heightChance - roll > 0)
                    return tiles[1].Tile;
                else
                    return tiles[0].Tile;
            }
            
            throw new Exception();
        }


        private void innitHumidity()
        {
            const double zone_freq = 0.05;
            //get terrain

            //get distance from equator (huge perlin)
            var distFromEquator = new Perlin()
            {
                Seed = Seed + 100,
                Frequency = zone_freq,

            };

            humidityModule = new ScaleBias()
            {
                Source0 = new Perlin()
                {
                    Seed = Seed + 101,
                    Frequency = 1,
                },
                Scale = 0.3,
                Bias = 0.5,
            };
            var zz = getMinMax(humidityModule, 100000);
        }

        private void initTerrain()
        {
            const float flat_freq = 1f;

            var flatTerrain =  new ScaleBias()
            {
                //flat terrain is ~(0; 0.45)
                Source0 = new Turbulence
                {
                    Source0 = new Billow()  
                    {
                        //billow ranges -1.42 to 2.65
                        Seed = Seed,
                        Frequency = flat_freq,
                    },
                    Frequency = flat_freq,
                    Power = 0.3,
                },
                Bias = 0.17,
                Scale = 0.1,
            };
            //var zuza = getMinMax(flatTerrain, 100000);
            var mountainTerrain = new ScaleBias()
            {
                // 0.3 : 1.00
                Source0 = new Turbulence()
                {
                    //????
                    Source0 = new RidgedMulti()
                    {
                        // -1 : 1.42?
                        Frequency = flat_freq / 2,
                        Seed = Seed + 1,
                    },
                    Frequency = flat_freq / 2,
                    Power = 0.85,
                },
                Bias = 0.6,
                Scale = 0.29,
            };
            var zuzz = getMinMax(mountainTerrain, 100000);
            var allTheGround = new Blend()
            {
                //flat terrain
                Source0 = flatTerrain,
                //mountain terrain
                Source1 = mountainTerrain,
                Control = new ScaleBias()
                {
                    Source0 = new Perlin()
                    {
                        Frequency = flat_freq / 20,
                        Persistence = 0.35,
                        Seed = Seed + 2,
                    },
                    Scale = 1,
                }
            };
            var seas = new ScaleBias()
            {
                //-1.0 : -0.2
                Source0 = flatTerrain,  //0 : 0.45
                //Source0 = new Invert()
                //{
                //    //is 0 : 0.45
                //    Source0 = flatTerrain,
                //},
                Scale = 1.78,
                Bias = -1,
            };
            var zzz = getMinMax(flatTerrain);
            var zz = getMinMax(seas);

            var seaGround = new Blend()
            {
                //flat terrain
                Source0 = allTheGround,
                //mountain terrain
                Source1 = seas,
                Control = new ScaleBias
                {
                    Source0 = new Perlin()
                    {
                        Frequency = flat_freq / 32,
                        Seed = Seed + 3,
                        Lacunarity = 1.05,
                        Persistence = 0.2,
                        OctaveCount = 3,
                    },
                    Scale = 1,
                    Bias = -0.15,
                },
                //Control = new ScaleBias()
                //{
                //    Source0 = new Perlin()
                //    {
                //        Frequency = flat_freq / 8,
                //        Persistence = 0.35,
                //        Seed = Seed + 3,
                //    },
                //    Scale = 1,
                //}
            };

            new Displace()
            {
                Source0 = mountainTerrain,
                ZDisplace = new Perlin(),
            };
            terrainModule = seaGround;
        }

        string getMinMax(Module kur, int samples = 1000)
        {
            const double range = 1E6;

            double min = double.MaxValue, max = double.MinValue;
            for (int i = 0; i < samples; i++)
            {
                var x = Rnd.NextDouble() * 2 * range - range;
                var y = Rnd.NextDouble() * 2 * range - range;
                var z = Rnd.NextDouble() * 2 * range - range;
                var val = kur.GetValue(x, y, z);
                if (val < min)
                    min = val;
                if (val > max)
                    max = val;
            }
            return min + " : " + max;
        }
    }
}
