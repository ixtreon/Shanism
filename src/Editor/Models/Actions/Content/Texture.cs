using Ix.Math;
using Shanism.Client;
using Shanism.Editor.Models.Content;
using System;

namespace Shanism.Editor.Actions.Content
{

    sealed class TextureResizeAction : PropertyChangeAction<ShanoTexture, Point>
    {
        public TextureResizeAction(ShanoTexture tex, Point newValue) 
            : base(tex, t => t.CellCount, newValue)
        {
            Description = $"Resized `{Target.Name}` to {NewValue.X} x {NewValue.Y}.";
        }
    }

    sealed class TextureRenameAction : PropertyChangeAction<ShanoTexture, string>
    {
        readonly PathTree<ShanoTexture> tree;

        public TextureRenameAction(PathTree<ShanoTexture> tree, ShanoTexture tex, string newName)
            : base(tex, d => d.Name, newName)
        {
            this.tree = tree;
            Description = $"Renamed texture `{OldValue}` to `{NewValue}`.";
        }

        public override void Apply()
        {
            base.Apply();

            if (!tree.Rename(OldValue, NewValue))
                throw new InvalidOperationException($"Can't find the texture `{OldValue}` in the tree.");
        }

        public override void Revert()
        {
            if (!tree.Rename(NewValue, OldValue))
                throw new InvalidOperationException($"Can't find the texture `{NewValue}` in the tree.");

            base.Revert();
        }
    }
}
