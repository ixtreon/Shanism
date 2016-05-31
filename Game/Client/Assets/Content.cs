using Shanism.Client.Assets;
using Shanism.Client.Textures;
using Shanism.Common;
using Shanism.Common.Content;
using Shanism.Common.Message.Server;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shanism.Client
{
    /// <summary>
    /// A singleton class that loads the content (mainly textures) needed to play the game. 
    /// There is the default content that is always loaded, 
    /// and the scenario content that is map-specific. 
    /// </summary>
    static class Content
    {
        public const string DefaultContentDirectory = @"Content/";

        public const string ScenarioContentDirectory = @"Scenario/Content";


        /// <summary>
        /// Gets the listing of textures and animations in the game. 
        /// </summary>
        public static ContentList Listing { get; } = new ContentList();

        /// <summary>
        /// Gets the fonts currently defined in the game. 
        /// </summary>
        public static FontCache Fonts { get; } = new FontCache();

        public static TextureCache Textures { get; } = new TextureCache();

        internal static SpriteCache Sprites { get; } = new SpriteCache(Listing);

        public static TerrainCache Terrain { get; } = new TerrainCache();

        public static CircleDict Circles { get; set; }

        public static void LoadDefaultContent(GraphicsDevice graphics, ContentManager content)
        {
            Circles = new CircleDict(graphics, 65600, 0.02);

            var textures = Directory.EnumerateFiles(DefaultContentDirectory, "*.png", SearchOption.AllDirectories)
                .Select(fn => fn
                    .ToLowerInvariant()
                    .GetRelativePath(DefaultContentDirectory))
                .Select(fn => new TextureDef(fn))
                .ToArray();

            // add to content listing
            Listing.Parse(textures);

            //load textures, terrain, fonts
            Textures.LoadContent(content, DefaultContentDirectory, textures);
            Terrain.Reload();
            Fonts.Load(content);

            //load the only (dummy) model for the content list
            Listing.Parse(new[] { AnimationDef.Default });
        }


        public static bool LoadScenarioContent(ContentManager content, HandshakeReplyMessage msg)
        {
            var sc = ScenarioConfig.LoadFromBytes(msg.ScenarioData);
            if (sc == null)
                return false;

            try
            {
                unzipMessage(msg);
                loadScenarioContent(content, sc.Content);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to read the received scenarCommon. The error was: {e.Message}. ");
                return false;
            }

        }

        static void loadScenarioContent(ContentManager content, ContentConfig config)
        {
            //add to content listing
            Listing.Parse(config);

            //load textures, terrain
            content.RootDirectory = ScenarioContentDirectory;
            Textures.LoadContent(content, ScenarioContentDirectory, config.Textures);

            //reload the terrain if it was swapped in. 
            Terrain.Reload();
        }

        static void unzipMessage(HandshakeReplyMessage msg)
        {

            //clear temp
            var TempDir = Path.GetFullPath("temp");
            if (Directory.Exists(TempDir))
                Directory.Delete(TempDir, true);

            //unzip to temp
            ScenarioConfig.UnzipContent(msg.ContentData, TempDir);


            //clear scenario dir
            var scenarioContentDir = Path.GetFullPath(ScenarioContentDirectory); 
            if(Directory.Exists(scenarioContentDir))
                Directory.Delete(scenarioContentDir, true);

            //create sub-directories
            if(Directory.Exists(TempDir))
            {
                foreach (string dirPath in Directory.GetDirectories(TempDir, "*",
                    SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(TempDir, scenarioContentDir));

                //copy all files
                foreach (string filePath in Directory.GetFiles(TempDir, "*.*",
                    SearchOption.AllDirectories))
                    File.Copy(filePath, filePath.Replace(TempDir, scenarioContentDir), true);
            }
        }

    }
}
