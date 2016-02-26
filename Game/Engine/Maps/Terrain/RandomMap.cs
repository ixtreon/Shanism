using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO.Common;
using SharpNoise.Modules;
using System.Diagnostics;
using Engine.Objects.Entities;
using Engine.Objects;

namespace Engine.Maps
{
    public enum RandomObject
    {
        Tree, Rock
    }

    /// <summary>
    /// Generates infinite procedural terrain and doodads using noise primitives. 
    /// </summary>
    public class RandomMap : ITerrainMap
    {
        struct TerrainData
        {
            public TerrainType Tile;
            public double Min, Max;
        }

        //struct RandomObjectData
        //{
        //    public Doodad Doodad;
        //    public double PreferredHumidity;
        //    public double PreferredTerrain;
        //    public double HumidityTolerance;
        //    public double TerrainTolerance;
        //}

        static readonly TerrainData[] tileSettings =
        {
            new TerrainData { Tile = TerrainType.DeepWater, Min = -1.0, Max = -0.5},
            new TerrainData { Tile = TerrainType.Water, Min = -0.75, Max = -0.1},
            new TerrainData { Tile = TerrainType.Sand, Min = -0.15, Max = -0},
            new TerrainData { Tile = TerrainType.Dirt, Min = -0.05, Max = 0.2},
            new TerrainData { Tile = TerrainType.Grass, Min = 0.10, Max = 0.4},
            new TerrainData { Tile = TerrainType.Stone, Min = 0.25, Max = 0.75},
            new TerrainData { Tile = TerrainType.Snow, Min = 0.4, Max = 1.0},
        };



        Module terrainModule;
        Module humidityModule;

        public readonly int Seed;

        public Rectangle Bounds { get; }
            = new Rectangle(-int.MaxValue / 2, -int.MaxValue / 2, int.MaxValue, int.MaxValue);

        internal RandomMap(int seed)
        {
            this.Seed = seed;
            initTerrain();
            innitHumidity();
        }

        /// <summary>
        /// Gets the terrain map for the given in-game rectangle and writes it to the second parameter. 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="outMap"></param>
        public void GetMap(Rectangle rect, ref TerrainType[] outMap)
        {
            foreach (var p in rect.Iterate())
                outMap[(p.X - rect.X) + IO.Constants.Terrain.ChunkSize * (p.Y - rect.Y)] = GetTerrainAt(p);
        }

        /// <summary>
        /// Gets the doodads for the given in-game rectangle. 
        /// NYI
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public IEnumerable<Entity> GetNativeEntities(Rectangle rect)
        {
            foreach (var pt in rect.Iterate())
            {
                if (!GetTerrainAt(pt).IsWater())
                {
                    var intPt = pointTransform(pt);
                    var v = humidityModule.GetValue(intPt.X, intPt.Y, 0);

                    var n_trees = ((int)(v * 2)).Clamp(0, 1);
                    for (int i = 0; i < n_trees; i++)
                    {
                        var dx = Hash.GetDouble(pt.X, pt.Y, 1);
                        var dy = Hash.GetDouble(pt.X, pt.Y, 2);
                        var loc = pt + new Vector(dx, dy);

                        yield return new Doodad { ModelName = "tree", Position = loc };
                    }
                }
            }
        }

        /// <summary>
        /// Gets the map tile at the given point. 
        /// </summary>
        public TerrainType GetTerrain(Vector loc)
        {
            return GetTerrainAt(loc.Floor());
        }

        /// <summary>
        /// Gets the map tile at the given point. 
        /// </summary>
        public TerrainType GetTerrainAt(Point p)
        {
            var internalPt = pointTransform(p);

            var tileHeight = terrainModule.GetValue(internalPt.X, internalPt.Y, 0);

            return getMapTile(p, tileHeight);
        }

        Vector pointTransform(Point p)
        {
            const float scale = 100;
            return (Vector)p / scale;
        }





        /// <summary>
        /// Gets the tile for the given position
        /// </summary>
        /// <param name="p"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        TerrainType getMapTile(Point p, double height)
        {
            height = height.Clamp(-1, 1);

            var tiles = tileSettings.Where(s => s.Min <= height && height <= s.Max).ToArray();
            Debug.Assert(tiles.Any());

            if (tiles.Length == 1)
                return tiles[0].Tile;
            if (tiles.Length == 2)
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


        void innitHumidity()
        {
            const double zone_freq = 0.05;
            //get terrain

            //get distance from equator (huge perlin)
            var distFromEquator = new Perlin
            {
                Seed = Seed + 100,
                Frequency = zone_freq,

            };

            humidityModule = new ScaleBias
            {
                Source0 = new Perlin
                {
                    Seed = Seed + 101,
                    Frequency = 1,
                },
                Scale = 0.3,
                Bias = 0.5,
            };
            var zz = getMinMax(humidityModule, 100000);
        }

        void initTerrain()
        {
            const float flat_freq = 1f;

            var flatTerrain = new ScaleBias
            {
                //flat terrain is ~(0; 0.45)
                Source0 = new Turbulence
                {
                    Source0 = new Billow
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
            var mountainTerrain = new ScaleBias
            {
                // 0.3 : 1.00
                Source0 = new Turbulence
                {
                    //????
                    Source0 = new RidgedMulti
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
            var allTheGround = new Blend
            {
                //flat terrain
                Source0 = flatTerrain,
                //mountain terrain
                Source1 = mountainTerrain,
                Control = new ScaleBias
                {
                    Source0 = new Perlin
                    {
                        Frequency = flat_freq / 20,
                        Persistence = 0.35,
                        Seed = Seed + 2,
                    },
                    Scale = 1,
                }
            };
            var seas = new ScaleBias
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

            var seaGround = new Blend
            {
                //flat terrain
                Source0 = allTheGround,
                //mountain terrain
                Source1 = seas,
                Control = new ScaleBias
                {
                    Source0 = new Perlin
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

            //new Displace()
            //{
            //    Source0 = mountainTerrain,
            //    ZDisplace = new Perlin(),
            //};
            terrainModule = seaGround;
        }

        string getMinMax(Module m, int samples = 1000)
        {
            const double range = 1E6;
            var rnd = new Random();

            double min = double.MaxValue, max = double.MinValue;
            for (int i = 0; i < samples; i++)
            {
                var x = rnd.NextDouble() * 2 * range - range;
                var y = rnd.NextDouble() * 2 * range - range;
                var z = rnd.NextDouble() * 2 * range - range;
                var val = m.GetValue(x, y, z);
                if (val < min)
                    min = val;
                if (val > max)
                    max = val;
            }
            return min + " : " + max;
        }

    }
}
