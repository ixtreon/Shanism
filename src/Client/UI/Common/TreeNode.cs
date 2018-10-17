using Shanism.Client.UI.Containers;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.Client.UI
{
    interface ITreeNode<T>
    {
        
    }

    class TreeNodeItem<T> : Control, ITreeNode<T>
    {
        public T Object { get; }

        readonly ListPanel childPanel = new ListPanel();

        public TreeNodeItem(T obj)
        {
            
            Object = obj;
        }

        protected virtual string GetText()
            => Object.ToString();

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);
        }
    }

    class TreeNodeFolder<T> : ListPanel, ITreeNode<T>
    {
        public string Name { get; set; }

        public IEnumerable<ITreeNode<T>> Nodes
            => Controls.OfType<ITreeNode<T>>();
    }
}
