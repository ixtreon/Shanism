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
using ShanoEditor.ViewModels;

namespace ShanoEditor.ScenarioViews
{
    partial class TexturesView : ScenarioView
    {

        public override ScenarioViewType ViewType { get; } = ScenarioViewType.Textures;

        TextureViewModel CurrentTexture { get; set; }


        public IEnumerable<TextureViewModel> AllTextures
        {
            get { return Model.Content.Textures.Values; }
        }

        public event Action TexturesLoaded;

        public TexturesView()
        {
            InitializeComponent();
            texTree.ImageList = SingletonImageList.FolderImageList;
        }

        protected override async Task LoadModel()
        {
            RefreshTree();
        }

        protected override void SaveModel()
        {
            Model.Save();
        }

        void RefreshTree()
        {
            texTree.Nodes.Clear();
            var rootNode = texTree.Nodes.Add("Textures");

            //update the visible list
            foreach (var tex in Model.Content.Textures.Values)
            {
                //split path into segments
                var texturePathSegments = tex.Path
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
                    Name = tex.Path,
                    Checked = tex.Included,
                    ImageIndex = 1,
                    SelectedImageIndex = 1,
                    Tag = tex
                });
            }

            rootNode.ExpandAll();

        }

        void updateUi(TextureViewModel texData)
        {
            pDetailSplitter.Visible = (texData != null);

            //update the right panel
            if (texData != null)
            {
                CurrentTexture = texData;

                texView.SetTexture(CurrentTexture);
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
            var texData = n.Tag as TextureViewModel;

            if (texData == null)   // a folder, recurse
            {
                selectAll(n, n.Checked);
                return;
            }

            // save to the scenario
            if (!Loading)
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

            var texData = e.Node.Tag as TextureViewModel;
            updateUi(texData);
        }
    }
}
