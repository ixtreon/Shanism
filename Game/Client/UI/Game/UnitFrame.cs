using Shanism.Client.Drawing;
using Shanism.Common;
using Shanism.Common.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.Client.UI.Game
{
    class UnitFrame : Control
    {
        static readonly Vector DefaultBoxSize = new Vector(0.6f, 0.2f);
        static readonly Vector DefaultSize = new Vector(0.6f, 0.5f);


        readonly UnitBox box;
        readonly BuffBar buffBar;

        public UnitSprite TargetSprite { get; set; }

        public UnitFrame()
        {
            Size = DefaultSize;

            Add(box = new UnitBox
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,
                Size = DefaultBoxSize,
            });

            Add(buffBar = new BuffBar
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,
                Location = box.Bounds.BottomLeft,
                Size = new Vector(Size.X, 0.15f),

                BackColor = Color.Transparent,
                CanHover = false,
            });
        }

        protected override void OnUpdate(int msElapsed)
        {
            box.TargetSprite = TargetSprite;
            buffBar.TargetUnit = TargetSprite?.Entity as IUnit;
        }
    }
}
