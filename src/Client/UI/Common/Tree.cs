using System.Collections.Generic;

namespace Shanism.Client.UI
{
    public interface IHasPath
    {
        IEnumerable<string> GetPath();

    }

    public class Tree<T> : Control
        where T : IHasPath
    {
        ScrollBar scrollBar;
        TreeNodeFolder<T> root;

        public Tree()
        {
            Add(scrollBar = new ScrollBar
            {
                ParentAnchor = AnchorMode.Top | AnchorMode.Bottom | AnchorMode.Right,
                Width = 0,
            });

            Add(root = new TreeNodeFolder<T>
            {
                ParentAnchor = AnchorMode.All,
                Size = Size,
            });
        }

        void setScrollVisible(bool isVisible)
        {
            scrollBar.Width = isVisible ? ScrollBar.DefaultWidth : 0;
            root.Width = Width - scrollBar.Width;
        }

        public override void Draw(Canvas g)
        {
            base.Draw(g);
        }
    }
}
