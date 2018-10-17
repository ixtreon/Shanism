using IconPacker.GameIcons;
using Ix.Math;
using Shanism.Common.Content;
using System;
using System.IO;
using System.Threading.Tasks;
using static System.Console;

namespace IconPacker
{
    class Program
    {

        const string IconFileName = "icons.png";
        const string LocalCacheFolder = "cache";
        const string TextureSubDirectory = "Textures";

        static readonly Point OutputIconSize = new Point(64);
        static readonly ArchiveParser parser = new ArchiveParser(IconFileName, OutputIconSize);

        static async Task Main(string[] args)
        {
            Client.Common.Json.JsonConfig.Initialize();

            var OutputPath = args.Length < 1 ? "temp" : args[0];
            WriteLine($"Content Builder Started. Output Path: {OutputPath}");

            // get the last file info
            var downloader = new GameIconsClient();
            var remoteFileInfo = await downloader.GetRemoteFileInfo();

            // check cache
            if (IsCacheOK(OutputPath, remoteFileInfo))
            {
                WriteLine($"Found an existing, up-to-date cache. Exiting.. ");
                return;
            }

            WriteLine($"Downloading `{downloader.DownloadUrl}`... ");
            var archiveStream = await downloader.GetIconsAsync();

            WriteLine("Processing the archive... ");
            var (sheet, blob) = parser.ParseArchive(archiveStream);


            WriteLine("Saving the texture...");
            var texDir = Path.Combine(OutputPath, TextureSubDirectory);
            Directory.CreateDirectory(texDir);

            var texPath = Path.Combine(texDir, IconFileName);
            sheet.SaveAsTransparentscale(texPath, 2);

            WriteLine("Creating the scenario...");
            var assembler = CreateDefaultScenario(blob);
            var config = assembler.CreateConfig(OutputPath);
            config.SaveToDisk();


            // update cache
            SaveToCache(OutputPath, remoteFileInfo);
            WriteLine("Done!");
        }

        static bool IsCacheOK(string outputPath, GameIconsInfo remoteInfo)
        {
            var cacheInfoFile = Path.Combine(outputPath, "game-icons.cache");
            if (!File.Exists(cacheInfoFile))
                return false;

            var text = File.ReadAllText(cacheInfoFile);
            if (!DateTime.TryParse(text, out var lastModified))
                return false;

            return lastModified == remoteInfo.LastModified;
        }

        static void SaveToCache(string outputPath, GameIconsInfo remoteInfo)
        {
            Directory.CreateDirectory(outputPath);

            var cacheInfoFile = Path.Combine(outputPath, "game-icons.cache");
            File.WriteAllText(cacheInfoFile, remoteInfo.LastModified.ToString());
        }

        static ScenarioAssembler CreateDefaultScenario(ContentBlob icons) => new ScenarioAssembler
        {
            icons,
            new ContentBlob("terrain.png", new Point(8, 8)),
            new ContentBlob("ui.png", new Point(4, 4), tex => new[]
            {
                tex.CreateStaticAnimation("ui/border", 0, 0),
                tex.CreateStaticAnimation("ui/border-hover", 1, 0),
                tex.CreateStaticAnimation("ui/default", 2, 0),
                tex.CreateStaticAnimation("ui/close", 3, 0),
            }),
            new ContentBlob("dummy.png", new Point(1, 1), tex => new[]
            {
                tex.CreateStaticAnimation("dummy", 0, 0)
            }),
        };
    }
}
