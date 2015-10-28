using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using IO;
using IO.Content;

namespace AbilityIDE.ScenarioViews
{
    public partial class TexturesView : ScenarioView
    {
        static HashSet<string> supportedExtensions = new HashSet<string>(new[]
        {
            ".JPG", ".JPEG", ".BMP", ".PNG"
        });

        public override ScenarioViewType ViewType { get; } = ScenarioViewType.Textures;

        TextureView CurrentTexture { get; set; }

        public readonly Dictionary<string, TextureView> AllTextures = new Dictionary<string, TextureView>();

        public TexturesView()
        {
            InitializeComponent();
        }

        protected override async Task LoadScenario()
        {
            lTextures.Items.Clear();

            await RefreshList();

            //update the visible list
            foreach (var kvp in AllTextures)
                lTextures.Items.Add(kvp.Value.Path, kvp.Value.Included);
        }

        public override void SaveScenario()
        {
            Scenario.ModelConfig.Textures = new HashSet<TextureDef>(AllTextures.Values
                .Where(t => t.Included)
                .Select(t => t.Data));
        }

        public async Task RefreshList()
        {
            AllTextures.Clear();
            if (Scenario == null)
                return;

            await Task.Run(() =>
            {
                //get files with nice extensions
                var imgPaths = Directory.EnumerateFiles(Scenario.BaseDirectory, "*", SearchOption.AllDirectories)
                .Where(fn => supportedExtensions.Contains(Path.GetExtension(fn).ToUpper()))
                .ToArray();

                //try to load them in-memory as images
                foreach (var imgPath in imgPaths)
                {
                    Bitmap bmp;
                    try
                    {
                        bmp = (Bitmap)Image.FromFile(imgPath);
                        var relPath = imgPath.GetRelativePath(Scenario.BaseDirectory);
                        AllTextures.Add(relPath, new TextureView
                        {
                            FullPath = imgPath,
                            Path = relPath,
                            Image = bmp,
                            Data = Scenario.ModelConfig.Textures.FirstOrDefault(t => t.Name == imgPath),
                        });
                    }
                    catch { }
                }
            });
        }

        private void lTextures_SelectedIndexChanged(object sender, EventArgs e)
        {
            var texId = lTextures.SelectedItem as string;
            if (string.IsNullOrEmpty(texId))
                return;

            //get and show the texture
            CurrentTexture = AllTextures[texId];
            pTexPreview.Image = CurrentTexture.Image;

            // select the object
            pTexProps.SelectedObject = CurrentTexture;
        }

        private void lTextures_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_loadingScenario)
                return;

            var texId = lTextures.Items[e.Index] as string;

            if (string.IsNullOrEmpty(texId))
                return;

            var texData = AllTextures[texId];

            texData.Included = !texData.Included;
            MarkAsChanged();
        }

        private void pTexProps_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var it = e.ChangedItem;

            pTexPreview.Invalidate();

            MarkAsChanged();
        }

        private void pTexPreview_Paint(object sender, PaintEventArgs e)
        {
            if (pTexPreview.Image == null || CurrentTexture == null || CurrentTexture.Included == false)
                return;

            var img = pTexPreview.Image;
            var imgRatio = (float)img.Width / img.Height;

            var imgW = Math.Min(pTexPreview.Width, pTexPreview.Height * imgRatio);
            var imgH = imgW / imgRatio;

            var g = e.Graphics;
            var splitsX = CurrentTexture.LogicalWidth ?? 1;
            var splitsY = CurrentTexture.LogicalHeight ?? 1;

            var left = (pTexPreview.Width - imgW) / 2;
            var top = (pTexPreview.Height - imgH) / 2;

            using (var p = new Pen(Color.Yellow, 2))
            {
                foreach (var ix in Enumerable.Range(0, splitsX + 1))
                {
                    var x = (float)(left + imgW * ix / splitsX);
                    g.DrawLine(p, x, top, x, top + imgH);
                }

                foreach (var iy in Enumerable.Range(0, splitsY + 1))
                {
                    var y = (float)(top + imgH * iy / splitsY);
                    g.DrawLine(p, left, y, left + imgW, y);
                }
            }
        }
    }
}
