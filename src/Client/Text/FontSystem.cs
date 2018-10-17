using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.IO;
using SixLabors.Fonts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shanism.Client.Text
{

    class FontSystem
    {
        const int UpdateFontsDelayMs = 500;   // delay before we act, after the window is resized


        readonly ScreenSystem screen;
        readonly FontPainter theMaker;

        readonly FontCollection gdxFonts = new FontCollection();
        readonly List<Font> fonts = new List<Font>();

        int timeToUpdateFonts;

        public FontSystem(ScreenSystem screen, GraphicsDevice device)
        {
            this.screen = screen;
            this.theMaker = new FontPainter(device);

            screen.WindowSizeChanged += scheduleFontUpdate;
        }

        void scheduleFontUpdate()
        {
            timeToUpdateFonts = UpdateFontsDelayMs;
        }

        // updates fonts based on window resize
        public void Update(int msElapsed)
        {

            // if no update at all, gg
            if (timeToUpdateFonts <= 0)
                return;

            // if no update now, gg
            timeToUpdateFonts -= msElapsed;
            if (timeToUpdateFonts > 0)
                return;

            // update
            foreach (var font in fonts)
            {
                var pxFont = CreateFont(font.Height, font.Family);
                font.SetPixelFont(pxFont);
            }
        }


        public Font LoadCustomFont(string path, float uiSize)
        {
            var slFontFamily = gdxFonts.Install(path);

            var pxFont = CreateFont(uiSize, slFontFamily);
            var font = new Font(pxFont, slFontFamily, uiSize);

            fonts.Add(font);

            return font;
        }


        public Font LoadSystemFont(string name, float uiSize)
        {
            var slFontFamily = SystemFonts.Find(name);

            var pxFont = CreateFont(uiSize, slFontFamily);
            var font = new Font(pxFont, slFontFamily, uiSize);

            fonts.Add(font);

            return font;
        }

        PixelFont CreateFont(float uiSize, SixLabors.Fonts.FontFamily slFontFamily)
        {
            var lineSpacing = uiSize * screen.UI.Scale;
            var slFont = slFontFamily.CreateFont(lineSpacing);

            var wtf = slFont.LineHeight;

            return theMaker.CreateFont(slFont, lineSpacing);
        }
    }

    public class FontCache
    {
        /// <summary>
        /// A fancy-looking, large font. 
        /// </summary>
        public Font FancyFont { get; }
        /// <summary>
        /// A fancy-looking, extra large font. 
        /// </summary>
        public Font ShanoFont { get; }

        /// <summary>
        /// A small-sized standard font. 
        /// </summary>
        public Font SmallFont { get; }
        /// <summary>
        /// A normal-sized standard font. 
        /// </summary>
        public Font NormalFont { get; }
        /// <summary>
        /// A large-sized standard font. 
        /// </summary>
        public Font LargeFont { get; }

        internal FontCache(FontSystem fonts)
        {
            FancyFont = fonts.LoadCustomFont("Content/Fonts/freud.ttf", 0.07f);
            ShanoFont = fonts.LoadCustomFont("Content/Fonts/freud.ttf", 0.16f);

            SmallFont = fonts.LoadCustomFont("Content/Fonts/FiraSans-Regular.ttf", 0.03f);
            NormalFont = fonts.LoadCustomFont("Content/Fonts/FiraSans-Regular.ttf", 0.05f);
            LargeFont = fonts.LoadCustomFont("Content/Fonts/FiraSans-Regular.ttf", 0.07f);
        }
    }

}
