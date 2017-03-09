using Shanism.Common;
using Shanism.Common.Util;
using Shanism.Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shanism.Editor.Views.Textures
{
    sealed class TextureNode : ShanoTreeNode
    {
        static readonly Regex invalidNameRegex = new Regex($"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]");

        public TextureViewModel Texture { get; }

        public TextureNode(TexturesView viewControl, TextureViewModel tex)
            : base(viewControl)
        {
            Texture = tex;

            //TODO: image
            ImageIndex = 1;
            SelectedImageIndex = 1;

            Refresh();
        }

        public override void SetChecked(bool isChecked)
        {
            Texture.Included = isChecked;

            Checked = isChecked;
            RefreshParents();
        }

        public override bool TryRename(string newName)
        {
            if (Parent == null
                || string.IsNullOrEmpty(newName)
                || invalidNameRegex.IsMatch(newName))
            {
                return false;
            }

            if (!File.Exists(Texture.FullPath))
                return false;

            var fullPath = Path.Combine(Path.GetDirectoryName(Texture.FullPath), newName);
            if (File.Exists(fullPath))
                return false;

            try
            {
                File.Move(Texture.FullPath, fullPath);
            }
            catch
            {
                return false;
            }


            Texture.SetNameAndPath(fullPath);

            Refresh();
            return true;
        }

        public override void Refresh()
        {
            Text = ShanoPath.GetLastSegment(Texture.FullPath);
            Name = Texture.FullPath;
            ToolTipText = Texture.FullPath;
            Checked = Texture.Included;
        }
    }
}
