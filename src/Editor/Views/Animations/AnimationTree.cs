using Shanism.Common.Util;
using Shanism.Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shanism.Editor.Views.Content
{
    class AnimationTree : TreeView
    {

        TreeNode Root;

        public event Action ModelChanged;

        public event Action<AnimationViewModel> AnimationSelected;

        AnimationsViewModel animations;

        public bool ReadOnly { get; set; }

        public AnimationTree()
        {
            AllowDrop = true;
            LabelEdit = true;
            PathSeparator = "/";
            HideSelection = false;
            ImageList = ImageLists.AnimationList;

            AfterSelect += (o, e)
                => updateSelection();
            AfterLabelEdit += (o, e)
                => e.CancelEdit = finishRename(e.Node, e.Label);

            KeyDown += (o, e) => handleKeyPress(e.KeyCode);

            DragOver += (o, e) => selectHoverNode();
            ItemDrag += onItemDrag;
            DragEnter += onDragEnter;
            DragDrop += onDragDrop;
        }

        void onItemDrag(object sender, ItemDragEventArgs e)
        {
            if (!ReadOnly)
                DoDragDrop(e.Item, DragDropEffects.Move);
        }

        void onDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                var srcNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
                var srcName = srcNode.Name;
                var srcFolder = srcNode.Parent.Name;

                var destNode = SelectedNode;
                var destFolder = destNode.Name;

                //check dest is not subfolder of src
                if (ShanoPath.IsSubFolderOf(destFolder, srcName))
                {
                    if (destFolder != srcName)
                        MessageBox.Show("The destination folder is a sub-folder of the source folder!", "Animation Move", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                var destPath = SelectedNode.Name;
                var incomingAnims = enumNodes(srcNode)
                    .Where(n => n.Tag is AnimationViewModel);

                foreach (var an in incomingAnims)
                {
                    var relName = ShanoPath.GetRelativePath(an.Name, srcFolder);
                    var newName = ShanoPath.Combine(destPath, relName);

                    renameAnimation(an, (AnimationViewModel)an.Tag, newName);
                }
            }
        }

        void onDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = e.AllowedEffect & DragDropEffects.Move;
        }

        void selectHoverNode()
        {
            SelectedNode = GetNodeAt(PointToClient(Cursor.Position)) 
                ?? SelectedNode;
        }
        public void Load(AnimationsViewModel model)
        {
            animations = model;

            Nodes.Clear();
            Root = new TreeNode { Text = "Animations", Name = "" };
            Nodes.Add(Root);
            TopNode = Root;

            foreach (var anim in model.Values)
                addNode(anim);
        }

        public void AddNewAnimation(string animName, bool renameAfterwards)
        {
            if (animations.ContainsKey(animName))
                throw new InvalidOperationException("Animation with such name already exists!");

            //modify the model
            var anim = animations.Create(animName);

            //modify the view
            var animNode = addNode(anim);

            if (renameAfterwards)
                startRename(animNode);
        }
        public void RenameSelectedNode() => startRename(SelectedNode);


        public void RemoveSelectedNode()
            => RemoveNode(SelectedNode);

        public void RemoveNode(TreeNode n)
        {
            if (n == null)
                return;

            //fix the model
            var toRemove = enumAnimations(n);
            foreach (var a in toRemove)
                if (!animations.Remove(a.Name))
                    throw new Exception();

            //fix the view
            n.Remove();

            AnimationSelected?.Invoke(null);
            ModelChanged?.Invoke();
        }

        /// <summary>
        /// Reflects any edits in the currently selected node. 
        /// </summary>
        public void RefreshSelectedNode() => refreshNode(SelectedNode);

        #region Private Methods

        void refreshNode(TreeNode n)
        {
            var anim = n?.Tag as AnimationViewModel;
            if (anim == null)
                return;

            //update changes in name
            if (n.Name != anim.Name)
            {
                var newNode = addNode(anim);
                SelectedNode = newNode;
                if (n != newNode)
                    removeAnimNode(n);
            }
        }

        void updateSelection()
        {
            var isRoot = SelectedNode == Root;
            var isAnim = SelectedNode != null && SelectedNode.Tag is AnimationViewModel;

            //if (SelectedNode.Nodes.Count > 0)
            //    SelectedNode.Expand();

            AnimationSelected?.Invoke((AnimationViewModel)SelectedNode.Tag);
        }

        void handleKeyPress(Keys k)
        {
            if (ReadOnly)
                return;

            var n = SelectedNode;
            var isAnim = n?.Tag is AnimationViewModel;

            switch (k)
            {
                case Keys.F2:
                    RenameSelectedNode();
                    break;
                case Keys.C:
                    if (isAnim && ModifierKeys == Keys.Control)
                        copyNode(n);
                    break;
                case Keys.V:
                    pasteNode();
                    break;
                case Keys.Delete:
                    RemoveSelectedNode();
                    break;
            }
        }

        TreeNode clipboardedNode = null;

        void copyNode(TreeNode n) => clipboardedNode = n;


        void pasteNode()
        {
            var srcAnim = clipboardedNode?.Tag as AnimationViewModel;
            if (srcAnim == null)
                return;

            var destNode = SelectedNode;
            if (destNode.Tag is AnimationViewModel)
                destNode = destNode.Parent;

            //get a name for the new node
            var destName = ShanoPath.Combine(destNode.Name, clipboardedNode.Text);
            var i = 1;
            while (animations.ContainsKey(destName))
                destName = $"{destNode.Name}/{clipboardedNode.Text} ({++i})";

            //create and fill the animation
            var newAnim = new AnimationViewModel(srcAnim)
            {
                Name = destName
            };
            animations.Add(newAnim);
            var n = addNode(newAnim);
            SelectedNode = n;
        }

        /// <summary>
        /// Builds the directory tree for the given animation
        /// up to the last segment. 
        /// </summary>
        /// <param name="anim"></param>
        TreeNode addNode(AnimationViewModel anim)
        {
            var segments = ShanoPath.SplitPath(anim.Name);

            var curNode = Root;

            //follow or create the directory structure
            for (var i = 0; i < segments.Length; i++)
            {
                var curName = segments[i];
                var curPath = ShanoPath.Combine(segments.Take(i + 1));

                var childNode = curNode.Nodes.Find(curPath, false).FirstOrDefault();

                if (childNode == null)
                {
                    childNode = new TreeNode
                    {
                        Name = curPath,
                        Text = curName,

                    };
                    curNode.Nodes.Add(childNode);
                }

                curNode = childNode;
            }

            //set the last node's properties
            curNode.Tag = anim;
            curNode.ImageIndex = 1;
            curNode.SelectedImageIndex = 1;

            return curNode;
        }

        void removeAnimNode(TreeNode nd)
        {
            //if compound, keep the sub-anims
            //but demote the node to a folder
            if (nd.Nodes.Count > 0)
            {
                nd.Tag = null;
                nd.ImageIndex = nd.SelectedImageIndex = 0;
                return;
            }

            // otherwise remove the node and any parent folder
            // that has the node as their single child
            while (nd.Parent != Root
                && !(nd.Parent.Tag is AnimationViewModel)
                && nd.Parent.Nodes.Count == 1)
                nd = nd.Parent;

            nd.Remove();
        }

        void startRename(TreeNode n)
        {
            if (n == null || n == Root)
                return;

            SelectedNode = n;
            n.BeginEdit();
        }

        bool finishRename(TreeNode renamedNode, string newLastName)
        {
            if (renamedNode == null || renamedNode == Root || string.IsNullOrEmpty(newLastName))
                return false;

            var anim = renamedNode.Tag as AnimationViewModel;
            if (anim != null)
            {
                //an animation
                var fullName = ShanoPath.Combine(renamedNode.Parent.Name, newLastName);
                renameAnimation(renamedNode, anim, fullName);
            }
            else
            {
                //a directory
                var renamedNodes = enumNodes(renamedNode)
                    .Where(nn => nn.Tag is AnimationViewModel);

                //rename all animations inside it
                var newPath = ShanoPath.Combine(renamedNode.Parent.Name, newLastName);

                foreach (var n in renamedNodes)
                {
                    var nAnim = (AnimationViewModel)n.Tag;
                    var relName = ShanoPath.GetRelativePath(nAnim.Name, renamedNode.Name);
                    var fullName = ShanoPath.Combine(newPath, relName);
                    if (!animations.ContainsKey(fullName))
                    {
                        //model
                        nAnim.Name = fullName;

                        //view
                        refreshNode(n);
                    }
                    ModelChanged?.Invoke();
                }


            }
            return true;
        }

        bool renameAnimation(TreeNode n, AnimationViewModel anim, string fullName)
        {
            if (anim == null || anim.Name == fullName)
                return false;

            if (animations.ContainsKey(fullName))
                return false;

            //model
            anim.Name = fullName;
            ModelChanged?.Invoke();

            //view
            refreshNode(n);

            return true;
        }

        IEnumerable<TreeNode> enumNodes(TreeNode start)
        {
            Stack<TreeNode> q = new Stack<TreeNode>();
            q.Push(start);
            while (q.Count > 0)
            {
                var elem = q.Pop();
                foreach (TreeNode e in elem.Nodes)
                    q.Push(e);

                yield return elem;
            }
        }

        IEnumerable<AnimationViewModel> enumAnimations(TreeNode start)
            => enumNodes(start)
            .Select(e => e.Tag as AnimationViewModel)
            .Where(a => a != null);


        #endregion
    }
}
