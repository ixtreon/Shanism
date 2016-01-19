using Client.Assets;
using Client.Textures;
using IO;
using IO.Content;
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
        public const string DefaultTexDir = @"Content\";

        public const string ScenarioTexDir = @"Scenario\";


        /// <summary>
        /// Gets the listing of the
        /// </summary>
        static ContentList ContentList { get; } = new ContentList();


        public static FontCache Fonts { get; } = new FontCache();

        public static TextureCache Textures { get; } = new TextureCache();

        public static SpriteCache Sprites { get; } = new SpriteCache(ContentList);

        public static TerrainCache Terrain { get; } = new TerrainCache();


        public static void LoadDefault(ContentManager content)
        {

            var textures = Directory.EnumerateFiles(DefaultTexDir, "*.png", SearchOption.AllDirectories)
                .Select(fn => fn
                    .ToLowerInvariant()
                    .GetRelativePath(DefaultTexDir))
                .Select(fn => new TextureDef(fn))
                .ToArray();

            // add to content listing
            ContentList.Parse(textures);

            //var effect = content.Load<Microsoft.Xna.Framework.Graphics.Effect>("Shaders\\shader");


            //load textures, terrain, fonts
            Textures.LoadContent(content, DefaultTexDir, textures);
            Terrain.Reload();
            Fonts.Load(content);

            //load the only (dummy) model for the content list
            ContentList.Parse(new[] { ModelDef.Default });
        }


        public static void LoadScenario(ContentManager content, ContentConfig config)
        {
            //add to content listing
            ContentList.Parse(config);

            //load textures, terrain
            Textures.LoadContent(content, ScenarioTexDir, config.Textures);
            Terrain.Reload();
        }

        public static void UnzipContent(byte[] texData, string outputDir, string archivePath = ".", string tempDir = "temp")
        {
            //clear temp
            tempDir = Path.GetFullPath(tempDir);
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, true);

            //unzip to temp
            ScenarioFile.UnzipContent(texData, tempDir);

            var newDir = Path.GetFullPath(outputDir); 

            //delete old scenarios
            if(Directory.Exists(newDir))
                Directory.Delete(newDir, true);

            //create sub-directories
            if(Directory.Exists(tempDir))
            {
                foreach (string dirPath in Directory.GetDirectories(tempDir, "*",
                    SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(tempDir, newDir));

                //copy all files
                foreach (string filePath in Directory.GetFiles(tempDir, "*.*",
                    SearchOption.AllDirectories))
                    File.Copy(filePath, filePath.Replace(tempDir, newDir), true);
            }
        }

    }
}
