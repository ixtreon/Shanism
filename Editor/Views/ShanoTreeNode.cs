using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shanism.Editor.Views
{
    class ShanoTreeNode : TreeNode
    {
        public ScenarioControl ViewControl { get; }

        public ShanoTreeNode(ScenarioControl viewControl)
        {
            ViewControl = viewControl;
        }

        public virtual bool TryRename(string newName) { return false; }

        /// <summary>
        /// Refreshes this instance. 
        /// </summary>
        public virtual void Refresh() { }

        public virtual void SetChecked(bool isChecked)
        {
            Checked = isChecked;
        }

        /// <summary>
        /// Refreshes this node along with its parent chain.
        /// </summary>
        public void RefreshParents()
        {
            ShanoTreeNode n = this;
            do n.Refresh();
            while ((n = n.Parent as ShanoTreeNode) != null);

        }

        /// <summary>
        /// Refreshes this node along with all of its children. 
        /// </summary>
        public void RefreshRecurse()
        {
            Refresh();

            foreach (var n in Nodes.OfType<ShanoTreeNode>())
                n.RefreshRecurse();
        }


        public T AddOrRefresh<T>(string nodeName, Func<T> addValueFactory)
            where T : ShanoTreeNode
        {
            var texNode = TryFind<T>(nodeName);
            if (texNode == null)
                Nodes.Add(texNode = addValueFactory());
            else
                texNode.Refresh();

            return texNode;
        }

        public T TryFind<T>(string name) where T : ShanoTreeNode
            => Nodes.Find(name, false)
            .OfType<T>()
            .FirstOrDefault();

        public IEnumerable<T> GetNodes<T>()
            where T : ShanoTreeNode
        {
            var l = new List<T>();
            var s = new Stack<TreeNode>();
            s.Push(this);
            do
            {
                var p = s.Pop();
                foreach (TreeNode n in p.Nodes)
                {
                    s.Push(n);
                    if (n is T)
                        l.Add((T)n);
                }
            }
            while (s.Any());

            return l;
        }
    }
}
