using Client.Assets;
using Client.Textures;
using IO;
using IO.Content;
using IO.Message.Server;
using Microsoft.Xna.Framework.Content;
using ScenarioLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Client
{
    /// <summary>
    /// A singleton class that loads the content (mainly textures) needed to play the game. 
    /// There is the default content that is always loaded, 
    /// and the scenario content that is map-specific. 
    /// </summary>
    static class Content
    {
        public const string DefaultContentDirectory = @"Content\";

        public const string ScenarioContentDirectory = @"Scenario\";


        /// <summary>
        /// Gets the listing of the
        /// </summary>
        static ContentList ContentList { get; } = new ContentList();


        public static FontCache Fonts { get; } = new FontCache();

        public static TextureCache Textures { get; } = new TextureCache();

        public static SpriteCache Sprites { get; } = new SpriteCache(ContentList);

        public static TerrainCache Terrain { get; } = new TerrainCache();


        public static void LoadDefaultContent(ContentManager content)
        {

            var textures = Directory.EnumerateFiles(DefaultContentDirectory, "*.png", SearchOption.AllDirectories)
                .Select(fn => fn
                    .ToLowerInvariant()
                    .GetRelativePath(DefaultContentDirectory))
                .Select(fn => new TextureDef(fn))
                .ToArray();

            // add to content listing
            ContentList.Parse(textures);

            //var effect = content.Load<Microsoft.Xna.Framework.Graphics.Effect>("Shaders\\shader");


            //load textures, terrain, fonts
            Textures.LoadContent(content, DefaultContentDirectory, textures);
            Terrain.Reload();
            Fonts.Load(content);

            //load the only (dummy) model for the content list
            ContentList.Parse(new[] { ModelDef.Default });
        }


        public static void LoadScenarioContent(ContentManager content, HandshakeReplyMessage msg)
        {
            var sc = ScenarioFile.LoadBytes(msg.ScenarioData);

            unzipMessage(msg);


            loadScenarioContent(content, sc.Content);
        }

        static void loadScenarioContent(ContentManager content, ContentConfig config)
        {
            //add to content listing
            ContentList.Parse(config);

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
            ScenarioFile.UnzipContent(msg.ContentData, TempDir);


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
