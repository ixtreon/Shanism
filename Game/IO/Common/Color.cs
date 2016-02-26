using IxSerializer.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IO.Common
{
    /// <summary>
    /// Represents a standard ARGB color value. 
    /// </summary>
    /// <seealso cref="IxSerializer.Modules.IxSerializable" />
    public struct ShanoColor : IxSerializable
    {
        #region Static Members
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {240, 248, 255}. 
        /// </summary>
        public static readonly ShanoColor AliceBlue = new ShanoColor(240, 248, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {250, 235, 215}. 
        /// </summary>
        public static readonly ShanoColor AntiqueWhite = new ShanoColor(250, 235, 215);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 255, 255}. 
        /// </summary>
        public static readonly ShanoColor Aqua = new ShanoColor(0, 255, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {127, 255, 212}. 
        /// </summary>
        public static readonly ShanoColor Aquamarine = new ShanoColor(127, 255, 212);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {240, 255, 255}. 
        /// </summary>
        public static readonly ShanoColor Azure = new ShanoColor(240, 255, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {245, 245, 220}. 
        /// </summary>
        public static readonly ShanoColor Beige = new ShanoColor(245, 245, 220);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 228, 196}. 
        /// </summary>
        public static readonly ShanoColor Bisque = new ShanoColor(255, 228, 196);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 0}. 
        /// </summary>
        public static readonly ShanoColor Black = new ShanoColor(0, 0, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 235, 205}. 
        /// </summary>
        public static readonly ShanoColor BlanchedAlmond = new ShanoColor(255, 235, 205);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 255}. 
        /// </summary>
        public static readonly ShanoColor Blue = new ShanoColor(0, 0, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {138, 43, 226}. 
        /// </summary>
        public static readonly ShanoColor BlueViolet = new ShanoColor(138, 43, 226);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {165, 42, 42}. 
        /// </summary>
        public static readonly ShanoColor Brown = new ShanoColor(165, 42, 42);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {222, 184, 135}. 
        /// </summary>
        public static readonly ShanoColor Burlywood = new ShanoColor(222, 184, 135);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {95, 158, 160}. 
        /// </summary>
        public static readonly ShanoColor CadetBlue = new ShanoColor(95, 158, 160);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {127, 255, 0}. 
        /// </summary>
        public static readonly ShanoColor Chartreuse = new ShanoColor(127, 255, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {210, 105, 30}. 
        /// </summary>
        public static readonly ShanoColor Chocolate = new ShanoColor(210, 105, 30);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 127, 80}. 
        /// </summary>
        public static readonly ShanoColor Coral = new ShanoColor(255, 127, 80);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {100, 149, 237}. 
        /// </summary>
        public static readonly ShanoColor CornflowerBlue = new ShanoColor(100, 149, 237);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 248, 220}. 
        /// </summary>
        public static readonly ShanoColor Cornsilk = new ShanoColor(255, 248, 220);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {220, 20, 60}. 
        /// </summary>
        public static readonly ShanoColor Crimson = new ShanoColor(220, 20, 60);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 255, 255}. 
        /// </summary>
        public static readonly ShanoColor Cyan = new ShanoColor(0, 255, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 139}. 
        /// </summary>
        public static readonly ShanoColor DarkBlue = new ShanoColor(0, 0, 139);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 139, 139}. 
        /// </summary>
        public static readonly ShanoColor DarkCyan = new ShanoColor(0, 139, 139);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {184, 134, 11}. 
        /// </summary>
        public static readonly ShanoColor DarkGoldenrod = new ShanoColor(184, 134, 11);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {169, 169, 169}. 
        /// </summary>
        public static readonly ShanoColor DarkGray = new ShanoColor(169, 169, 169);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 100, 0}. 
        /// </summary>
        public static readonly ShanoColor DarkGreen = new ShanoColor(0, 100, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {189, 183, 107}. 
        /// </summary>
        public static readonly ShanoColor DarkKhaki = new ShanoColor(189, 183, 107);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {139, 0, 139}. 
        /// </summary>
        public static readonly ShanoColor DarkMagenta = new ShanoColor(139, 0, 139);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {85, 107, 47}. 
        /// </summary>
        public static readonly ShanoColor DarkOliveGreen = new ShanoColor(85, 107, 47);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 140, 0}. 
        /// </summary>
        public static readonly ShanoColor DarkOrange = new ShanoColor(255, 140, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {153, 50, 204}. 
        /// </summary>
        public static readonly ShanoColor DarkOrchid = new ShanoColor(153, 50, 204);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {139, 0, 0}. 
        /// </summary>
        public static readonly ShanoColor DarkRed = new ShanoColor(139, 0, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {233, 150, 122}. 
        /// </summary>
        public static readonly ShanoColor DarkSalmon = new ShanoColor(233, 150, 122);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {143, 188, 139}. 
        /// </summary>
        public static readonly ShanoColor DarkseaGreen = new ShanoColor(143, 188, 139);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {72, 61, 139}. 
        /// </summary>
        public static readonly ShanoColor DarkslateBlue = new ShanoColor(72, 61, 139);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {47, 79, 79}. 
        /// </summary>
        public static readonly ShanoColor DarkslateGray = new ShanoColor(47, 79, 79);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 206, 209}. 
        /// </summary>
        public static readonly ShanoColor DarkTurquoise = new ShanoColor(0, 206, 209);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {148, 0, 211}. 
        /// </summary>
        public static readonly ShanoColor DarkViolet = new ShanoColor(148, 0, 211);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 20, 147}. 
        /// </summary>
        public static readonly ShanoColor DeepPink = new ShanoColor(255, 20, 147);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 191, 255}. 
        /// </summary>
        public static readonly ShanoColor DeepSkyBlue = new ShanoColor(0, 191, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {105, 105, 105}. 
        /// </summary>
        public static readonly ShanoColor DimGray = new ShanoColor(105, 105, 105);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {30, 144, 255}. 
        /// </summary>
        public static readonly ShanoColor DodgerBlue = new ShanoColor(30, 144, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {178, 34, 34}. 
        /// </summary>
        public static readonly ShanoColor Firebrick = new ShanoColor(178, 34, 34);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 250, 240}. 
        /// </summary>
        public static readonly ShanoColor FloralWhite = new ShanoColor(255, 250, 240);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {34, 139, 34}. 
        /// </summary>
        public static readonly ShanoColor ForestGreen = new ShanoColor(34, 139, 34);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 0, 255}. 
        /// </summary>
        public static readonly ShanoColor Fuchsia = new ShanoColor(255, 0, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {220, 220, 220}. 
        /// </summary>
        public static readonly ShanoColor Gainsboro = new ShanoColor(220, 220, 220);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {248, 248, 255}. 
        /// </summary>
        public static readonly ShanoColor GhostWhite = new ShanoColor(248, 248, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 215, 0}. 
        /// </summary>
        public static readonly ShanoColor Gold = new ShanoColor(255, 215, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {218, 165, 32}. 
        /// </summary>
        public static readonly ShanoColor Goldenrod = new ShanoColor(218, 165, 32);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {128, 128, 128}. 
        /// </summary>
        public static readonly ShanoColor Gray = new ShanoColor(128, 128, 128);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 128, 0}. 
        /// </summary>
        public static readonly ShanoColor Green = new ShanoColor(0, 128, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {173, 255, 47}. 
        /// </summary>
        public static readonly ShanoColor GreenYellow = new ShanoColor(173, 255, 47);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {240, 255, 240}. 
        /// </summary>
        public static readonly ShanoColor Honeydew = new ShanoColor(240, 255, 240);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 105, 180}. 
        /// </summary>
        public static readonly ShanoColor HotPink = new ShanoColor(255, 105, 180);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {205, 92, 92}. 
        /// </summary>
        public static readonly ShanoColor IndianRed = new ShanoColor(205, 92, 92);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {75, 0, 130}. 
        /// </summary>
        public static readonly ShanoColor Indigo = new ShanoColor(75, 0, 130);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 255, 240}. 
        /// </summary>
        public static readonly ShanoColor Ivory = new ShanoColor(255, 255, 240);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {240, 230, 140}. 
        /// </summary>
        public static readonly ShanoColor Khaki = new ShanoColor(240, 230, 140);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {230, 230, 250}. 
        /// </summary>
        public static readonly ShanoColor Lavender = new ShanoColor(230, 230, 250);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 240, 245}. 
        /// </summary>
        public static readonly ShanoColor LavenderBlush = new ShanoColor(255, 240, 245);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {124, 252, 0}. 
        /// </summary>
        public static readonly ShanoColor LawnGreen = new ShanoColor(124, 252, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 250, 205}. 
        /// </summary>
        public static readonly ShanoColor LemonChiffon = new ShanoColor(255, 250, 205);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {173, 216, 230}. 
        /// </summary>
        public static readonly ShanoColor LightBlue = new ShanoColor(173, 216, 230);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {240, 128, 128}. 
        /// </summary>
        public static readonly ShanoColor LightCoral = new ShanoColor(240, 128, 128);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {224, 255, 255}. 
        /// </summary>
        public static readonly ShanoColor LightCyan = new ShanoColor(224, 255, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {250, 250, 210}. 
        /// </summary>
        public static readonly ShanoColor LightGoldenrodYellow = new ShanoColor(250, 250, 210);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {211, 211, 211}. 
        /// </summary>
        public static readonly ShanoColor LightGray = new ShanoColor(211, 211, 211);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {144, 238, 144}. 
        /// </summary>
        public static readonly ShanoColor LightGreen = new ShanoColor(144, 238, 144);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 182, 193}. 
        /// </summary>
        public static readonly ShanoColor LightPink = new ShanoColor(255, 182, 193);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 160, 122}. 
        /// </summary>
        public static readonly ShanoColor LightSalmon = new ShanoColor(255, 160, 122);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {32, 178, 170}. 
        /// </summary>
        public static readonly ShanoColor LightSeaGreen = new ShanoColor(32, 178, 170);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {135, 206, 250}. 
        /// </summary>
        public static readonly ShanoColor LightSkyBlue = new ShanoColor(135, 206, 250);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {119, 136, 153}. 
        /// </summary>
        public static readonly ShanoColor LightSlateGray = new ShanoColor(119, 136, 153);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {176, 196, 222}. 
        /// </summary>
        public static readonly ShanoColor LightSteelBlue = new ShanoColor(176, 196, 222);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 255, 224}. 
        /// </summary>
        public static readonly ShanoColor LightYellow = new ShanoColor(255, 255, 224);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 255, 0}. 
        /// </summary>
        public static readonly ShanoColor Lime = new ShanoColor(0, 255, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {50, 205, 50}. 
        /// </summary>
        public static readonly ShanoColor LimeGreen = new ShanoColor(50, 205, 50);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {250, 240, 230}. 
        /// </summary>
        public static readonly ShanoColor Linen = new ShanoColor(250, 240, 230);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 0, 255}. 
        /// </summary>
        public static readonly ShanoColor Magenta = new ShanoColor(255, 0, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {128, 0, 0}. 
        /// </summary>
        public static readonly ShanoColor Maroon = new ShanoColor(128, 0, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {102, 205, 170}. 
        /// </summary>
        public static readonly ShanoColor MediumAquamarine = new ShanoColor(102, 205, 170);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 205}. 
        /// </summary>
        public static readonly ShanoColor MediumBlue = new ShanoColor(0, 0, 205);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {186, 85, 211}. 
        /// </summary>
        public static readonly ShanoColor MediumOrchid = new ShanoColor(186, 85, 211);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {147, 112, 219}. 
        /// </summary>
        public static readonly ShanoColor MediumPurple = new ShanoColor(147, 112, 219);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {60, 179, 113}. 
        /// </summary>
        public static readonly ShanoColor MediumSeaGreen = new ShanoColor(60, 179, 113);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {123, 104, 238}. 
        /// </summary>
        public static readonly ShanoColor MediumSlateBlue = new ShanoColor(123, 104, 238);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 250, 154}. 
        /// </summary>
        public static readonly ShanoColor MediumSpringGreen = new ShanoColor(0, 250, 154);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {72, 209, 204}. 
        /// </summary>
        public static readonly ShanoColor MediumTurquoise = new ShanoColor(72, 209, 204);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {199, 21, 133}. 
        /// </summary>
        public static readonly ShanoColor MediumVioletRed = new ShanoColor(199, 21, 133);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {25, 25, 112}. 
        /// </summary>
        public static readonly ShanoColor MidnightBlue = new ShanoColor(25, 25, 112);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {245, 255, 250}. 
        /// </summary>
        public static readonly ShanoColor MintCream = new ShanoColor(245, 255, 250);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 228, 225}. 
        /// </summary>
        public static readonly ShanoColor MistyRose = new ShanoColor(255, 228, 225);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 228, 181}. 
        /// </summary>
        public static readonly ShanoColor Moccasin = new ShanoColor(255, 228, 181);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 222, 173}. 
        /// </summary>
        public static readonly ShanoColor NavajoWhite = new ShanoColor(255, 222, 173);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 128}. 
        /// </summary>
        public static readonly ShanoColor Navy = new ShanoColor(0, 0, 128);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {253, 245, 230}. 
        /// </summary>
        public static readonly ShanoColor OldLace = new ShanoColor(253, 245, 230);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {128, 128, 0}. 
        /// </summary>
        public static readonly ShanoColor Olive = new ShanoColor(128, 128, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {107, 142, 35}. 
        /// </summary>
        public static readonly ShanoColor OliveDrab = new ShanoColor(107, 142, 35);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 165, 0}. 
        /// </summary>
        public static readonly ShanoColor Orange = new ShanoColor(255, 165, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 69, 0}. 
        /// </summary>
        public static readonly ShanoColor OrangeRed = new ShanoColor(255, 69, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {218, 112, 214}. 
        /// </summary>
        public static readonly ShanoColor Orchid = new ShanoColor(218, 112, 214);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {238, 232, 170}. 
        /// </summary>
        public static readonly ShanoColor PaleGoldenrod = new ShanoColor(238, 232, 170);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {152, 251, 152}. 
        /// </summary>
        public static readonly ShanoColor PaleGreen = new ShanoColor(152, 251, 152);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {175, 238, 238}. 
        /// </summary>
        public static readonly ShanoColor PaleTurquoise = new ShanoColor(175, 238, 238);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {219, 112, 147}. 
        /// </summary>
        public static readonly ShanoColor PaleVioletRed = new ShanoColor(219, 112, 147);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 239, 213}. 
        /// </summary>
        public static readonly ShanoColor PapayaWhip = new ShanoColor(255, 239, 213);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 218, 185}. 
        /// </summary>
        public static readonly ShanoColor PeachPuff = new ShanoColor(255, 218, 185);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {205, 133, 63}. 
        /// </summary>
        public static readonly ShanoColor Peru = new ShanoColor(205, 133, 63);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 192, 203}. 
        /// </summary>
        public static readonly ShanoColor Pink = new ShanoColor(255, 192, 203);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {221, 160, 221}. 
        /// </summary>
        public static readonly ShanoColor Plum = new ShanoColor(221, 160, 221);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {176, 224, 230}. 
        /// </summary>
        public static readonly ShanoColor PowderBlue = new ShanoColor(176, 224, 230);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {128, 0, 128}. 
        /// </summary>
        public static readonly ShanoColor Purple = new ShanoColor(128, 0, 128);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 0, 0}. 
        /// </summary>
        public static readonly ShanoColor Red = new ShanoColor(255, 0, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {188, 143, 143}. 
        /// </summary>
        public static readonly ShanoColor RosyBrown = new ShanoColor(188, 143, 143);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {65, 105, 225}. 
        /// </summary>
        public static readonly ShanoColor RoyalBlue = new ShanoColor(65, 105, 225);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {139, 69, 19}. 
        /// </summary>
        public static readonly ShanoColor SaddleBrown = new ShanoColor(139, 69, 19);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {250, 128, 114}. 
        /// </summary>
        public static readonly ShanoColor Salmon = new ShanoColor(250, 128, 114);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {244, 164, 96}. 
        /// </summary>
        public static readonly ShanoColor SandyBrown = new ShanoColor(244, 164, 96);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {46, 139, 87}. 
        /// </summary>
        public static readonly ShanoColor SeaGreen = new ShanoColor(46, 139, 87);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 245, 238}. 
        /// </summary>
        public static readonly ShanoColor SeaShell = new ShanoColor(255, 245, 238);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {160, 82, 45}. 
        /// </summary>
        public static readonly ShanoColor Sienna = new ShanoColor(160, 82, 45);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {192, 192, 192}. 
        /// </summary>
        public static readonly ShanoColor Silver = new ShanoColor(192, 192, 192);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {135, 206, 235}. 
        /// </summary>
        public static readonly ShanoColor SkyBlue = new ShanoColor(135, 206, 235);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {106, 90, 205}. 
        /// </summary>
        public static readonly ShanoColor SlateBlue = new ShanoColor(106, 90, 205);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {112, 128, 144}. 
        /// </summary>
        public static readonly ShanoColor SlateGray = new ShanoColor(112, 128, 144);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 250, 250}. 
        /// </summary>
        public static readonly ShanoColor Snow = new ShanoColor(255, 250, 250);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 255, 127}. 
        /// </summary>
        public static readonly ShanoColor SpringGreen = new ShanoColor(0, 255, 127);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {70, 130, 180}. 
        /// </summary>
        public static readonly ShanoColor SteelBlue = new ShanoColor(70, 130, 180);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {210, 180, 140}. 
        /// </summary>
        public static readonly ShanoColor Tan = new ShanoColor(210, 180, 140);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 128, 128}. 
        /// </summary>
        public static readonly ShanoColor Teal = new ShanoColor(0, 128, 128);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {216, 191, 216}. 
        /// </summary>
        public static readonly ShanoColor Thistle = new ShanoColor(216, 191, 216);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 99, 71}. 
        /// </summary>
        public static readonly ShanoColor Tomato = new ShanoColor(255, 99, 71);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {64, 224, 208}. 
        /// </summary>
        public static readonly ShanoColor Turquoise = new ShanoColor(64, 224, 208);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {238, 130, 238}. 
        /// </summary>
        public static readonly ShanoColor Violet = new ShanoColor(238, 130, 238);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {245, 222, 179}. 
        /// </summary>
        public static readonly ShanoColor Wheat = new ShanoColor(245, 222, 179);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 255, 255}. 
        /// </summary>
        public static readonly ShanoColor White = new ShanoColor(255, 255, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {245, 245, 245}. 
        /// </summary>
        public static readonly ShanoColor WhiteSmoke = new ShanoColor(245, 245, 245);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 255, 0}. 
        /// </summary>
        public static readonly ShanoColor Yellow = new ShanoColor(255, 255, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {154, 205, 50}. 
        /// </summary>
        public static readonly ShanoColor YellowGreen = new ShanoColor(154, 205, 50);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 0, 0}. 
        /// </summary>
        public static readonly ShanoColor Transparent = new ShanoColor(0, 0, 0, 0);
        #endregion


        /// <summary>
        /// Gets the Red component of this color. 
        /// </summary>
        public byte R;
        /// <summary>
        /// Gets the Green component of this color. 
        /// </summary>
        public byte G;
        /// <summary>
        /// Gets the Blue component of this color. 
        /// </summary>
        public byte B;

        /// <summary>
        /// Gets the Alpha component of this color. 
        /// </summary>
        public byte A;

        /// <summary>
        /// Creates a new color of the specified RGB values and full alpha. 
        /// </summary>
        /// <param name="r">The red component as a value in the range 0-255.</param>
        /// <param name="g">The blue component as a value in the range 0-255.</param>
        /// <param name="b">The green component as a value in the range 0-255.</param>
        public ShanoColor(byte r, byte g, byte b)
            : this(r, g, b, 255) { }

        /// <summary>
        /// Creates a new color of the specified RGB and alpha values. 
        /// </summary>
        /// <param name="r">The red component as a value in the range 0-255.</param>
        /// <param name="g">The blue component as a value in the range 0-255.</param>
        /// <param name="b">The green component as a value in the range 0-255.</param>
        /// <param name="a">The alpha component as a value in the range 0-255.</param>
        public ShanoColor(byte r, byte g, byte b, byte a)
        {
            R = r; G = g; B = b; A = a;
        }


        /// <summary>
        /// Returns a new color with the alpha value multiplied from 0 to 255. 
        /// </summary>
        /// <param name="a">The alpha component of the returned color as a value in the range 0-255.</param>
        /// <returns></returns>
        public ShanoColor SetAlpha(byte a)
        {
            return new ShanoColor(R, G, B, a);
        }

        /// <summary>
        /// Returns a darker version of the provided color. 
        /// </summary>
        /// <param name="perc">The amount of darkening to apply. Should be an int between 0 and 100. </param>
        /// <returns></returns>
        public ShanoColor Darken(int perc = 5)
        {
            unchecked
            {
                var ratio = 100 - perc;
                return new ShanoColor((byte)(R * ratio / 100), (byte)(G * ratio / 100), (byte)(B * ratio / 100), A);
            }
        }

        /// <summary>
        /// Returns a brighter version of the provided color. 
        /// </summary>
        /// <param name="perc">The amount of brightening to apply. Should be an int between 0 and 100. </param>
        /// <returns></returns>
        public ShanoColor Brighten(int perc = 5)
        {
            unchecked
            {
                return new ShanoColor(
                    (byte)(R + (255 - R) * perc / 100),
                    (byte)(G + (255 - G) * perc / 100),
                    (byte)(B + (255 - B) * perc / 100), A);
            }
        }

        /// <summary>
        /// Deserializes the data from the specified reader into this object.
        /// </summary>
        public void Deserialize(BinaryReader r)
        {
            R = r.ReadByte();
            G = r.ReadByte();
            B = r.ReadByte();
            A = r.ReadByte();
        }

        /// <summary>
        /// Serializes this object to the given writer.
        /// </summary>
        public void Serialize(BinaryWriter w)
        {
            w.Write(R);
            w.Write(G);
            w.Write(B);
            w.Write(A);
        }
    }
}
