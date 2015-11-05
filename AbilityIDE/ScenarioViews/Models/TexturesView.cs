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

        TextureModelView CurrentTexture { get; set; }

        public readonly Dictionary<string, TextureModelView> AllTextures = new Dictionary<string, TextureModelView>();

        public TexturesView()
        {
            InitializeComponent();
            texTree.ImageList = SingletonImageList.FolderImageList;
        }

        protected override async Task LoadScenario()
        {
            await LoadTextures();

            await RefreshTree();
        }

        public override void SaveScenario()
        {
            Scenario.ModelConfig.Textures = AllTextures.Values
                .Where(t => t.Included)
                .Select(t => t.Data)
                .ToList();
        }

        async Task RefreshTree()
        {
            texTree.Nodes.Clear();
            var rootNode = texTree.Nodes.Add("Textures");

            //update the visible list
            foreach (var kvp in AllTextures)
            {
                //split path into segments
                var texturePathSegments = kvp.Value.Path
                    .Split(Path.DirectorySeparatorChar);


                //find or create the folder structure
                var folderNode = rootNode;
                foreach (var pathSegment in texturePathSegments.DropLast())
                {
                    var segmentNode = folderNode.Nodes.Find(pathSegment, false)
                        .SingleOrDefault();
                    if (segmentNode == null)
                        folderNode.Nodes.Add(segmentNode = new TreeNode
                        {   //add new folder
                            Text = pathSegment,
                            Name = pathSegment,
                            ImageIndex = 0,
                            SelectedImageIndex = 0,
                        });
                    folderNode = segmentNode;
                }

                //create the texture node
                var nodeText = texturePathSegments.Last();
                folderNode.Nodes.Add(new TreeNode
                {
                    Text = nodeText,
                    Name = kvp.Value.Path,
                    Checked = kvp.Value.Included,
                    ImageIndex = 1,
                    SelectedImageIndex = 1,
                    Tag = kvp.Value
                });
            }

            rootNode.ExpandAll();

        }

        async Task LoadTextures()
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
                        AllTextures.Add(relPath, new TextureModelView
                        {
                            FullPath = imgPath,
                            Path = relPath,
                            Image = bmp,
                            Data = Scenario.ModelConfig.Textures.FirstOrDefault(t => t.Name == relPath),
                        });
                    }
                    catch { }
                }
            });
        }

        void updateUi(TextureModelView texData)
        {
            pDetailSplitter.Visible = (texData != null);

            //update the right panel
            if (texData != null)
            {
                CurrentTexture = AllTextures[texData.Path];

                texView.SetModel(CurrentTexture);
                pTexProps.SelectedObject = CurrentTexture;
            }
        }

        private void pTexProps_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var it = e.ChangedItem;

            texView.Invalidate();
            MarkAsChanged();
        }

        private void texTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            var n = e.Node;
            var texData = n.Tag as TextureModelView;

            if (texData == null)   // a folder, recurse
            {
                selectAll(n, n.Checked);
                return;
            }

            // save to the scenario
            if (!_loadingScenario)
            {
                texData.Included = n.Checked;
                MarkAsChanged();
            }

            if (n.IsSelected)
                updateUi(texData);
        }

        void selectAll(TreeNode n, bool selected)
        {
            foreach(TreeNode cn in n.Nodes)
            {
                cn.Checked = selected;
                selectAll(cn, selected);
            }
        }

        private void texTree_AfterSelect(object sender, TreeViewEventArgs e)
        {

            var texData = e.Node.Tag as TextureModelView;
            updateUi(texData);
        }
    }
}
