using Ix.Math;
using Microsoft.Xna.Framework.Input;
using Shanism.Client.IO;
using Shanism.Client.UI;
using Shanism.Client.UI.Containers;
using Shanism.Client.Views;
using Shanism.Common;
using System;
using System.Linq;
using System.Numerics;

namespace Shanism.Client.Test
{
    class TestsGame : ShanismClient
    {
        View view;
        
        GlowTimer maxStringWidth = new GlowTimer(3456);
        GlowTimer scissors = new GlowTimer(2100);

        Font dynamicFont;

        protected override void OnGameLoaded()
        {
            var fonts = DefaultContent.Fonts;

            dynamicFont = CreateFont(0.1f);

            ColorScheme.Current = ColorScheme.Dark;

            IsMouseVisible = true;

            Views.SetMain(view = new TitleView());

            Window window;
            ListPanel listPanel;

            view.Add(window = new Window
            {
                Location = new Vector2(0.1f),
                Size = new Vector2(1),
            });

            window.Add(listPanel = new ListPanel(Direction.TopDown)
            {
                Bounds = window.ClientBounds,
                ParentAnchor = AnchorMode.All,
            });
            listPanel.Add(new ValueLabel
            {
                Text = "some text",
                Value = "value",

                BackColor = Color.SkyBlue,
                Font = fonts.NormalFont,
                ValueFont = fonts.NormalFont,
            });

            listPanel.Add(new ValueLabel
            {
                Text = "some text",
                Value = "value",
                Font = fonts.NormalFont,

                BackColor = Color.DarkRed,
            });

            listPanel.Add(new TextLabel
            {
                Text = "text label here",
                Font = fonts.NormalFont,

                ControlFont = fonts.LargeFont,
            });

            listPanel.Add(new SplitPanel(Axis.Horizontal)
            {
                First = new Control { BackColor = Color.Green },
                Second = new Control { BackColor = Color.Red },
                
                Width = 0.5f,
                Height = 0.5f,
            });

            window.Show();
        }

        Font CreateFont(float uiSize)
        {
            throw new NotImplementedException();

            //var pxSize = (uiSize * Screen.UI.Scale);
            //var fontPainter = new FontPainter(GraphicsDevice);
            //var family = fontPainter.CreateFontFamily("Arial", pxSize);

            //return new Font(Screen, family, uiSize);
        }

        protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //base.Draw(gameTime);
            //return;

            using (var c = starter.BeginUI())
            {

                if (Keyboard.JustPressed.Contains(new Keybind(Keys.F1)))
                {
                    dynamicFont = CreateFont(0.1f);
                }

                c.Clear(Color.Black);

                if (dynamicFont != null)
                {
                    dynamicFont.GetWidth(' ');
                    starter.SpriteBatch.ShanoDraw(dynamicFont.Texture, 
                        dynamicFont.Texture.Bounds.ToRect(), 
                        new RectangleF(0, 0, 
                            dynamicFont.Texture.Width / Screen.UI.Scale,
                            dynamicFont.Texture.Height / Screen.UI.Scale), Color.White);
                }



                //var canvasWidth = (float)scissors.GetValue(0.2f, 1.5f);
                var canvasWidth = 1.5f;
                var textMaxWidth = (float)maxStringWidth.GetValue(0f, 0.8f);

                //font = Content.Fonts.NormalFont;
                var textPosition = new Vector2(0.5f);
                //var text = "123";

                var text = $@"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. 
                    
Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. 


{"\t"}Duis aute irure dolor in reprehenderit in 
voluptate velit esse cillum dolore eu fugiat nulla pariatur. 
Excepteur sint occaecat cupidatat non proident, 
sunt in culpa qui officia deserunt mollit anim id est laborum.";


                c.FillRectangle(new RectangleF(canvasWidth, 0, Screen.UiSize.X - canvasWidth, Screen.UiSize.Y),
                    Color.White.SetAlpha(10));

                var textAlign = AnchorPoint.TopCenter;
                c.Draw(starter.BlankTexture,
                    new RectangleF(Vector2.Zero, new Vector2(textMaxWidth + textPosition.X, 2)),
                    Color.Red.SetAlpha(50));


                c.DrawString(dynamicFont, text, textPosition,
                        Color.SkyBlue, textMaxWidth, AnchorPoint.TopLeft);
            }
        }
    }
}
