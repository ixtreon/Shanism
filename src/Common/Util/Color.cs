using System;

namespace Shanism.Common
{

    // TODO: use System.Drawing !!

    /// <summary>
    /// Represents a standard ARGB color value. 
    /// </summary>
    public struct Color : IEquatable<Color>
    {

        #region Predefined Colors
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {240, 248, 255}. 
        /// </summary>
        public static Color AliceBlue { get; } = new Color(240, 248, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {250, 235, 215}. 
        /// </summary>
        public static Color AntiqueWhite { get; } = new Color(250, 235, 215);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 255, 255}. 
        /// </summary>
        public static Color Aqua { get; } = new Color(0, 255, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {127, 255, 212}. 
        /// </summary>
        public static Color Aquamarine { get; } = new Color(127, 255, 212);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {240, 255, 255}. 
        /// </summary>
        public static Color Azure { get; } = new Color(240, 255, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {245, 245, 220}. 
        /// </summary>
        public static Color Beige { get; } = new Color(245, 245, 220);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 228, 196}. 
        /// </summary>
        public static Color Bisque { get; } = new Color(255, 228, 196);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 0}. 
        /// </summary>
        public static Color Black { get; } = new Color(0, 0, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 235, 205}. 
        /// </summary>
        public static Color BlanchedAlmond { get; } = new Color(255, 235, 205);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 255}. 
        /// </summary>
        public static Color Blue { get; } = new Color(0, 0, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {138, 43, 226}. 
        /// </summary>
        public static Color BlueViolet { get; } = new Color(138, 43, 226);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {165, 42, 42}. 
        /// </summary>
        public static Color Brown { get; } = new Color(165, 42, 42);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {222, 184, 135}. 
        /// </summary>
        public static Color Burlywood { get; } = new Color(222, 184, 135);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {95, 158, 160}. 
        /// </summary>
        public static Color CadetBlue { get; } = new Color(95, 158, 160);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {127, 255, 0}. 
        /// </summary>
        public static Color Chartreuse { get; } = new Color(127, 255, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {210, 105, 30}. 
        /// </summary>
        public static Color Chocolate { get; } = new Color(210, 105, 30);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 127, 80}. 
        /// </summary>
        public static Color Coral { get; } = new Color(255, 127, 80);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {100, 149, 237}. 
        /// </summary>
        public static Color CornflowerBlue { get; } = new Color(100, 149, 237);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 248, 220}. 
        /// </summary>
        public static Color Cornsilk { get; } = new Color(255, 248, 220);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {220, 20, 60}. 
        /// </summary>
        public static Color Crimson { get; } = new Color(220, 20, 60);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 255, 255}. 
        /// </summary>
        public static Color Cyan { get; } = new Color(0, 255, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 139}. 
        /// </summary>
        public static Color DarkBlue { get; } = new Color(0, 0, 139);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 139, 139}. 
        /// </summary>
        public static Color DarkCyan { get; } = new Color(0, 139, 139);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {184, 134, 11}. 
        /// </summary>
        public static Color DarkGoldenrod { get; } = new Color(184, 134, 11);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {169, 169, 169}. 
        /// </summary>
        public static Color DarkGray { get; } = new Color(169, 169, 169);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 100, 0}. 
        /// </summary>
        public static Color DarkGreen { get; } = new Color(0, 100, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {189, 183, 107}. 
        /// </summary>
        public static Color DarkKhaki { get; } = new Color(189, 183, 107);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {139, 0, 139}. 
        /// </summary>
        public static Color DarkMagenta { get; } = new Color(139, 0, 139);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {85, 107, 47}. 
        /// </summary>
        public static Color DarkOliveGreen { get; } = new Color(85, 107, 47);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 140, 0}. 
        /// </summary>
        public static Color DarkOrange { get; } = new Color(255, 140, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {153, 50, 204}. 
        /// </summary>
        public static Color DarkOrchid { get; } = new Color(153, 50, 204);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {139, 0, 0}. 
        /// </summary>
        public static Color DarkRed { get; } = new Color(139, 0, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {233, 150, 122}. 
        /// </summary>
        public static Color DarkSalmon { get; } = new Color(233, 150, 122);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {143, 188, 139}. 
        /// </summary>
        public static Color DarkseaGreen { get; } = new Color(143, 188, 139);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {72, 61, 139}. 
        /// </summary>
        public static Color DarkslateBlue { get; } = new Color(72, 61, 139);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {47, 79, 79}. 
        /// </summary>
        public static Color DarkslateGray { get; } = new Color(47, 79, 79);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 206, 209}. 
        /// </summary>
        public static Color DarkTurquoise { get; } = new Color(0, 206, 209);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {148, 0, 211}. 
        /// </summary>
        public static Color DarkViolet { get; } = new Color(148, 0, 211);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 20, 147}. 
        /// </summary>
        public static Color DeepPink { get; } = new Color(255, 20, 147);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 191, 255}. 
        /// </summary>
        public static Color DeepSkyBlue { get; } = new Color(0, 191, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {105, 105, 105}. 
        /// </summary>
        public static Color DimGray { get; } = new Color(105, 105, 105);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {30, 144, 255}. 
        /// </summary>
        public static Color DodgerBlue { get; } = new Color(30, 144, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {178, 34, 34}. 
        /// </summary>
        public static Color Firebrick { get; } = new Color(178, 34, 34);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 250, 240}. 
        /// </summary>
        public static Color FloralWhite { get; } = new Color(255, 250, 240);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {34, 139, 34}. 
        /// </summary>
        public static Color ForestGreen { get; } = new Color(34, 139, 34);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 0, 255}. 
        /// </summary>
        public static Color Fuchsia { get; } = new Color(255, 0, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {220, 220, 220}. 
        /// </summary>
        public static Color Gainsboro { get; } = new Color(220, 220, 220);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {248, 248, 255}. 
        /// </summary>
        public static Color GhostWhite { get; } = new Color(248, 248, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 215, 0}. 
        /// </summary>
        public static Color Gold { get; } = new Color(255, 215, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {218, 165, 32}. 
        /// </summary>
        public static Color Goldenrod { get; } = new Color(218, 165, 32);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {128, 128, 128}. 
        /// </summary>
        public static Color Gray { get; } = new Color(128, 128, 128);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 128, 0}. 
        /// </summary>
        public static Color Green { get; } = new Color(0, 128, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {173, 255, 47}. 
        /// </summary>
        public static Color GreenYellow { get; } = new Color(173, 255, 47);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {240, 255, 240}. 
        /// </summary>
        public static Color Honeydew { get; } = new Color(240, 255, 240);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 105, 180}. 
        /// </summary>
        public static Color HotPink { get; } = new Color(255, 105, 180);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {205, 92, 92}. 
        /// </summary>
        public static Color IndianRed { get; } = new Color(205, 92, 92);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {75, 0, 130}. 
        /// </summary>
        public static Color Indigo { get; } = new Color(75, 0, 130);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 255, 240}. 
        /// </summary>
        public static Color Ivory { get; } = new Color(255, 255, 240);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {240, 230, 140}. 
        /// </summary>
        public static Color Khaki { get; } = new Color(240, 230, 140);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {230, 230, 250}. 
        /// </summary>
        public static Color Lavender { get; } = new Color(230, 230, 250);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 240, 245}. 
        /// </summary>
        public static Color LavenderBlush { get; } = new Color(255, 240, 245);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {124, 252, 0}. 
        /// </summary>
        public static Color LawnGreen { get; } = new Color(124, 252, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 250, 205}. 
        /// </summary>
        public static Color LemonChiffon { get; } = new Color(255, 250, 205);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {173, 216, 230}. 
        /// </summary>
        public static Color LightBlue { get; } = new Color(173, 216, 230);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {240, 128, 128}. 
        /// </summary>
        public static Color LightCoral { get; } = new Color(240, 128, 128);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {224, 255, 255}. 
        /// </summary>
        public static Color LightCyan { get; } = new Color(224, 255, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {250, 250, 210}. 
        /// </summary>
        public static Color LightGoldenrodYellow { get; } = new Color(250, 250, 210);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {211, 211, 211}. 
        /// </summary>
        public static Color LightGray { get; } = new Color(211, 211, 211);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {144, 238, 144}. 
        /// </summary>
        public static Color LightGreen { get; } = new Color(144, 238, 144);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 182, 193}. 
        /// </summary>
        public static Color LightPink { get; } = new Color(255, 182, 193);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 160, 122}. 
        /// </summary>
        public static Color LightSalmon { get; } = new Color(255, 160, 122);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {32, 178, 170}. 
        /// </summary>
        public static Color LightSeaGreen { get; } = new Color(32, 178, 170);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {135, 206, 250}. 
        /// </summary>
        public static Color LightSkyBlue { get; } = new Color(135, 206, 250);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {119, 136, 153}. 
        /// </summary>
        public static Color LightSlateGray { get; } = new Color(119, 136, 153);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {176, 196, 222}. 
        /// </summary>
        public static Color LightSteelBlue { get; } = new Color(176, 196, 222);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 255, 224}. 
        /// </summary>
        public static Color LightYellow { get; } = new Color(255, 255, 224);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 255, 0}. 
        /// </summary>
        public static Color Lime { get; } = new Color(0, 255, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {50, 205, 50}. 
        /// </summary>
        public static Color LimeGreen { get; } = new Color(50, 205, 50);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {250, 240, 230}. 
        /// </summary>
        public static Color Linen { get; } = new Color(250, 240, 230);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 0, 255}. 
        /// </summary>
        public static Color Magenta { get; } = new Color(255, 0, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {128, 0, 0}. 
        /// </summary>
        public static Color Maroon { get; } = new Color(128, 0, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {102, 205, 170}. 
        /// </summary>
        public static Color MediumAquamarine { get; } = new Color(102, 205, 170);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 205}. 
        /// </summary>
        public static Color MediumBlue { get; } = new Color(0, 0, 205);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {186, 85, 211}. 
        /// </summary>
        public static Color MediumOrchid { get; } = new Color(186, 85, 211);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {147, 112, 219}. 
        /// </summary>
        public static Color MediumPurple { get; } = new Color(147, 112, 219);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {60, 179, 113}. 
        /// </summary>
        public static Color MediumSeaGreen { get; } = new Color(60, 179, 113);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {123, 104, 238}. 
        /// </summary>
        public static Color MediumSlateBlue { get; } = new Color(123, 104, 238);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 250, 154}. 
        /// </summary>
        public static Color MediumSpringGreen { get; } = new Color(0, 250, 154);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {72, 209, 204}. 
        /// </summary>
        public static Color MediumTurquoise { get; } = new Color(72, 209, 204);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {199, 21, 133}. 
        /// </summary>
        public static Color MediumVioletRed { get; } = new Color(199, 21, 133);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {25, 25, 112}. 
        /// </summary>
        public static Color MidnightBlue { get; } = new Color(25, 25, 112);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {245, 255, 250}. 
        /// </summary>
        public static Color MintCream { get; } = new Color(245, 255, 250);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 228, 225}. 
        /// </summary>
        public static Color MistyRose { get; } = new Color(255, 228, 225);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 228, 181}. 
        /// </summary>
        public static Color Moccasin { get; } = new Color(255, 228, 181);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 222, 173}. 
        /// </summary>
        public static Color NavajoWhite { get; } = new Color(255, 222, 173);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 128}. 
        /// </summary>
        public static Color Navy { get; } = new Color(0, 0, 128);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {253, 245, 230}. 
        /// </summary>
        public static Color OldLace { get; } = new Color(253, 245, 230);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {128, 128, 0}. 
        /// </summary>
        public static Color Olive { get; } = new Color(128, 128, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {107, 142, 35}. 
        /// </summary>
        public static Color OliveDrab { get; } = new Color(107, 142, 35);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 165, 0}. 
        /// </summary>
        public static Color Orange { get; } = new Color(255, 165, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 69, 0}. 
        /// </summary>
        public static Color OrangeRed { get; } = new Color(255, 69, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {218, 112, 214}. 
        /// </summary>
        public static Color Orchid { get; } = new Color(218, 112, 214);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {238, 232, 170}. 
        /// </summary>
        public static Color PaleGoldenrod { get; } = new Color(238, 232, 170);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {152, 251, 152}. 
        /// </summary>
        public static Color PaleGreen { get; } = new Color(152, 251, 152);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {175, 238, 238}. 
        /// </summary>
        public static Color PaleTurquoise { get; } = new Color(175, 238, 238);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {219, 112, 147}. 
        /// </summary>
        public static Color PaleVioletRed { get; } = new Color(219, 112, 147);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 239, 213}. 
        /// </summary>
        public static Color PapayaWhip { get; } = new Color(255, 239, 213);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 218, 185}. 
        /// </summary>
        public static Color PeachPuff { get; } = new Color(255, 218, 185);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {205, 133, 63}. 
        /// </summary>
        public static Color Peru { get; } = new Color(205, 133, 63);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 192, 203}. 
        /// </summary>
        public static Color Pink { get; } = new Color(255, 192, 203);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {221, 160, 221}. 
        /// </summary>
        public static Color Plum { get; } = new Color(221, 160, 221);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {176, 224, 230}. 
        /// </summary>
        public static Color PowderBlue { get; } = new Color(176, 224, 230);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {128, 0, 128}. 
        /// </summary>
        public static Color Purple { get; } = new Color(128, 0, 128);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 0, 0}. 
        /// </summary>
        public static Color Red { get; } = new Color(255, 0, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {188, 143, 143}. 
        /// </summary>
        public static Color RosyBrown { get; } = new Color(188, 143, 143);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {65, 105, 225}. 
        /// </summary>
        public static Color RoyalBlue { get; } = new Color(65, 105, 225);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {139, 69, 19}. 
        /// </summary>
        public static Color SaddleBrown { get; } = new Color(139, 69, 19);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {250, 128, 114}. 
        /// </summary>
        public static Color Salmon { get; } = new Color(250, 128, 114);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {244, 164, 96}. 
        /// </summary>
        public static Color SandyBrown { get; } = new Color(244, 164, 96);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {46, 139, 87}. 
        /// </summary>
        public static Color SeaGreen { get; } = new Color(46, 139, 87);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 245, 238}. 
        /// </summary>
        public static Color SeaShell { get; } = new Color(255, 245, 238);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {160, 82, 45}. 
        /// </summary>
        public static Color Sienna { get; } = new Color(160, 82, 45);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {192, 192, 192}. 
        /// </summary>
        public static Color Silver { get; } = new Color(192, 192, 192);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {135, 206, 235}. 
        /// </summary>
        public static Color SkyBlue { get; } = new Color(135, 206, 235);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {106, 90, 205}. 
        /// </summary>
        public static Color SlateBlue { get; } = new Color(106, 90, 205);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {112, 128, 144}. 
        /// </summary>
        public static Color SlateGray { get; } = new Color(112, 128, 144);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 250, 250}. 
        /// </summary>
        public static Color Snow { get; } = new Color(255, 250, 250);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 255, 127}. 
        /// </summary>
        public static Color SpringGreen { get; } = new Color(0, 255, 127);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {70, 130, 180}. 
        /// </summary>
        public static Color SteelBlue { get; } = new Color(70, 130, 180);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {210, 180, 140}. 
        /// </summary>
        public static Color Tan { get; } = new Color(210, 180, 140);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 128, 128}. 
        /// </summary>
        public static Color Teal { get; } = new Color(0, 128, 128);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {216, 191, 216}. 
        /// </summary>
        public static Color Thistle { get; } = new Color(216, 191, 216);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 99, 71}. 
        /// </summary>
        public static Color Tomato { get; } = new Color(255, 99, 71);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {64, 224, 208}. 
        /// </summary>
        public static Color Turquoise { get; } = new Color(64, 224, 208);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {238, 130, 238}. 
        /// </summary>
        public static Color Violet { get; } = new Color(238, 130, 238);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {245, 222, 179}. 
        /// </summary>
        public static Color Wheat { get; } = new Color(245, 222, 179);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 255, 255}. 
        /// </summary>
        public static Color White { get; } = new Color(255, 255, 255);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {245, 245, 245}. 
        /// </summary>
        public static Color WhiteSmoke { get; } = new Color(245, 245, 245);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {255, 255, 0}. 
        /// </summary>
        public static Color Yellow { get; } = new Color(255, 255, 0);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {154, 205, 50}. 
        /// </summary>
        public static Color YellowGreen { get; } = new Color(154, 205, 50);
        /// <summary>
        /// Gets a pre-defined color that has an ARGB value of {0, 0, 0, 0}. 
        /// </summary>
        public static Color Transparent { get; } = new Color(0, 0, 0, 0);
        #endregion


        public static Color Lerp(Color a, Color b, float ratio)
        {
            ratio = ratio.Clamp(0, 1);
            byte f(byte x, byte y) => (byte)(x + (y - x) * ratio);

            return new Color(
                f(a.R, b.R),
                f(a.G, b.G),
                f(a.B, b.B),
                f(a.A, b.A)
            );
        }



        /// <summary>
        /// Gets the Red component of this color. 
        /// </summary>
        public byte R { get; }
        /// <summary>
        /// Gets the Green component of this color. 
        /// </summary>
        public byte G { get; }
        /// <summary>
        /// Gets the Blue component of this color. 
        /// </summary>
        public byte B { get; }
        /// <summary>
        /// Gets the Alpha component of this color. 
        /// </summary>
        public byte A { get; }

        /// <summary>
        /// Creates a new color of the specified RGB values and full alpha. 
        /// </summary>
        /// <param name="r">The red component as a value in the range 0-255.</param>
        /// <param name="g">The blue component as a value in the range 0-255.</param>
        /// <param name="b">The green component as a value in the range 0-255.</param>
        public Color(byte r, byte g, byte b)
            : this(r, g, b, byte.MaxValue) { }

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

        public static Color FromGrayscale(byte value)
            => new Color(value, value, value, 255);

        public static Color FromPacked(int packedValue) => new Color(
            (byte)(packedValue >> 0),
            (byte)(packedValue >> 8),
            (byte)(packedValue >> 16),
            (byte)(packedValue >> 24));


        public int Pack()
            => (R << 0) | (G << 8) | (B << 16) | (A << 24);

        /// <summary>
        /// Mixes another color with this one. 
        /// </summary>
        /// <param name="other">The color to mix this color with.</param>
        /// <param name="ratio">The ratio of the second color. Should be between 0 and 1.</param>
        /// <returns></returns>
        public Color MixWith(Color other, float ratio) 
            => Lerp(this, other, ratio);

        /// <summary>
        /// Returns a new color with the alpha value set to a value from the range 0 to 255. 
        /// </summary>
        /// <param name="newAlpha">The alpha component of the returned color as a value in the range 0-255.</param>
        public Color SetAlpha(byte newAlpha)
            => new Color(R, G, B, newAlpha);

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
                byte f(byte val) => (byte)(val * ratio / 100);

                return new Color(f(R), f(G), f(B), A);
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
                byte f(byte val) => (byte)(val + (255 - val) * perc / 100);

                return new Color(f(R), f(G), f(B), A);
            }
        }

        public static bool operator ==(Color a, Color b) => a.Equals(b);

        public static bool operator !=(Color a, Color b) => !a.Equals(b);

        public override bool Equals(object obj) => (obj is Color c) && Equals(c);

        public override int GetHashCode() => Pack().GetHashCode();

        public bool Equals(Color other)
            => R == other.R
            && G == other.G
            && B == other.B
            && A == other.A;
    }
}
