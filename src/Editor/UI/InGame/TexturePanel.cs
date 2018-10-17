using Shanism.Client;
using Shanism.Client.UI;
using Shanism.Client.UI.Containers;
using Shanism.Common;
using Shanism.Editor.Controllers.Content;
using Shanism.Editor.UI.InGame.Textures;

namespace Shanism.Editor.UI.InGame
{
    class TexturePanel : Control
    {
        readonly TexturesController controller;


        readonly ListPanel toolBar;
        readonly TreeControl<ShanoTexture> texList;

        readonly TexturePreview textureView;
        readonly TextureProperties textureProps;


        public TexturePanel(TexturesController controller)
        {
            this.controller = controller;

            BackColor = UiColors.ControlBackground;

            Size = new System.Numerics.Vector2(0.6f);

            // init the UI
            toolBar = new ListPanel(Direction.LeftToRight)
            {
                BackColor = Color.Azure.SetAlpha(100),
            };

            texList = new TreeControl<ShanoTexture>(controller.Tree)
            {

            };

            textureView = new TexturePreview(controller)
            {
            };

            textureProps = new TextureProperties(controller)
            {
                BackColor = UiColors.ControlBackground,
            };

            Add(new SplitPanel(Axis.Horizontal)
            {
                ParentAnchor = AnchorMode.All,
                Bounds = ClientBounds,

                SizeMode = SplitSizeMode.FixedFirst,
                SplitAt = 0.5f,

                // split for toolbar + tex tree
                First = new SplitPanel(Axis.Vertical)
                {
                    SizeMode = SplitSizeMode.FixedFirst,
                    Height = 0.5f,
                    SplitAt = 0.1f,
                    AllowUserResize = false,

                    First = toolBar,
                    Second = texList,
                },

                // split for tex preview + properties
                Second = new SplitPanel(Axis.Vertical)
                {
                    SizeMode = SplitSizeMode.FixedSecond,
                    Height = 0.5f,
                    SplitAt = 0.15f,

                    First = textureView,
                    Second = textureProps,
                },

            });

            texList.NodeFocused += TexList_NodeFocused;
        }

        void TexList_NodeFocused(ShanoTexture tex)
        {
            textureView.SetTexture(tex);
            textureProps.SetTexture(tex);
            
        }
    }
}
