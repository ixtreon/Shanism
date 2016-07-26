using Shanism.Client.Drawing;
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
using Shanism.Client.Exceptions;
using Shanism.Common.Util;

namespace Shanism.Client
{
    /// <summary>
    /// A list of the loaded game assets: textures, animations, fonts.  
    /// </summary>
    class AssetList
    {
        public const string DefaultContentDirectory = @"Content";

        public const string ScenarioContentDirectory = @"Scenario/Content";


        static readonly AnimationDef[] DefaultAnimations = { AnimationDef.Default };



        readonly Dictionary<string, AnimationDef> animations = new Dictionary<string, AnimationDef>();


        /// <summary>
        /// Gets a list of the currently loaded animations. 
        /// </summary>
        public IReadOnlyDictionary<string, AnimationDef> Animations => animations;

        /// <summary>
        /// Gets the currently loaded fonts. 
        /// </summary>
        public FontCache Fonts { get; } = new FontCache();

        public TextureCache Textures { get; } = new TextureCache();

        public TerrainCache Terrain { get; } = new TerrainCache();

        public CircleDict Circles { get; }


        public AssetList(GraphicsDevice graphics)
        {
            Circles = new CircleDict(graphics, 65600, 8);
        }

        public void LoadDefaultContent(ContentManager content)
        {
            if (!Directory.Exists(DefaultContentDirectory))
                throw new ContentDirectoryMissingException();


            // load all files from content dir as textures
            var defaultTextureList = Directory.EnumerateFiles(DefaultContentDirectory, "*.*", SearchOption.AllDirectories)
                .Select(fn => fn
                    .GetRelativePath(DefaultContentDirectory)
                    .ToLowerInvariant())
                .Select(fn => new TextureDef(fn))
                .ToList();
            Textures.Load(content, DefaultContentDirectory, defaultTextureList);

            //load terrain, fonts
            Terrain.Reload(Textures);
            Fonts.Load(content);

            //load the only (dummy) model in the content list
            foreach (var a in DefaultAnimations)
                animations[ShanoPath.Normalize(a.Name)] = a;
        }


        public bool LoadScenarioContent(ContentManager content, HandshakeReplyMessage msg)
        {
            var sc = ScenarioConfig.LoadFromBytes(msg.ScenarioData);
            if (sc == null)
                Console.WriteLine($"Unable to read the received scenario: invalid data. ");

            try
            {
                unzipMessage(msg);
                loadScenarioContent(content, sc.Content);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to read the received scenario. The error was: {e.Message}. ");
                return false;
            }
        }

        void loadScenarioContent(ContentManager content, ContentConfig config)
        {
            //load textures, terrain
            content.RootDirectory = ScenarioContentDirectory;
            Textures.Load(content, ScenarioContentDirectory, config.Textures);
            Terrain.Reload(Textures);

            //animations
            foreach (var a in config.Animations)
                animations[ShanoPath.Normalize(a.Name)] = a;
        }

        static void unzipMessage(HandshakeReplyMessage msg)
        {
            //clear scenario dir
            var scenarioContentDir = Path.GetFullPath(ScenarioContentDirectory); 
            if(Directory.Exists(scenarioContentDir))
                Directory.Delete(scenarioContentDir, true);

            //unzip to temp dir
            ScenarioConfig.UnzipContent(msg.ContentData, scenarioContentDir);
        }

    }
}
