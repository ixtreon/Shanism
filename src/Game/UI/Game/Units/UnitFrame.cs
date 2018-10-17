using Shanism.Client.Sprites;
using Shanism.Client.UI.Containers;
using Shanism.Client.UI.Game.Units;
using Shanism.Common;
using Shanism.Common.Entities;
using System.Numerics;

namespace Shanism.Client.UI.Game
{
    class UnitFrame : ListPanel
    {
        const float BoxSize = 0.12f;
        
        readonly EntitySpriteBox box;
        readonly UnitInfoPanel info;
        readonly ListPanel boxAndInfoPanel;
        readonly BuffBar buffs;

        UnitSprite _target;

        public UnitSprite TargetSprite
        {
            get => _target;
            set
            {
                if(_target == value)
                    return;

                var u = value?.Entity as IUnit;

                box.Target = value;
                info.Target = buffs.Target = u;

                _target = value;
                IsVisible = value != null;
            }
        }

        public UnitFrame()
            : base(Direction.TopDown, sizeMode: ListSizeMode.ResizeBoth)
        {
            // picture box + info panel
            Add(boxAndInfoPanel = new ListPanel(Direction.LeftToRight, sizeMode: ListSizeMode.ResizeBoth)
            {
                Padding = 0,
                ControlSpacing = Vector2.Zero,
                BackColor = UiColors.ControlBackground,
            });
            {
                boxAndInfoPanel.Add(box = new EntitySpriteBox());
                boxAndInfoPanel.Add(info = new UnitInfoPanel
                {
                    BackColor = Color.Transparent,
                    ControlSpacing = new Vector2(DefaultPadding / 2),
                    ContentWidth = 0.3f,
                });
            }

            // buff bar
            Add(buffs = new BuffBar
            {
                Size = new Vector2(0.5f, 0.15f),
                CanHover = false,
            });

            box.Size = new Vector2(info.Height);

            IsVisible = false;
        }
    }
}
