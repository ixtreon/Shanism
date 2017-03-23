using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.UI.Chat;
using Shanism.Client.UI.Game;
using Shanism.Client.UI.Menus;
using Shanism.Common;

namespace Shanism.Client.UI
{
    partial class GameRoot
    {
        /// <summary>
        /// Removes and then recreates all UI controls.
        /// </summary>
        public void ReloadUi()
        {
            RemoveAll();

            //tooltips
            Add(new Tooltips.SimpleTip());
            Add(new Tooltips.AbilityTip());

            /* add controls: order is important, unless ZValue is manually set. */
            addIndicators();
            addUnitFrames();
            addBars();
            addChat();

            //menus
            Add(Menus = new MenuBar(this));
        }


        /// <summary>
        /// Adds the hero, target & hover unit frames.
        /// </summary>
        void addUnitFrames()
        {
            var unitFrameXOffset = 0.25;

            Add(HeroFrame = new UnitFrame
            {
                ParentAnchor = AnchorMode.Top,
                Location = new Vector(1 - unitFrameXOffset - UnitBox.DefaultSize.X, 0),
            });

            Add(TargetFrame = new UnitFrame
            {
                ParentAnchor = AnchorMode.Top,
                Location = new Vector(1 + unitFrameXOffset, 0),
            });

            Add(HoverFrame = new UnitHoverFrame(0.02, 0.02)
            {
                ParentAnchor = AnchorMode.Top,
            });
        }

        /// <summary>
        /// Adds the spell and cast bar.
        /// </summary>
        void addBars()
        {
            var castBarSize = new Vector(0.5f, 0.08f);

            Add(HeroAbilities = new SpellBar(0)
            {
                ParentAnchor = AnchorMode.Bottom,
                Location = new Vector(0.6, 0.8),
            });
            Add(HeroCastBar = new CastBar
            {
                ParentAnchor = AnchorMode.Bottom,

                Size = castBarSize,
                Location = new Vector(1 - castBarSize.X / 2, 0.55),
            });

            HeroAbilities.AbilityActivated += (a) => AbilityActivated?.Invoke(a);
        }

        /// <summary>
        /// Adds the chat bar and box.
        /// </summary>
        void addChat()
        {
            var chatFont = Content.Fonts.NormalFont;
            var chatSize = new Vector(HeroAbilities.Size.X, chatFont.HeightUi + 2 * Padding);

            Add(ChatBar = new ChatBar(serverChatEndpoint)
            {
                ParentAnchor = AnchorMode.Bottom,
                Font = chatFont,
                Size = chatSize,
                Location = HeroAbilities.Location - new Vector(0, chatSize.Y),
            });
            Add(ChatBox = new ChatFrame(ChatBar, serverChatSource)
            {
                Size = new Vector(chatSize.X, ChatFrame.DefaultSize.Y),
                Location = new Vector(2, 0) - new Vector(chatSize.X, 0),
                ParentAnchor = AnchorMode.Right | AnchorMode.Top,
            });
        }

        /// <summary>
        /// Adds range indicator, floating and error text.
        /// </summary>
        void addIndicators()
        {
            Add(FloatingText = new FloatingTextProvider());
            Add(RangeIndicator = new RangeIndicator());
            Add(Errors = new ErrorTextControl());
        }
    }
}
