using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Editor.Models.Content
{
    public interface ITreeNode
    {
        IEnumerable<ITreeNode> Children { get; }
        bool HasItem { get; }
        string Name { get; }
    }

    public class TreeNode<T> : ITreeNode
        where T : class
    {

        readonly Dictionary<string, TreeNode<T>> nodes = new Dictionary<string, TreeNode<T>>();

        public string Name { get; }

        public T Item { get; private set; }


        public IReadOnlyDictionary<string, TreeNode<T>> Nodes => nodes;
        public IEnumerable<TreeNode<T>> Children => nodes.Values;
        public bool HasItem => Item != null;
        bool NeedsDisposal => nodes.Count == 0 && !HasItem;


        IEnumerable<ITreeNode> ITreeNode.Children => nodes.Values;

        public TreeNode(string name)
        {
            Name = name;
        }

        public void UpdateItem(T item)
        {
            Item = item;
        }


        public TreeNode<T> GetOrCreate(string path, T value) 
            => getOrCreate(new PathView(path), value);

        public bool TryFind(string path, out TreeNode<T> node)
            => tryFind(new PathView(path), out node);

        public bool TryRemove(string path, out T value)
            => tryRemove(new PathView(path), out value);

        public bool TryRename(string oldPath, string newPath)
            => tryRename(new PathView(oldPath), new PathView(newPath));


        bool tryGet(PathView path, out TreeNode<T> value)
        {
            // root should be our child
            if(!nodes.TryGetValue(path.First, out value))
            {
                value = null;
                return false;
            }

            if(path.HasRest)
                return tryGet(path.Rest, out value);
            else
                return true;
        }

        TreeNode<T> getOrCreate(PathView path, T value)
        {
            // root should be our child
            if(!nodes.TryGetValue(path.First, out var node))
                nodes[path.First] = node = new TreeNode<T>(path.First);

            // last segment -> put value here
            if(!path.HasRest)
            {
                node.Item = value;
                return node;
            }

            // got more segments -> recurse
            return node.getOrCreate(path.Rest, value);
        }


        bool tryFind(PathView path, out TreeNode<T> node)
        {
            // no such child -> fail
            if(!nodes.TryGetValue(path.First, out node))
                return false;

            // that's all of path -> return it
            if(!path.HasRest)
                return true;

            // got more segments -> recurse
            return node.tryFind(path.Rest, out node);
        }

        bool tryRemove(PathView path, out T value)
        {
            // no such child -> fail
            if(!nodes.TryGetValue(path.First, out var node))
            {
                value = null;
                return false;
            }

            // that's all of path -> remove it
            if(!path.HasRest)
            {
                value = node.Item;
                node.Item = null;
            }
            else
            {
                // or recurse
                if(!node.tryRemove(path.Rest, out value))
                    return false;
            }

            // remove the root node, if now empty
            if(node.NeedsDisposal)
                nodes.Remove(path.First);

            return true;
        }


        bool tryRename(PathView oldPath, PathView newPath)
        {

            // same root -> recurse
            if(string.Equals(oldPath.First, newPath.First, StringComparison.InvariantCulture))
            {
                if(!nodes.TryGetValue(oldPath.First, out var node))
                    return false;

                return node.tryRename(oldPath.Rest, newPath.Rest);
            }

            // otherwise remove from old... 
            if(!tryRemove(oldPath, out var oldNode))
                return false;

            // .. and add to new
            getOrCreate(newPath, oldNode);
            return true;
        }

        public IEnumerable<TreeNode<T>> GetDescendantsAndSelf()
        {
            var stack = new Stack<TreeNode<T>>();
            stack.Push(this);

            while(stack.Any())
            {
                var node = stack.Pop();

                yield return node;

                foreach(var c in node.nodes.Values)
                    stack.Push(c);
            }
        }

        public void WriteTo(string pathSoFar, StringBuilder sb)
        {
            sb.Append(pathSoFar);
            sb.Append('/');
            if(HasItem) sb.Append('[');
            sb.Append(Name);
            if(HasItem) sb.Append(']');
            sb.AppendLine();

            pathSoFar = $"{pathSoFar}/{Name}";
            foreach(var c in nodes)
                c.Value.WriteTo(pathSoFar, sb);
        }
    }
}
