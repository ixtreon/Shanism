using IO.Content;
using ScenarioLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;

namespace ShanoEditor.ViewModels
{
    /// <summary>
    /// A ViewModel for all the content in a loaded scenario. 
    /// </summary>
    class ContentViewModel
    {
        static HashSet<string> supportedExtensions = new HashSet<string>(new[]
        {
            ".JPG", ".JPEG", ".BMP", ".PNG"
        });


        public Dictionary<string, TextureViewModel> Textures { get; } = new Dictionary<string,TextureViewModel>();

        public Dictionary<string, ModelDef> Models { get; set; }


        public async Task Load(CompiledScenario sc)
        {
            //load models
            Models = sc.Content.Models
                .ToDictionary(m => m.Name, m => m);

            //load textures
            await loadTextures(sc);
        }

        async Task loadTextures(CompiledScenario sc)
        { 
            Textures.Clear();
            await Task.Run(() =>
            {
                // get files with nice extensions
                var imgPaths = Directory.EnumerateFiles(sc.BaseDirectory, "*", SearchOption.AllDirectories)
                .Where(fn => supportedExtensions.Contains(Path.GetExtension(fn).ToUpper()))
                .ToArray();

                //try to load them in-memory as images
                foreach (var imgPath in imgPaths)
                {
                    Bitmap bmp;
                    try
                    {
                        bmp = (Bitmap)Image.FromFile(imgPath);
                        var relPath = imgPath.GetRelativePath(sc.BaseDirectory);
                        var tex = new TextureViewModel
                        {
                            FullPath = imgPath,
                            Path = relPath,
                            Image = bmp,
                            Data = sc.Content.Textures.FirstOrDefault(t => t.Name == relPath),
                        };

                        Textures.Add(relPath, tex);
                    }
                    catch { }
                }

                //TODO: what about existing definitions which have no corresponding files?
            });
        }

        public void Save(CompiledScenario sc)
        {
            sc.Content.Textures = Textures
                .Select(t => t.Value.Data)
                .ToList();

            sc.Content.Models = Models.Values
                .ToList();
        }
    }
}
