using Ix.Math;
using Shanism.Client;
using Shanism.Client.Assets;
using Shanism.Editor.Actions;
using Shanism.Editor.Actions.Content;
using Shanism.Editor.Models.Content;
using System;

namespace Shanism.Editor.Controllers.Content
{
    class TexturesController
    {

        public PathTree<ShanoTexture> Tree { get; set; }

        public event Action<ShanoTexture> TextureModified;

        readonly ActionList history;

        public TexturesController(ActionList history, ContentList content)
        {
            Tree = new PathTree<ShanoTexture>();
            foreach(var tex in content.Textures)
                Tree.Add(tex.Name, tex);

            this.history = history;
            this.history.ActionDone += onHistoryAction;
            this.history.ActionUndone += onHistoryAction;
        }

        void onHistoryAction(ActionBase act)
        {
            switch(act)
            {
                case TextureResizeAction s:
                    TextureModified?.Invoke(s.Target);
                    break;

                case TextureRenameAction r:
                    TextureModified?.Invoke(r.Target);
                    break;
            }
        }

        public void Resize(ShanoTexture tex, Point newSize)
        {
            if (tex.CellCount != newSize)
                history.Do(new TextureResizeAction(tex ?? throw new ArgumentNullException(nameof(tex)), newSize));
        }

        public void RenameSelf(ShanoTexture tex, string newName)
        {
            if (tex.Name != newName)
                history.Do(new TextureRenameAction(Tree, tex, newName));
        }

        public void RenameRecursive(ShanoTexture tex, string newName)
        {
            if(!Tree.Root.TryFind(tex.Name, out var texNode))
                throw new Exception("Could not find the texture in the tree.");

            var act = new CompoundAction();
            foreach(var n in texNode.GetDescendantsAndSelf())
                if(n.HasItem)
                    act.Add(new TextureRenameAction(Tree, n.Item, newName));

            history.Do(new TextureRenameAction(Tree, tex, newName));
        }
    }
}
