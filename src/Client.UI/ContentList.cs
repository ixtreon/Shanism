using Shanism.Common.Content;
using Shanism.Common.Message.Server;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shanism.Common.Util;
using Shanism.Client.Assets;

namespace Shanism.Client
{
    /// <summary>
    /// A list of the loaded game assets: textures, animations, fonts.  
    /// </summary>
    public class ContentList
    {
        public const string DefaultContentFile = @"scenario.json";

        public const string ScenarioDir = @"Scenario/Content";


        readonly Dictionary<string, AnimationDef> animations = new Dictionary<string, AnimationDef>();

        public AnimationDef DefaultAnimation { get; private set; }

        public TextureCache Textures { get; }

        /// <summary>
        /// Gets a list of the currently loaded animations. 
        /// </summary>
        public IReadOnlyDictionary<string, AnimationDef> Animations => animations;

        public ContentManager ContentManager { get; }

        /// <summary>
        /// Gets the currently loaded fonts. 
        /// </summary>
        public FontCache Fonts { get; private set; }

        public UiCache UI { get; private set; }

        public IconCache Icons { get; private set; }

        public CircleDict Circles { get; }

        public ShaderContainer Shaders { get; private set; }


        public ContentList(GraphicsDevice graphics, ContentManager contentManager)
        {
            ContentManager = contentManager;

            Circles = new CircleDict(graphics, 65600, 8);
            Textures = new TextureCache(graphics);
        }

        public bool LoadDefault()
        {
            Shaders = new ShaderContainer(ContentManager);

            string errors;
            var sc = ScenarioConfig.LoadFromDisk(".", out errors);
            if (sc == null)
            {
                Console.WriteLine($"Unable to load the default content: {errors}");
                return false;
            }

            loadConfig(sc.Content, "Textures");

            DefaultAnimation = animations["dummy"];

            return true;
        }


        public bool LoadScenario(HandshakeReplyMessage msg)
        {
            var sc = ScenarioConfig.LoadFromBytes(msg.ScenarioData);
            if (sc == null)
                Console.WriteLine($"Unable to read the received scenario: invalid data. ");

            try
            {
                unzipMessage(msg, ScenarioDir);
                loadConfig(sc.Content, ScenarioDir);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to read the received scenario. The error was: {e.Message}. ");
                return false;
            }
        }

        void loadConfig(ContentConfig config, string textureDir)
        {
            //load textures
            ContentManager.RootDirectory = textureDir;
            Textures.Load(textureDir, config.Textures);

            //load animations
            foreach (var a in config.Animations)
                animations[ShanoPath.Normalize(a.Name)] = a;

            //reload fonts, icons, ui elems
            Fonts = new FontCache(ContentManager);
            UI = new UiCache(this);
            Icons = new IconCache(this);
        }

        static void unzipMessage(HandshakeReplyMessage msg, string outDir)
        {
            outDir = Path.GetFullPath(outDir);

            //clear scenario dir
            if (Directory.Exists(outDir))
                Directory.Delete(outDir, true);

            //unzip to temp dir
            Directory.CreateDirectory(outDir);
            ScenarioConfig.UnzipContent(msg.ContentData, outDir);
        }

    }
}
