using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shanism.Editor.ViewModels;
using Shanism.Editor.Views.Textures;
using Shanism.Common.Game;
using System.Collections;
using System.Diagnostics;
using System.IO;

namespace Shanism.Editor.Views.Content
{
    partial class TextureTree : UserControl
    {
        //first folders then files, alphabetical order
        static readonly IComparer nodeComparer = new GenericComparer<TreeNode>((a, b) =>
        {
            var cmpImg = a.ImageIndex.CompareTo(b.ImageIndex);
            if (cmpImg != 0)
                return cmpImg;

            return string.Compare(a.Name, b.Name, StringComparison.Ordinal);
        });

        readonly TextureNodeBuilder texBuilder = new TextureNodeBuilder();

        TexturesViewModel Model;


        public event Action<TextureNode> TextureChanged;

        public event Action<TextureViewModel[]> SelectionChanged;
        public event Action<ShanoTreeNode> SelectedNodeChanged;

        public void SetModel(TexturesViewModel model)
        {
            Model = model;

            texBuilder.Model = model;
            texBuilder.RefreshNodes(treeView.Nodes, null);
            treeView.Sort();
        }

        public TextureTree()
        {
            InitializeComponent();

            treeView.PathSeparator = "/";

            treeView.TreeViewNodeSorter = nodeComparer;
            treeView.CheckBoxes = true;
            treeView.ImageList = ImageLists.DefaultList;
            treeView.AfterSelect += treeView_AfterSelect;
            treeView.AfterCheck += treeView_AfterCheck;
            treeView.AfterLabelEdit += treeView_AfterLabelEdit;
            treeView.KeyDown += treeView_KeyDown;
            treeView.NodeMouseClick += treeView_NodeMouseClick;
        }

        void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //show the context menu
            if (e.Button == MouseButtons.Right)
            {
                treeView.SelectedNode = e.Node;
                menuStrip.Show(Cursor.Position);
            }
        }

        void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node is TextureNode)
            {
                var n = (TextureNode)e.Node;
                var ts = new[] { n.Texture };

                SelectedNodeChanged?.Invoke(n);
                SelectionChanged?.Invoke(ts);
            }
            else if (e.Node is TextureFolderNode)
            {
                var n = (TextureFolderNode)e.Node;
                var ts = n.GetNodes<TextureNode>()
                    .Select(tn => tn.Texture)
                    .ToArray();

                SelectedNodeChanged?.Invoke(n);
                SelectionChanged?.Invoke(ts);
            }
        }

        void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (treeView.SelectedNode == null)
                return;

            if (e.KeyCode == Keys.F2)
            {
                treeView.SelectedNode.BeginEdit();
            }
        }

        void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown) //do not recurse.. when?
                return;

            var n = e.Node as ShanoTreeNode;
            if (n == null)
                return;

            n.SetChecked(n.Checked);
            n.Refresh();

            TextureChanged?.Invoke(null);
        }

        async void btnReload_Click(object sender, EventArgs e)
        {
            await reloadModel();
        }

        #region Rename/Open/New/Delete

        void btnOpen_Click(object sender, EventArgs e)
        {
            var tn = treeView.SelectedNode;

            if (tn is TextureFolderNode)
                Process.Start(((TextureFolderNode)tn).FolderPath);
            else if(tn is TextureNode)
                Process.Start(Path.GetDirectoryName(((TextureNode)tn).Texture.FullPath));
        }

        void btnDelete_Click(object sender, EventArgs e)
        {

        }

        void btnRename_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode is ShanoTreeNode)
                treeView.SelectedNode.BeginEdit();
        }


        void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            var oldNode = e.Node as ShanoTreeNode;
            if (oldNode == null || !oldNode.TryRename(e.Label))
            {
                e.CancelEdit = true;
                return;
            }

            TextureChanged?.Invoke(null);
            selectNode(oldNode.Name);
        }

        #endregion

        async Task reloadModel()
        {
            treeView.Enabled = false;
            await Model.Reload();
            texBuilder.RefreshNodes(treeView.Nodes, null);
            treeView.Enabled = true;
        }

        void selectNode(string name)
        {
            treeView.SelectedNode = treeView.Nodes
                .Find(name, true)
                .FirstOrDefault();
        }
    }
}
