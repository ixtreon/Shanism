using Shanism.Client;
using Shanism.Client.UI;
using Shanism.Client.UI.Containers;
using Shanism.Editor.Controllers.Content;
using System;
using System.Numerics;

namespace Shanism.Editor.UI.InGame.Textures
{
    class AnimationProperties : ListPanel
    {
        readonly AnimationController controller;

        readonly TextLabel name;
        readonly TextLabel nameLabel;

        public AnimationProperties(AnimationController controller)
            : base(Direction.TopDown)
        {
            this.controller = controller;

            Size = new Vector2(0.4f);
            
            var labelWidth = 0.2f;

            Add(nameLabel = new TextLabel
            {
                Text = "Name",
                Font = Content.Fonts.NormalFont,

                SplitAt = labelWidth,

                Width = ClientBounds.Width,
                ParentAnchor = AnchorMode.Horizontal | AnchorMode.Top,
            });

            Hide();
        }


        void foo(ShanoAnimation a)
        {
        }

    }
}
