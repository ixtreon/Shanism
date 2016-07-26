using Shanism.Common;
using Shanism.Common.Content;
using Shanism.Common.Util;
using Shanism.Editor.Util;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shanism.Editor.ViewModels
{
    class TexturesViewModel
    {
        readonly ContentConfig Model;

        readonly Dictionary<string, TextureViewModel> textures = new Dictionary<string, TextureViewModel>();

        int _isReloading = 0;

        public string ContentDirectory { get; }


        public event Action TexturesReloaded;

        public IEnumerable<TextureViewModel> Textures => textures.Values;


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
            if (Interlocked.CompareExchange(ref _isReloading, 1, 0) != 0)
                return;

            textures.Clear();

            await Task.Run(() =>
            {
                // get files with nice extensions
                var imgPaths = Directory.EnumerateFiles(Path.GetFullPath(ContentDirectory), "*", SearchOption.AllDirectories)
                    .Where(ImageUtils.IsValidTexture)
                    .Select(f => f.GetRelativePath(ContentDirectory))
                    .ToList();

                //try to load them in-memory as images
                foreach (var relPath in imgPaths)
                {
                    TextureViewModel tex;
                    if (tryLoadTexture(relPath, out tex))
                        textures.Add(relPath, tex);
                }

                //TODO: what about existing definitions which have no corresponding files?
                //currently dumped
            });

            TexturesReloaded?.Invoke();
            _isReloading = 0;
        }

        bool tryLoadTexture(string relPath, out TextureViewModel tex)
        {
            //load the bitmap
            var fullPath = Path.Combine(ContentDirectory, relPath);
            var bmp = loadBitmap(fullPath);
            if (bmp == null)
            {
                tex = null;
                return false;
            }

            //load existing metadata about the texture
            var textureData = Model.Textures.FirstOrDefault(t => t.Name == relPath);
            if (textureData != null)
                tex = new TextureViewModel(ContentDirectory, fullPath, bmp, textureData) { Included = true };
            else
                tex = new TextureViewModel(ContentDirectory, fullPath, bmp, new TextureDef(relPath));

            return true;
        }

        public void Save()
        {
            Model.Textures = Textures
                .Where(t => t.Included)
                .Select(t => t.Data)
                .ToList();
        }


        public void AddTexture(string srcPath, string destFolder)
        {
            if (!File.Exists(srcPath) || !ImageUtils.IsValidTexture(srcPath))
                return;

            copyFile(srcPath, destFolder);
        }

        public void MoveTexture(TextureViewModel tex, string newDir)
        {
            var texName = ShanoPath.GetLastSegment(tex.FullPath);
            var destPath = ShanoPath.Combine(newDir, texName);

            File.Move(tex.FullPath, destPath);

            Model.Textures.Remove(tex.Data);
        }

        static Bitmap loadBitmap(string fullPath)
        {
            try
            {
                using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                    return (Bitmap)Image.FromStream(stream);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        static void copyFile(string fileToCopy, string destinationFolder)
        {
            if (!File.Exists(fileToCopy))
                throw new FileNotFoundException();

            if (!Directory.Exists(destinationFolder))
                throw new DirectoryNotFoundException();

            var fileName = ShanoPath.GetLastSegment(fileToCopy);
            var fn = ShanoPath.Combine(destinationFolder, fileName);

            if (!File.Exists(fn))
                File.Copy(fileToCopy, fn);
        }
    }
}
