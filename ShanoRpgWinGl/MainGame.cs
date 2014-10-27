#region Using Statements
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IO;
using ShanoRpgWinGl.UI;
using System.Collections.Generic;
using ShanoRpgWinGl.Objects;
using ShanoRpgWinGl.Sprites;
using ShanoRpgWinGl.Properties;
using MapTile = IO.Common.MapTile;
using MovementState = IO.Common.MovementState;
#endregion

namespace ShanoRpgWinGl
{
    /// <summary>
    /// This is the main type for our game
    /// </summary>
    public class MainGame : Game
    {
        public IServer Server;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        MapTile[,] mapTiles = new MapTile[Constants.Game.ScreenWidth + 5, Constants.Game.ScreenHeight + 5];

        /// <summary>
        /// The main UI window. 
        /// </summary>
        UiManager mainInterface;

        /// <summary>
        /// Gets the local hero. 
        /// </summary>
        readonly IHero localHero;

        /// <summary>
        /// All entities in our proximity. 
        /// </summary>
        IEnumerable<IUnit> entities;

        public MainGame(IHero h)
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.localHero = h;
        }

        //hack so we can start the game without referencing monogame. duh
        public bool Running
        {
            set
            {
                if(value)
                    Run();
            }
        }

        /// <summary>
        /// Run at the beginning. 
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            mainInterface = new UiManager(localHero, Server);

            Screen.CenterPoint = new Vector2((float)localHero.Location.X, (float)localHero.Location.Y);
            Screen.ScreenSize = new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            this.IsMouseVisible = true;
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += Window_ClientSizeChanged;
            Window_ClientSizeChanged(null, null);
        }



        Rectangle lastWindowBounds;
        private ObjectManager ObjectManager;

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            var s = this.Window.ClientBounds;
            if (s != lastWindowBounds)
            {
                lastWindowBounds = s;
                graphics.PreferredBackBufferWidth = s.Width;
                graphics.PreferredBackBufferHeight = s.Height;
                graphics.ApplyChanges();
            }
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            //var heroTexture = Content.Load<Texture2D>("hero.bmp");

            TextureCache.LoadContent(Content);
            SpriteCache.Load();

            this.ObjectManager = new ObjectManager(localHero);
            ObjectManager.UnitClicked += (u) =>
            {
                mainInterface.Target = u;
            };

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            // TODO: Add your update logic here

            var msElapsed = (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            //check local hero keys
            UpdateKeys();

            //update the object manager
            var objs = Server.GetGameObjects();
            ObjectManager.Update(msElapsed, objs);

            //update UI
            mainInterface.Update(msElapsed);

            base.Update(gameTime);
        }
        public void UpdateKeys()
        {
            //converts a bool to an int
            Func<bool, int> b2i = (b) => (b ? 1 : 0);

            //keyboard handlers
            var kbd = Keyboard.GetState();

            var dx = b2i(kbd.IsKeyDown(Keys.D)) - b2i(kbd.IsKeyDown(Keys.A));
            var dy = b2i(kbd.IsKeyDown(Keys.S)) - b2i(kbd.IsKeyDown(Keys.W));
            Server.MovementState = new MovementState()
            {
                XDirection = dx,
                YDirection = dy
            };

            Settings.Default.AlwaysShowHealthBars = kbd.GetPressedKeys().Contains(Keys.C);

            //terrain
            double x, y;
            Server.GetNearbyTiles(ref mapTiles, out x, out y);


            // overrides the hero position to the one we've just received. 
            // this is a problem in local games since the ObjectManager
            // queries a unit's more up-to-date position than in the update. 
            ObjectManager.LocalHero.CustomLocation = new Vector2((float)x, (float)y);

            //update cameraInfo
            Screen.CenterPoint = new Vector2((float)x, (float)y);
            Screen.ScreenSize = new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (Server != null)
            {
                //start drawing
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone);

                //terrain
                drawTerrain();

                //hero n units
                //doodads and effects?
                ObjectManager.Draw(spriteBatch);


                //interface
                mainInterface.Draw(spriteBatch);

                //debug stuff
                var mp = Screen.ScreenToUi(Mouse.GetState().Position);
                var sFps =
                    "FPS: " + (1000 / gameTime.ElapsedGameTime.TotalMilliseconds).ToString("00");
                var sUiCoord = string.Format(
                    "UI: {0} {1}", mp.X.ToString("0.00"), mp.Y.ToString("0.00"));
                var sGameCoord = string.Format(
                    "Game: {0} {1}", localHero.Location.X.ToString("0.00"), localHero.Location.Y.ToString("0.00"));

                TextureCache.MainFont.DrawString(spriteBatch, sFps, Color.Goldenrod, new Point(24, 18));
                TextureCache.MainFont.DrawString(spriteBatch, sGameCoord, Color.Black, new Point(24, 2 * 24));
                TextureCache.MainFont.DrawString(spriteBatch, sUiCoord, Color.Black, new Point(24, 3 * 24));

                //end drawing
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }


        private void drawTerrain()
        {
            int xRange = Constants.Game.ScreenWidth / 2,
                yRange = Constants.Game.ScreenHeight / 2;

            int xHero = (int)Math.Floor(Screen.CenterPoint.X),
                yHero = (int)Math.Floor(Screen.CenterPoint.Y);

            //draw the terrain
            for (int ix = -xRange; ix <= xRange; ix++)
                for (int iy = -yRange; iy <= yRange; iy++)
                {
                    var tileX = xHero + ix;
                    var tileY = yHero + iy;

                    var mapTile = mapTiles[ix + xRange, iy + yRange];

                    Sprite tileTexture = SpriteCache.Terrain.GetSprite(mapTile);

                    tileTexture.DrawInGame(spriteBatch, new Vector2(tileX, tileY), new Vector2(1, 1));
                }
        }
    }
}
