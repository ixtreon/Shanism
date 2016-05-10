using Shanism.Common;
using Shanism.Common.Content;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Editor.ViewModels
{
    class TexturesViewModel
    {
        static HashSet<string> supportedExtensions = new HashSet<string>(new[]
        {
            ".JPG", ".JPEG", ".BMP", ".PNG"
        });


        public static bool IsValidTexture(string fileName)
        {
            var targetExtension = Path.GetExtension(fileName)
                .ToUpperInvariant();
            if (string.IsNullOrEmpty(targetExtension))
                return false;

            return supportedExtensions.Contains(targetExtension);
        }



        readonly ContentConfig Model;


        public Dictionary<string, TextureViewModel> Textures { get; } = new Dictionary<string, TextureViewModel>();

        public string ContentDirectory { get; }


        public event Action TexturesReloaded;


        public TexturesViewModel(ContentConfig model, string baseDir)
        {
            Model = model;
            ContentDirectory = Path.Combine(baseDir, Constants.Content.TexturesDirectory);
            if (!Directory.Exists(ContentDirectory))
                Directory.CreateDirectory(ContentDirectory);
        }

        /// <summary>
        /// Reloads all images from the current scenario's base directory and attaches them to a new
        /// or existing <see cref="TextureDef"/> from the given scenario. 
        /// Finally saves these to the <see cref="Textures"/> field and raises the <see cref="TexturesReloaded"/> event. 
        /// </summary>
        public async Task Reload()
        {
            Textures.Clear();

            await Task.Run(() =>
            {
                // get files with nice extensions
                var imgPaths = Directory.EnumerateFiles(ContentDirectory, "*", SearchOption.AllDirectories)
                    .Where(fn => supportedExtensions.Contains(Path.GetExtension(fn).ToUpper()))
                    .ToArray();

                //try to load them in-memory as images
                foreach (var imgPath in imgPaths)
                {
                    Bitmap bmp;
                    try
                    {
                        using (var stream = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
                            bmp = (Bitmap)Image.FromStream(stream);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }

                    //get texturedef (metadata) from scenario, if it exists
                    var relPath = imgPath
                        .GetRelativePath(ContentDirectory);
                    var tex = Model.Textures
                        .FirstOrDefault(t => t.Name == relPath);

                    var wasTexIncluded = (tex != null);
                    tex = tex ?? new TextureDef(relPath);

                    var texModel = new TextureViewModel(ContentDirectory, bmp, tex)
                    {
                        Included = wasTexIncluded,
                    };

                    Textures.Add(relPath, texModel);
                }

                //TODO: what about existing definitions which have no corresponding files?
                //currently dumped
            });

            TexturesReloaded?.Invoke();
        }



        public void Save()
        {
            Model.Textures = Textures
                .Where(t => t.Value.Included)
                .Select(t => t.Value.Data)
                .ToList();
        }


        public void AddTexture(string srcPath, string destFolder)
        {
            if (!File.Exists(srcPath) || !IsValidTexture(srcPath))
                return;

            var fileName = Path.GetFileName(srcPath);
            var destFile = Path.Combine(destFolder, fileName);

            if(!File.Exists(destFile))
                File.Copy(srcPath, destFile);
        }

        public void MoveTexture(TextureViewModel tex, string newDir)
        {
            var texName = Path.GetFileName(tex.FullPath);
            var destPath = Path.Combine(newDir, texName);

            File.Move(tex.FullPath, destPath);

            Model.Textures.Remove(tex.Data);
        }
    }
}
