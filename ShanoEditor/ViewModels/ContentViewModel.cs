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


        public Dictionary<string, TextureViewModel> Textures { get; } = new Dictionary<string, TextureViewModel>();

        public Dictionary<string, ModelDef> Models { get; set; }

        public Dictionary<string, AnimationViewModel> ModelDefaultAnimations { get; set; }

        public async Task Load(CompiledScenario sc)
        {
            //load models
            Models = sc.Content.Models
                .ToDictionary(m => m.Name, m => m);

            //load textures
            await loadTextures(sc);

            loadAnimations();
        }

        /// <summary>
        /// Loads all images from the scenario's base directory and attaches them to a new
        /// or existing <see cref="TextureDef"/> from the given scenario. 
        /// Finally saves these to <see cref="Textures"/>. 
        /// </summary>
        async Task loadTextures(CompiledScenario sc)
        {
            Textures.Clear();
            await Task.Run(() =>
            {
                // get files with nice extensions
                var baseDir = Path.Combine(sc.BaseDirectory, "Content");
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
                    }
                    catch
                    {
                        continue;
                    }

                    //get texturedef (metadata) from scenario, if it exists
                    var relPath = imgPath
                        .GetRelativePath(baseDir);
                    var tex = sc.Content.Textures
                        .FirstOrDefault(t => t.Name == relPath);

                    var wasTexIncluded = (tex != null);
                    tex = tex ?? new TextureDef(relPath);

                    var texModel = new TextureViewModel(imgPath, relPath, bmp, tex)
                    {
                        Included = wasTexIncluded,
                    };

                    Textures.Add(relPath, texModel);
                }

                //TODO: what about existing definitions which have no corresponding files?
            });
        }


        /// <summary>
        /// Loads the default animations for all models into the <see cref="ModelDefaultAnimations"/> dictionary. 
        /// Requires that both models and textures are already loaded. 
        /// </summary>
        void loadAnimations()
        {
            //load default animations
            ModelDefaultAnimations = Models
                .Select(m => new
                {
                    Name = m.Key,
                    Anim = m.Value.Animations.TryGet(Constants.Content.DefaultValues.Animation),
                })
                .Where(o => o.Anim != null)
                .ToDictionary(
                    o => o.Name,
                    o => new AnimationViewModel(this, o.Anim));
        }

        public void Save(CompiledScenario sc)
        {
            sc.Content.Textures = Textures
                .Where(t => t.Value.Included)
                .Select(t => t.Value.Data)
                .ToList();

            sc.Content.Models = Models.Values
                .ToList();

            loadAnimations();
        }
    }
}
