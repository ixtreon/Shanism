using Shanism.Client;
using Shanism.Client.UI;
using Shanism.Client.UI.Containers;
using Shanism.Common;
using Shanism.Editor.Controllers.Content;
using Shanism.Editor.UI.InGame.Animations;

namespace Shanism.Editor.UI.InGame
{
    class AnimationPanel : Control
    {
        readonly AnimationController controller;

        readonly SplitPanel mainPanel;
        readonly TreeControl<ShanoAnimation> animationList;
        readonly AnimationPreview preview;

        public AnimationPanel(AnimationController controller)
        {
            Size = new System.Numerics.Vector2(0.6f);
            this.controller = controller;

            BackColor = UiColors.ControlBackground;

            animationList = new TreeControl<ShanoAnimation>(controller.Tree);

            Add(mainPanel = new SplitPanel(Axis.Horizontal)
            {
                Bounds = ClientBounds,
                ParentAnchor = AnchorMode.All,

                SizeMode = SplitSizeMode.FixedFirst,
                SplitAt = 0.5f,

                First = animationList,
                Second = new SplitPanel(Axis.Vertical)
                {
                    First = preview = new AnimationPreview
                    {
                        BackColor = Color.BlanchedAlmond,
                    },

                },
            });

            animationList.NodeFocused += preview.SetAnimation;
        }

    }
}
