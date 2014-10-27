using LibNoise;
using LibNoise.Filter;
using LibNoise.Modifier;
using LibNoise.Primitive;
using IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using IO.Common;

namespace Engine.Maps
{
    public class WorldMap
    {
        public readonly int Seed;

        private IModule terrainModule;

        internal WorldMap(int seed)
        {
            this.Seed = seed;

            initTerrain();
        }


        internal void GetMap(int x, int y, int w, int h, ref MapTile[,] outMap)
        {
            for(int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    outMap[i, j] = GetTile(x + i, y + j);
        }

        public MapTile GetTile(int x, int y)
        {
            const float scale = 100f;

            var px = (float)(x) / scale;
            var py = (float)(y) / scale;
            var val = ((IModule3D)terrainModule).GetValue(px, py, 0);

            return val < 0.0f ? MapTile.Dirt : MapTile.Grass;
        }


        private void initTerrain()
        {
            //terrain type 1 - flat
            PrimitiveModule flatPrimitive = new SimplexPerlin(this.Seed, NoiseQuality.Standard);

            var flatBase = new Billow()
            {
                Primitive3D = (IModule3D)flatPrimitive,
                Frequency = 1,
            };


            var flatTerrain = new ScaleBias(flatBase, 0.2f, -0.15f);


            //terrain type 2 - mountains
            var mountainTerrain = new RidgedMultiFractal()
            {
                Frequency = flatBase.Frequency / 2,
                Primitive3D = (IModule3D)flatPrimitive,
            };



            //mixer for flat/mountain terrains
            var mountainMixerBase = new Billow()
            {
                Frequency = mountainTerrain.Frequency / 2,
                Primitive3D = (IModule3D)flatPrimitive,
            };
            var mountainMixer = new LibNoise.Modifier.ScaleBias(mountainMixerBase, 1f, -0.95f);

            //mix flat/mountains here
            var mixedTerrain = new Select(mountainMixer, mountainTerrain, flatTerrain, 0, 1, 0.5f);


            //deep water (inverted mountains)
            IModule deepWater = new Invert(mountainTerrain);
            deepWater = new ScaleBias(deepWater)
            {
                Scale = 0.2f,
                Bias = -1
            };

            //water mixer
            IModule waterMixer = new Billow()
            {
                Frequency = 0.1f,
                Lacunarity = 6,
                Primitive3D = (IModule3D)flatPrimitive,
            };
            waterMixer = new ScaleBias(waterMixer, 1, -0.95f);

            //mix water/terrain here
            var finalTerrainA = new Select(waterMixer, mixedTerrain, deepWater, -1, 1, 0.3f);

            //clamp them vals (why?)
            var finalTerrainB = new Clamp(finalTerrainA, -1, 1);

            var finalTerrain = new ScaleBias(finalTerrainB, 0.5f, 0.5f);

            this.terrainModule = mixedTerrain;
        }
    }
}
