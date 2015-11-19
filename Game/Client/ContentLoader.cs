using Client.Assets.Fonts;
using Client.Textures;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScenarioLib;
using System.IO;
using IO.Content;
using Client.Assets;
using Client.Assets.Sprites;
using Client.Assets.Terrain;
using System.Threading;

namespace Client
{
    static class Content
    {
        public const string DefaultTexDir = @"Content\";
        public const string ScenarioTexDir = @"Scenario\";



        static ContentList ContentList { get; } = new ContentList();


        public static FontCache Fonts { get; } = new FontCache();

        public static TextureCache Textures { get; } = new TextureCache();

        public static SpriteCache Sprites { get; } = new SpriteCache(ContentList);

        public static TerrainCache Terrain { get; } = new TerrainCache();


        public static void LoadDefault(ContentManager content)
        {

            var textures = Directory.EnumerateFiles(DefaultTexDir, "*.png", SearchOption.AllDirectories)
                .Select(fn => new TextureDef(fn));

            // add to content listing
            ContentList.Parse(textures);

            //load textures, terrain, fonts
            Textures.LoadContent(content, DefaultTexDir, textures);
            Terrain.Reload();
            Fonts.Load(content);

            //load the only (dummy) model for the content list
            var dummyTex = textures.Single(t => t.Name == @"Content\Objects\dummy.png");
            var dummyModel = new ModelDef(IO.Constants.Content.DefaultModel);
            dummyModel.Animations.Add("stand", new AnimationDef(dummyTex));
            ContentList.Parse(new[] { dummyModel });
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

            var tempPath = Path.Combine(tempDir, archivePath);
            var newPath = Path.GetFullPath(outputDir); 

            //delete old scenarios
            if(Directory.Exists(newPath))
                Directory.Delete(newPath, true);

            //create sub-directories
            if(Directory.Exists(tempPath))
            {
                foreach (string dirPath in Directory.GetDirectories(tempPath, "*",
                    SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(tempPath, newPath));

                //copy all files
                foreach (string filePath in Directory.GetFiles(tempPath, "*.*",
                    SearchOption.AllDirectories))
                    File.Copy(filePath, filePath.Replace(tempPath, newPath), true);
            }
        }

    }
}
