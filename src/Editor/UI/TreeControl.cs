using Shanism.Client.UI;
using Shanism.Client.UI.Containers;
using Shanism.Common;
using Shanism.Editor.Models.Content;
using System;
using System.Numerics;

namespace Shanism.Editor.UI
{
    class TreeControl<T> : TreeControl, ITreeEventHandler
        where T : class
    {
        public event Action<T> NodeFocused;

        public TreeControl(PathTree<T> tree) : base(tree.Root)
        {
            // nothing here
        }

        protected override void OnNodeFocused(ITreeNode node)
        {
            if (!(node is TreeNode<T> typedNode))
                throw new Exception("Unexpected inner node type");

            NodeFocused?.Invoke(typedNode.Item);
        }
    }

    abstract class TreeControl : ListPanel, ITreeEventHandler
    {
        readonly TreeRow root;

        TreeRow selectedRow;

        public bool AllowRename { get; set; }

        public TreeControl(ITreeNode rootNode)
            : base(Client.Direction.LeftToRight, Client.ContentWrap.NoWrap)
        {
            Size = new Vector2(0.1f);
            CanFocus = true;

            selectedRow = root = new TreeRow(rootNode, this) { Width = Width };
            Add(root);
        }


        protected override void OnMouseScroll(MouseScrollArgs e)
        {
            base.OnMouseScroll(e);

            var maxScroll = Height - root.Height;
            if(maxScroll < 0)
            {
                var newPos = root.Top - 0.03f * e.ScrollDelta;
                root.Top = newPos.Clamp(maxScroll, 0);
            }
        }

        public void OnRowSelected(TreeRow row)
        {
            if (row == selectedRow || !row.Node.HasItem)
                return;

            selectedRow.IsSelected = false;
            selectedRow = row;
            selectedRow.IsSelected = true;

            OnNodeFocused(row.Node);
        }

        protected abstract void OnNodeFocused(ITreeNode node);

        public void OnRowExpanded(TreeRow row)
        {
            row.IsExpanded ^= true;
        }
    }

    interface ITreeEventHandler
    {
        void OnRowSelected(TreeRow row);
        void OnRowExpanded(TreeRow row);
    }
}
