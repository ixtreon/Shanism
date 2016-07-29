﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Shanism.Engine.Noise
//{
//    public class PerlinNoise : IModule
//    {
//        int[] p = { 151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148, 247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166, 77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169, 200, 196, 135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42, 223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31, 181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254, 138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180, 151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148, 247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166, 77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169, 200, 196, 135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42, 223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31, 181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254, 138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180 };

//        protected override void generate(int width, int height, byte[,] arr)
//        {
//            for (var i = 0; i < width; i++)
//                for (var j = 0; j < height; j++)
//                {
//                    var X = i % 256;
//                    var Y = j % 256;

//                    var x = (float)i / width;
//                    var y = (float)j / height;

//                    var u = fade(x);
//                    var v = fade(y);

//                }
//        }

//        float fade(float t) { return t * t * t * (t * (t * 6 - 15) + 10); }

//        float noise(float x, float y)
//        {

//            var X = (int)Math.Floor(x) & 255;                  // FIND UNIT CUBE THAT
//            var Y = (int)Math.Floor(y) & 255;                  // CONTAINS POINT.
//            x -= (int)Math.Floor(x);                                     // FIND RELATIVE X,Y,Z
//            y -= (int)Math.Floor(y);                                     // OF POINT IN CUBE.
//            var u = fade(x);                                        // COMPUTE FADE CURVES
//            var v = fade(y);                                        // FOR EACH OF X,Y,Z.

//            var A = p[X] + Y;
//            var B = p[X + 1] + Y;
//            var AA = p[A];
//            var AB = p[A + 1];                                  // HASH COORDINATES OF
//            var BA = p[B];
//            var BB = p[B + 1];                                  // THE 8 CUBE CORNERS,

//            return lerp(
//                    lerp(
//                        lerp(
//                            grad(p[AA], x, y, z),      // AND ADD
//                            grad(p[BA], x - 1, y, z), 
//                            u),                                           // BLENDED
//                        lerp(
//                            grad(p[AB], x, y - 1, z),                              // RESULTS
//                            grad(p[BB], x - 1, y - 1, z), 
//                            u), 
//                        v),                                      // FROM  8
//                    lerp(
//                        lerp(
//                            grad(p[AA + 1], x, y, z - 1),            // CORNERS
//                            grad(p[BA + 1], x - 1, y, z - 1), 
//                            u),                                   // OF CUBE
//                        lerp(
//                            grad(p[AB + 1], x, y - 1, z - 1), 
//                            grad(p[BB + 1], x - 1, y - 1, z - 1), 
//                            u), 
//                        v), 
//                    w);
//        }
//        float grad(int hash, float x, float y, float z)
//        {
//            var h = hash & 15;
//            float u, v;

//            if (h < 8)
//                u = x;
//            else
//                u = y;


//            if (h < 4)
//                v = y;
//            else
//            {
//                if (h == 12 || h == 14)
//                    v = x;
//                else
//                    v = z;
//            }

//            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
//        }

//        static float lerp(float a, float b, float t) => a + t * (b - a);
//    }
//}