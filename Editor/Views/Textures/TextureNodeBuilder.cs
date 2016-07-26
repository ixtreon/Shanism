using Shanism.Common;
using Shanism.Common.Util;
using Shanism.Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shanism.Editor.Views.Textures
{
    class TextureNodeBuilder
    {
        TextureFolderNode Root;

        public TexturesViewModel Model { get; set; }

        public void RefreshNodes(TreeNodeCollection container, TexturesView viewControl)
        {
            if (Model == null)
                throw new InvalidOperationException("Textures is null!");

            //create root inside container
            if (!container.Contains(Root))
            {
                Root?.Remove();
                Root = new TextureFolderNode(Model.ContentDirectory);
                container.Add(Root);
            }

            //get old nodes
            var oldNodes = new HashSet<TreeNode>(Root.GetNodes<ShanoTreeNode>());

            //refresh all dir/tex nodes
            foreach (var tex in Model.Textures)
            {
                var texPath = tex.FullPath;

                //walk the directory structure
                var dirPath = Path.GetDirectoryName(texPath);
                var dirNode = Root.RecreatePath(dirPath);

                //recreate the texture node
                var texNode = dirNode.AddOrRefresh(texPath, () => new TextureNode(viewControl, tex));

                //remove from old nodes
                TreeNode curNode = texNode;
                while (curNode != Root && oldNodes.Remove(curNode))
                    curNode = curNode.Parent;
            }

            //remove remaining old nodes from the tree
            foreach (var n in oldNodes)
                n.Remove();

            Root.RefreshRecurse();
        }
    }
}
