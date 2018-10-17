using Shanism.Client.UI;
using Shanism.Client.UI.Chat;
using Shanism.Client.UI.Game;
using Shanism.Client.UI.Menus;
using Shanism.Client.UI.Tooltips;
using System;
using System.Numerics;

namespace Shanism.Client.Game.Views
{
    partial class GameView
    {

        public UnitFrame HeroFrame { get; private set; }
        public UnitFrame TargetFrame { get; private set; }
        public UnitInfoPanel HoverFrame { get; private set; }

        public CastBar CastBar { get; private set; }
        public SpellBar AbilityBar { get; private set; }

        public ChatBar ChatBar { get; private set; }
        public ChatFrame ChatBox { get; private set; }

        public ErrorTextControl Errors { get; private set; }
        public RangeIndicator RangeIndicator { get; private set; }

        public MenuBar Menus { get; private set; }


        public void ReloadUI()
        {
            /// model

            /// view
            RemoveAll();

            //tooltips
            AddTooltip<TextTooltip>();
            AddTooltip<AbilityTooltip>();

            // add UI controls
            // order is important, unless the ZValue is manually set
            addIndicators();
            addUnitFrames();
            addBars();
            addChat();

            //menus
            Add(Menus = new MenuBar());

            void addUnitFrames()
            {
                float UnitFrameXOffset = 0.6f;

                Add(HeroFrame = new UnitFrame
                {
                    Top = 0,
                    Right = Width / 2 - UnitFrameXOffset,
                    ParentAnchor = AnchorMode.Top,
                });

                Add(TargetFrame = new UnitFrame
                {
                    Top = 0,
                    Left = Width / 2 + UnitFrameXOffset,
                    ParentAnchor = AnchorMode.Top,
                });

                Add(HoverFrame = new UnitInfoPanel
                {
                    Width = 0.5f,

                    ControlSpacing = new Vector2(DefaultPadding / 2),
                    HealthBarHeight = 0.03f,

                    Top = 0,
                    CenterX = true,
                    ParentAnchor = AnchorMode.Top,
                });
            }

            void addBars()
            {
                var CastBarSize = new Vector2(0.5f, 0.08f);

                Add(AbilityBar = new SpellBar(0)
                {
                    ButtonCount = 16,
                    MaxButtonsPerRow = 8,

                    CenterX = true,
                    Bottom = Screen.UiSize.Y,
                    ParentAnchor = AnchorMode.Bottom,
                });
                Add(CastBar = new CastBar
                {
                    Size = CastBarSize,
                    Left = (Screen.UiSize.X - CastBarSize.X) / 2,
                    Top = 0.55f,
                    ParentAnchor = AnchorMode.Bottom,
                });
            }

            void addChat()
            {
                var ChatFont = Content.Fonts.NormalFont;
                var ChatBarSize = new Vector2(AbilityBar.Width, ChatFont.Height + 2 * Padding);
                var ChatFrameSize = new Vector2(0.7f, 0.4f);

                Add(ChatBar = new ChatBar(game.Chat)
                {
                    Font = ChatFont,

                    Size = ChatBarSize,
                    Left = AbilityBar.Left,
                    Bottom = AbilityBar.Top,

                    ParentAnchor = AnchorMode.Bottom,
                });
                Add(ChatBox = new ChatFrame(ChatBar, game.Chat)
                {
                    Size = ChatFrameSize,
                    Right = Screen.UiSize.X,
                    Top = 0,

                    ParentAnchor = AnchorMode.Right | AnchorMode.Top,
                });
            }

            void addIndicators()
            {
                Add(RangeIndicator = new RangeIndicator());
                Add(Errors = new ErrorTextControl());
            }
        }

    }
}
