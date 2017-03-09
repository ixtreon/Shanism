using Shanism.Common;
using Shanism.Common.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shanism.Editor.Views.Textures
{
    class TextureFolderNode : ShanoTreeNode
    {
        static readonly Regex invalidNameRegex = new Regex($"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]");

        public string FolderPath { get; private set; }

        public TextureFolderNode(string folderPath)
            : base(null)
        {
            FolderPath = folderPath;
            Refresh();
        }


        //puts a path (sequence of folder nodes) 
        public TextureFolderNode RecreatePath(string dir)
        {
            var segments = dir.GetRelativePath(FolderPath).Split(ShanoPath.RecognizedDelimiters, StringSplitOptions.RemoveEmptyEntries);

            var curNode = this;
            var curPath = FolderPath;
            foreach (var segm in segments)
            {
                curPath = Path.Combine(curPath, segm);
                curNode = curNode.AddOrRefresh(curPath, () => new TextureFolderNode(curPath));
            }

            return curNode;
        }

        public override void SetChecked(bool isChecked)
        {
            foreach (var n in GetNodes<ShanoTreeNode>())
                n.SetChecked(isChecked);

            Checked = isChecked;
        }

        public override bool TryRename(string newName)
        {
            if (Parent == null
                || string.IsNullOrEmpty(newName)
                || invalidNameRegex.IsMatch(newName))
            {
                return false;
            }

            //check old is there
            var oldPath = FolderPath;
            if (!Directory.Exists(oldPath))
                return false;

            //check new is not there
            var newPath = Path.Combine(Path.GetDirectoryName(FolderPath), newName);
            if (Directory.Exists(newPath))
                return false;

            //check new is not subfolder of old
            if (ShanoPath.IsSubFolderOf(newPath, oldPath))
                return false;

            try
            {
                Directory.Move(oldPath, newPath);
            }
            catch
            {
                return false;
            }

            FolderPath = newPath;
            renameSubNodes(oldPath, newPath);
            RefreshRecurse();

            return true;
        }

        void renameSubNodes(string oldPath, string newPath)
        {
            foreach (var t in GetNodes<TextureNode>())
            {
                var newTexPath = Path.Combine(newPath, 
                    t.Texture.FullPath.GetRelativePath(oldPath));

                t.Texture.SetNameAndPath(newTexPath);
            }

            foreach (var t in GetNodes<TextureFolderNode>())
            {
                var newFolderPath = Path.Combine(newPath, 
                    t.FolderPath.GetRelativePath(oldPath));

                t.FolderPath = newFolderPath;
            }
        }

        public override void Refresh()
        {
            var texNodes = Nodes.OfType<ShanoTreeNode>();
            var isChecked = texNodes.Any() && texNodes.All(n => n.Checked);

            Checked = isChecked;

            Text = ShanoPath.GetLastSegment(FolderPath);
            Name = FolderPath;
            ToolTipText = FolderPath;
        }
    }
}
