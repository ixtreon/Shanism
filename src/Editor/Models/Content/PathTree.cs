using System.Text;

namespace Shanism.Editor.Models.Content
{

    public class PathTree<T>
        where T : class
    {
        public TreeNode<T> Root { get; } = new TreeNode<T>(string.Empty);


        public void Add(string path, T value)
        {
            Root.GetOrCreate(path, value);
        }

        public bool Rename(string oldKey, string newKey)
        {
            return Root.TryRename(oldKey, newKey);
        }

        public bool Remove(string path)
        {
            return Root.TryRemove(path, out var node);
        }

        public bool TryFind(string path, out T value)
        {
            if (Root.TryFind(path, out var node) && node.Item != null)
            {
                value = node.Item;
                return true;
            }

            value = null;
            return false;
        }

        public void WriteTo(StringBuilder sb)
            => Root.WriteTo(string.Empty, sb);

        public override string ToString()
        {
            var sb = new StringBuilder();
            WriteTo(sb);
            return sb.ToString();
        }
    }
}
