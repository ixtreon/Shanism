using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO;
using Client.Objects;
using Microsoft.Xna.Framework.Input;
using Client.Controls;
using Client.UI.Menus;
using IO.Message.Client;
using IO.Objects;
using IO.Common;
using Color = Microsoft.Xna.Framework.Color;

namespace Client.UI
{
    class UiManager : Control
    {
        readonly UnitFrame heroFrame;
        readonly UnitFrame targetFrame;

        readonly SpellBar abilityBar;
        readonly SpellButton mainAbilityButton;
        readonly SpellButton secondaryAbilityButton;

        readonly SpellBook spellbookMenu;
        readonly MainMenu mainMenu;

        readonly CastBar castBar;

        readonly ChatBox chatBox;

        readonly CharacterMenu characterMenu;


        readonly BuffBar HeroBuffs;

        HeroControl _mainHeroControl;
        public HeroControl MainHeroControl
        {
            get { return _mainHeroControl; }
            set
            {
                if(_mainHeroControl != value)
                {
                    _mainHeroControl = value;

                    abilityBar.Controls
                        .OfType<SpellButton>()
                        .Zip(MainHero.Abilities,
                            (sb, a) => sb.Ability = a)
                        .ToArray();
                }
            }
        }

        public IHero MainHero {  get { return MainHeroControl?.Hero; } }

        public event Action<ActionMessage> OnActionPerformed;

        public UnitControl Target
        {
            get { return targetFrame.Target; }
            set { targetFrame.Target = value; }
        }

        public UiManager()
        {
            Size = new Vector(2, 1);

            //hero panel
            this.heroFrame = new UnitFrame
            {
                ParentAnchor = AnchorMode.Top,
                Location = new Vector(0.25, 0),
                Size = new Vector(0.5, 0.15),
            };
            this.Add(heroFrame);

            //target panel
            this.targetFrame = new UnitFrame
            {
                ParentAnchor = AnchorMode.Top,
                Location = new Vector(1.25, 0),
                Size = new Vector(0.5, 0.15),
            };
            this.Add(targetFrame);

            const double btnSize = 0.15;

            //main ability
            this.mainAbilityButton = new SpellButton(sz: btnSize)
            {
                ParentAnchor = AnchorMode.Bottom,
                Location = new Vector(0.4, 1 - btnSize),
            };
            this.Add(mainAbilityButton);

            //secondary ability
            this.secondaryAbilityButton = new SpellButton(sz: btnSize)
            {
                ParentAnchor = AnchorMode.Bottom,
                Location = new Vector(1.6 - btnSize, 1 - btnSize),
            };
            this.Add(secondaryAbilityButton);

            //ability bar
            this.abilityBar = new SpellBar
            {
                ParentAnchor = AnchorMode.Bottom,
                Location = new Vector(0.6, 0.8),
            };
            this.Add(abilityBar);

            //main menu
            this.mainMenu = new MainMenu
            {
                ParentAnchor = AnchorMode.None,
                Location = new Vector(0.6, 0.2),
                Size = new Vector(0.8, 0.6),
            };
            this.Add(mainMenu);

            //spell-book
            this.spellbookMenu = new SpellBook();
            this.Add(spellbookMenu);

            //character info
            this.characterMenu = new CharacterMenu();
            this.Add(characterMenu);

            //chatbox
            this.chatBox = new ChatBox();
            //this.Add(chatBox);

            //cast bar
            var castBarSize = new Vector(0.5f, 0.08f);
            this.castBar = new CastBar()
            {
                Size = castBarSize,
                Location = new Vector(-castBarSize.X / 2, 0.7),
            };
            this.Add(castBar, false);

            //buff bar
            this.HeroBuffs = new BuffBar()
            {
                AbsolutePosition = new Vector(0, 0),
            };
            //this.Add(HeroBuffs);

            //tooltip
            this.Add(new Tooltips.SimpleTip());
            this.Add(new Tooltips.AbilityTip());

            //spellbar?!
            //this.Add(new SpellBar()
            //{
            //    AbsolutePosition = new Vector(-1, 0.5f),
            //}, false);
        }

        public override void Update(int msElapsed)
        {
            base.UpdateMain(msElapsed);     // this is always the root control. 


            castBar.Target = MainHero;
            HeroBuffs.Target = MainHero;
            spellbookMenu.Target = MainHero;
            characterMenu.Target = MainHero;
            heroFrame.Target = MainHeroControl;
            targetFrame.Target = MainHeroControl;

            var mouseOnScreen = this.MouseOver || (HoverControl != null && HoverControl is ObjectControl);
            //check fire!
            if (mouseOnScreen)
            {
                IAbility a=null;
                if (mouseState.LeftButton == ButtonState.Pressed)
                    a = mainAbilityButton.Ability;
                if (mouseState.RightButton == ButtonState.Pressed)
                    a = secondaryAbilityButton.Ability;

                if (a != null && a.CurrentCooldown <= 0)
                {
                    var actionId = a.Name;
                    var targetLoc = Screen.ScreenToGame(mouseState.Position.ToPoint());
                    var targetGuid = (HoverControl as ObjectControl)?.Object.Guid ?? -1;
                    var message = new ActionMessage(actionId, targetGuid, targetLoc);

                    OnActionPerformed?.Invoke(message);
                }
            }
        }
    }
}
