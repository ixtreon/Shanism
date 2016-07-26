using IxSerializer.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Shanism.Common.Util
{
    /// <summary>
    /// Represents a standard ARGB color value. 
    /// </summary>
    /// <seealso cref="IxSerializer.Modules.IxSerializable" />
    public struct Color : IxSerializable
    {
        #region Static Members
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {240, 248, 255}. 
        /// </summary>
        public static readonly Color AliceBlue = new Color(240, 248, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {250, 235, 215}. 
        /// </summary>
        public static readonly Color AntiqueWhite = new Color(250, 235, 215);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 255, 255}. 
        /// </summary>
        public static readonly Color Aqua = new Color(0, 255, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {127, 255, 212}. 
        /// </summary>
        public static readonly Color Aquamarine = new Color(127, 255, 212);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {240, 255, 255}. 
        /// </summary>
        public static readonly Color Azure = new Color(240, 255, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {245, 245, 220}. 
        /// </summary>
        public static readonly Color Beige = new Color(245, 245, 220);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 228, 196}. 
        /// </summary>
        public static readonly Color Bisque = new Color(255, 228, 196);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 0}. 
        /// </summary>
        public static readonly Color Black = new Color(0, 0, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 235, 205}. 
        /// </summary>
        public static readonly Color BlanchedAlmond = new Color(255, 235, 205);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 255}. 
        /// </summary>
        public static readonly Color Blue = new Color(0, 0, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {138, 43, 226}. 
        /// </summary>
        public static readonly Color BlueViolet = new Color(138, 43, 226);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {165, 42, 42}. 
        /// </summary>
        public static readonly Color Brown = new Color(165, 42, 42);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {222, 184, 135}. 
        /// </summary>
        public static readonly Color Burlywood = new Color(222, 184, 135);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {95, 158, 160}. 
        /// </summary>
        public static readonly Color CadetBlue = new Color(95, 158, 160);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {127, 255, 0}. 
        /// </summary>
        public static readonly Color Chartreuse = new Color(127, 255, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {210, 105, 30}. 
        /// </summary>
        public static readonly Color Chocolate = new Color(210, 105, 30);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 127, 80}. 
        /// </summary>
        public static readonly Color Coral = new Color(255, 127, 80);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {100, 149, 237}. 
        /// </summary>
        public static readonly Color CornflowerBlue = new Color(100, 149, 237);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 248, 220}. 
        /// </summary>
        public static readonly Color Cornsilk = new Color(255, 248, 220);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {220, 20, 60}. 
        /// </summary>
        public static readonly Color Crimson = new Color(220, 20, 60);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 255, 255}. 
        /// </summary>
        public static readonly Color Cyan = new Color(0, 255, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 139}. 
        /// </summary>
        public static readonly Color DarkBlue = new Color(0, 0, 139);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 139, 139}. 
        /// </summary>
        public static readonly Color DarkCyan = new Color(0, 139, 139);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {184, 134, 11}. 
        /// </summary>
        public static readonly Color DarkGoldenrod = new Color(184, 134, 11);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {169, 169, 169}. 
        /// </summary>
        public static readonly Color DarkGray = new Color(169, 169, 169);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 100, 0}. 
        /// </summary>
        public static readonly Color DarkGreen = new Color(0, 100, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {189, 183, 107}. 
        /// </summary>
        public static readonly Color DarkKhaki = new Color(189, 183, 107);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {139, 0, 139}. 
        /// </summary>
        public static readonly Color DarkMagenta = new Color(139, 0, 139);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {85, 107, 47}. 
        /// </summary>
        public static readonly Color DarkOliveGreen = new Color(85, 107, 47);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 140, 0}. 
        /// </summary>
        public static readonly Color DarkOrange = new Color(255, 140, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {153, 50, 204}. 
        /// </summary>
        public static readonly Color DarkOrchid = new Color(153, 50, 204);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {139, 0, 0}. 
        /// </summary>
        public static readonly Color DarkRed = new Color(139, 0, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {233, 150, 122}. 
        /// </summary>
        public static readonly Color DarkSalmon = new Color(233, 150, 122);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {143, 188, 139}. 
        /// </summary>
        public static readonly Color DarkseaGreen = new Color(143, 188, 139);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {72, 61, 139}. 
        /// </summary>
        public static readonly Color DarkslateBlue = new Color(72, 61, 139);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {47, 79, 79}. 
        /// </summary>
        public static readonly Color DarkslateGray = new Color(47, 79, 79);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 206, 209}. 
        /// </summary>
        public static readonly Color DarkTurquoise = new Color(0, 206, 209);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {148, 0, 211}. 
        /// </summary>
        public static readonly Color DarkViolet = new Color(148, 0, 211);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 20, 147}. 
        /// </summary>
        public static readonly Color DeepPink = new Color(255, 20, 147);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 191, 255}. 
        /// </summary>
        public static readonly Color DeepSkyBlue = new Color(0, 191, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {105, 105, 105}. 
        /// </summary>
        public static readonly Color DimGray = new Color(105, 105, 105);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {30, 144, 255}. 
        /// </summary>
        public static readonly Color DodgerBlue = new Color(30, 144, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {178, 34, 34}. 
        /// </summary>
        public static readonly Color Firebrick = new Color(178, 34, 34);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 250, 240}. 
        /// </summary>
        public static readonly Color FloralWhite = new Color(255, 250, 240);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {34, 139, 34}. 
        /// </summary>
        public static readonly Color ForestGreen = new Color(34, 139, 34);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 0, 255}. 
        /// </summary>
        public static readonly Color Fuchsia = new Color(255, 0, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {220, 220, 220}. 
        /// </summary>
        public static readonly Color Gainsboro = new Color(220, 220, 220);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {248, 248, 255}. 
        /// </summary>
        public static readonly Color GhostWhite = new Color(248, 248, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 215, 0}. 
        /// </summary>
        public static readonly Color Gold = new Color(255, 215, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {218, 165, 32}. 
        /// </summary>
        public static readonly Color Goldenrod = new Color(218, 165, 32);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {128, 128, 128}. 
        /// </summary>
        public static readonly Color Gray = new Color(128, 128, 128);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 128, 0}. 
        /// </summary>
        public static readonly Color Green = new Color(0, 128, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {173, 255, 47}. 
        /// </summary>
        public static readonly Color GreenYellow = new Color(173, 255, 47);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {240, 255, 240}. 
        /// </summary>
        public static readonly Color Honeydew = new Color(240, 255, 240);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 105, 180}. 
        /// </summary>
        public static readonly Color HotPink = new Color(255, 105, 180);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {205, 92, 92}. 
        /// </summary>
        public static readonly Color IndianRed = new Color(205, 92, 92);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {75, 0, 130}. 
        /// </summary>
        public static readonly Color Indigo = new Color(75, 0, 130);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 255, 240}. 
        /// </summary>
        public static readonly Color Ivory = new Color(255, 255, 240);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {240, 230, 140}. 
        /// </summary>
        public static readonly Color Khaki = new Color(240, 230, 140);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {230, 230, 250}. 
        /// </summary>
        public static readonly Color Lavender = new Color(230, 230, 250);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 240, 245}. 
        /// </summary>
        public static readonly Color LavenderBlush = new Color(255, 240, 245);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {124, 252, 0}. 
        /// </summary>
        public static readonly Color LawnGreen = new Color(124, 252, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 250, 205}. 
        /// </summary>
        public static readonly Color LemonChiffon = new Color(255, 250, 205);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {173, 216, 230}. 
        /// </summary>
        public static readonly Color LightBlue = new Color(173, 216, 230);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {240, 128, 128}. 
        /// </summary>
        public static readonly Color LightCoral = new Color(240, 128, 128);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {224, 255, 255}. 
        /// </summary>
        public static readonly Color LightCyan = new Color(224, 255, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {250, 250, 210}. 
        /// </summary>
        public static readonly Color LightGoldenrodYellow = new Color(250, 250, 210);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {211, 211, 211}. 
        /// </summary>
        public static readonly Color LightGray = new Color(211, 211, 211);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {144, 238, 144}. 
        /// </summary>
        public static readonly Color LightGreen = new Color(144, 238, 144);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 182, 193}. 
        /// </summary>
        public static readonly Color LightPink = new Color(255, 182, 193);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 160, 122}. 
        /// </summary>
        public static readonly Color LightSalmon = new Color(255, 160, 122);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {32, 178, 170}. 
        /// </summary>
        public static readonly Color LightSeaGreen = new Color(32, 178, 170);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {135, 206, 250}. 
        /// </summary>
        public static readonly Color LightSkyBlue = new Color(135, 206, 250);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {119, 136, 153}. 
        /// </summary>
        public static readonly Color LightSlateGray = new Color(119, 136, 153);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {176, 196, 222}. 
        /// </summary>
        public static readonly Color LightSteelBlue = new Color(176, 196, 222);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 255, 224}. 
        /// </summary>
        public static readonly Color LightYellow = new Color(255, 255, 224);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 255, 0}. 
        /// </summary>
        public static readonly Color Lime = new Color(0, 255, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {50, 205, 50}. 
        /// </summary>
        public static readonly Color LimeGreen = new Color(50, 205, 50);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {250, 240, 230}. 
        /// </summary>
        public static readonly Color Linen = new Color(250, 240, 230);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 0, 255}. 
        /// </summary>
        public static readonly Color Magenta = new Color(255, 0, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {128, 0, 0}. 
        /// </summary>
        public static readonly Color Maroon = new Color(128, 0, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {102, 205, 170}. 
        /// </summary>
        public static readonly Color MediumAquamarine = new Color(102, 205, 170);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 205}. 
        /// </summary>
        public static readonly Color MediumBlue = new Color(0, 0, 205);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {186, 85, 211}. 
        /// </summary>
        public static readonly Color MediumOrchid = new Color(186, 85, 211);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {147, 112, 219}. 
        /// </summary>
        public static readonly Color MediumPurple = new Color(147, 112, 219);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {60, 179, 113}. 
        /// </summary>
        public static readonly Color MediumSeaGreen = new Color(60, 179, 113);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {123, 104, 238}. 
        /// </summary>
        public static readonly Color MediumSlateBlue = new Color(123, 104, 238);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 250, 154}. 
        /// </summary>
        public static readonly Color MediumSpringGreen = new Color(0, 250, 154);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {72, 209, 204}. 
        /// </summary>
        public static readonly Color MediumTurquoise = new Color(72, 209, 204);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {199, 21, 133}. 
        /// </summary>
        public static readonly Color MediumVioletRed = new Color(199, 21, 133);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {25, 25, 112}. 
        /// </summary>
        public static readonly Color MidnightBlue = new Color(25, 25, 112);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {245, 255, 250}. 
        /// </summary>
        public static readonly Color MintCream = new Color(245, 255, 250);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 228, 225}. 
        /// </summary>
        public static readonly Color MistyRose = new Color(255, 228, 225);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 228, 181}. 
        /// </summary>
        public static readonly Color Moccasin = new Color(255, 228, 181);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 222, 173}. 
        /// </summary>
        public static readonly Color NavajoWhite = new Color(255, 222, 173);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 128}. 
        /// </summary>
        public static readonly Color Navy = new Color(0, 0, 128);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {253, 245, 230}. 
        /// </summary>
        public static readonly Color OldLace = new Color(253, 245, 230);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {128, 128, 0}. 
        /// </summary>
        public static readonly Color Olive = new Color(128, 128, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {107, 142, 35}. 
        /// </summary>
        public static readonly Color OliveDrab = new Color(107, 142, 35);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 165, 0}. 
        /// </summary>
        public static readonly Color Orange = new Color(255, 165, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 69, 0}. 
        /// </summary>
        public static readonly Color OrangeRed = new Color(255, 69, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {218, 112, 214}. 
        /// </summary>
        public static readonly Color Orchid = new Color(218, 112, 214);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {238, 232, 170}. 
        /// </summary>
        public static readonly Color PaleGoldenrod = new Color(238, 232, 170);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {152, 251, 152}. 
        /// </summary>
        public static readonly Color PaleGreen = new Color(152, 251, 152);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {175, 238, 238}. 
        /// </summary>
        public static readonly Color PaleTurquoise = new Color(175, 238, 238);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {219, 112, 147}. 
        /// </summary>
        public static readonly Color PaleVioletRed = new Color(219, 112, 147);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 239, 213}. 
        /// </summary>
        public static readonly Color PapayaWhip = new Color(255, 239, 213);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 218, 185}. 
        /// </summary>
        public static readonly Color PeachPuff = new Color(255, 218, 185);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {205, 133, 63}. 
        /// </summary>
        public static readonly Color Peru = new Color(205, 133, 63);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 192, 203}. 
        /// </summary>
        public static readonly Color Pink = new Color(255, 192, 203);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {221, 160, 221}. 
        /// </summary>
        public static readonly Color Plum = new Color(221, 160, 221);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {176, 224, 230}. 
        /// </summary>
        public static readonly Color PowderBlue = new Color(176, 224, 230);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {128, 0, 128}. 
        /// </summary>
        public static readonly Color Purple = new Color(128, 0, 128);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 0, 0}. 
        /// </summary>
        public static readonly Color Red = new Color(255, 0, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {188, 143, 143}. 
        /// </summary>
        public static readonly Color RosyBrown = new Color(188, 143, 143);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {65, 105, 225}. 
        /// </summary>
        public static readonly Color RoyalBlue = new Color(65, 105, 225);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {139, 69, 19}. 
        /// </summary>
        public static readonly Color SaddleBrown = new Color(139, 69, 19);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {250, 128, 114}. 
        /// </summary>
        public static readonly Color Salmon = new Color(250, 128, 114);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {244, 164, 96}. 
        /// </summary>
        public static readonly Color SandyBrown = new Color(244, 164, 96);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {46, 139, 87}. 
        /// </summary>
        public static readonly Color SeaGreen = new Color(46, 139, 87);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 245, 238}. 
        /// </summary>
        public static readonly Color SeaShell = new Color(255, 245, 238);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {160, 82, 45}. 
        /// </summary>
        public static readonly Color Sienna = new Color(160, 82, 45);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {192, 192, 192}. 
        /// </summary>
        public static readonly Color Silver = new Color(192, 192, 192);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {135, 206, 235}. 
        /// </summary>
        public static readonly Color SkyBlue = new Color(135, 206, 235);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {106, 90, 205}. 
        /// </summary>
        public static readonly Color SlateBlue = new Color(106, 90, 205);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {112, 128, 144}. 
        /// </summary>
        public static readonly Color SlateGray = new Color(112, 128, 144);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 250, 250}. 
        /// </summary>
        public static readonly Color Snow = new Color(255, 250, 250);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 255, 127}. 
        /// </summary>
        public static readonly Color SpringGreen = new Color(0, 255, 127);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {70, 130, 180}. 
        /// </summary>
        public static readonly Color SteelBlue = new Color(70, 130, 180);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {210, 180, 140}. 
        /// </summary>
        public static readonly Color Tan = new Color(210, 180, 140);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 128, 128}. 
        /// </summary>
        public static readonly Color Teal = new Color(0, 128, 128);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {216, 191, 216}. 
        /// </summary>
        public static readonly Color Thistle = new Color(216, 191, 216);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 99, 71}. 
        /// </summary>
        public static readonly Color Tomato = new Color(255, 99, 71);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {64, 224, 208}. 
        /// </summary>
        public static readonly Color Turquoise = new Color(64, 224, 208);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {238, 130, 238}. 
        /// </summary>
        public static readonly Color Violet = new Color(238, 130, 238);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {245, 222, 179}. 
        /// </summary>
        public static readonly Color Wheat = new Color(245, 222, 179);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 255, 255}. 
        /// </summary>
        public static readonly Color White = new Color(255, 255, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {245, 245, 245}. 
        /// </summary>
        public static readonly Color WhiteSmoke = new Color(245, 245, 245);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 255, 0}. 
        /// </summary>
        public static readonly Color Yellow = new Color(255, 255, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {154, 205, 50}. 
        /// </summary>
        public static readonly Color YellowGreen = new Color(154, 205, 50);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 0, 0}. 
        /// </summary>
        public static readonly Color Transparent = new Color(0, 0, 0, 0);
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
        public Color(byte r, byte g, byte b)
            : this(r, g, b, 255) { }

        /// <summary>
        /// Creates a new color of the specified RGB and alpha values. 
        /// </summary>
        /// <param name="r">The red component as a value in the range 0-255.</param>
        /// <param name="g">The blue component as a value in the range 0-255.</param>
        /// <param name="b">The green component as a value in the range 0-255.</param>
        /// <param name="a">The alpha component as a value in the range 0-255.</param>
        public Color(byte r, byte g, byte b, byte a)
        {
            R = r; G = g; B = b; A = a;
        }

        public Color(int val)
        {
            unchecked
            {
                R = (byte)(val >> 0);
                G = (byte)(val >> 8);
                B = (byte)(val >> 16);
                A = (byte)(val >> 24);
            }
        }

        public int Pack()
        {
            return (R << 0)
                | (G << 8)
                | (B << 16)
                | (A << 24);

        }


        /// <summary>
        /// Returns a new color with the alpha value multiplied from 0 to 255. 
        /// </summary>
        /// <param name="a">The alpha component of the returned color as a value in the range 0-255.</param>
        /// <returns></returns>
        public Color SetAlpha(byte a)
        {
            return new Color(R, G, B, a);
        }

        /// <summary>
        /// Returns a darker version of the provided color. 
        /// </summary>
        /// <param name="perc">The amount of darkening to apply. Should be an int between 0 and 100. </param>
        /// <returns></returns>
        public Color Darken(int perc = 5)
        {
            unchecked
            {
                var ratio = 100 - perc;
                return new Color((byte)(R * ratio / 100), (byte)(G * ratio / 100), (byte)(B * ratio / 100), A);
            }
        }

        /// <summary>
        /// Returns a brighter version of the provided color. 
        /// </summary>
        /// <param name="perc">The amount of brightening to apply. Should be an int between 0 and 100. </param>
        /// <returns></returns>
        public Color Brighten(int perc = 5)
        {
            unchecked
            {
                return new Color(
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

        public static bool operator ==(Color a, Color b)
        {
            return a.R == b.R
                && a.G == b.G
                && a.B == b.B
                && a.A == b.A;
        }
        public static bool operator !=(Color a, Color b) => !(a == b);

        public override bool Equals(object obj) 
            => (obj is Color) 
            && ((Color)obj == this);

        public override int GetHashCode() => Pack();
    }
}
