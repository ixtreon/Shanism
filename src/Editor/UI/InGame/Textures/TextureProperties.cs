using Ix.Math;
using Shanism.Client;
using Shanism.Client.UI;
using Shanism.Client.UI.Containers;
using Shanism.Editor.Controllers.Content;
using System;
using System.Numerics;

namespace Shanism.Editor.UI.InGame.Textures
{
    class TextureProperties : ListPanel
    {
        readonly TexturesController controller;

        readonly IntLabel xSplits, ySplits;
        readonly TextLabel nameLabel;

        ShanoTexture texture;

        public TextureProperties(TexturesController controller)
            : base(Direction.TopDown)
        {
            this.controller = controller;

            Size = new Vector2(0.4f);

            var labelWidth = 0.2f;

            Add(xSplits = new IntLabel
            {
                Text = "Splits X",
                Font = Content.Fonts.NormalFont,

                SizeMode = SplitSizeMode.FixedFirst,
                SplitAt = labelWidth,

                Width = ClientBounds.Width,
                ParentAnchor = AnchorMode.Left | AnchorMode.Top,

                MinValue = 1,
            });

            Add(ySplits = new IntLabel
            {
                Text = "Splits Y",
                Font = Content.Fonts.NormalFont,

                SizeMode = SplitSizeMode.FixedFirst,
                SplitAt = labelWidth,

                Width = ClientBounds.Width - 0.3f,
                ParentAnchor = AnchorMode.Horizontal | AnchorMode.Top,

                MinValue = 1,
            });

            ;
            Add(nameLabel = new TextLabel
            {
                Text = "Name",
                Font = Content.Fonts.NormalFont,

                SizeMode = SplitSizeMode.FixedFirst,
                SplitAt = labelWidth,

                Width = ClientBounds.Width,
                ParentAnchor = AnchorMode.Horizontal | AnchorMode.Top,
            });
            nameLabel.Control.CanFocus = false;


            controller.TextureModified += onTextureModified;
            xSplits.Control.ValueChanged += (o, e) 
                => controller.Resize(texture, new Point(xSplits.Value, texture.CellCount.Y));
            ySplits.Control.ValueChanged += (o, e) 
                => controller.Resize(texture, new Point(texture.CellCount.X, ySplits.Value));

            Hide();
        }

        void onTextureModified(ShanoTexture tex)
        {
            if (tex != texture)
                return;

            nameLabel.ValueText = tex.Name;

            xSplits.Value = tex.CellCount.X;
            ySplits.Value = tex.CellCount.Y;

            xSplits.Control.MaxValue = Math.Min(1024, (int)tex.Size.X);
            ySplits.Control.MaxValue = Math.Min(1024, (int)tex.Size.Y);
        }

        void onSizeXChanged(Control c, EventArgs e)
        {
            controller.Resize(texture, new Point(xSplits.Value, ySplits.Value));
        }

        internal void SetTexture(ShanoTexture tex)
        {
            texture = tex;
            onTextureModified(tex);
            Show();
        }
    }
}
