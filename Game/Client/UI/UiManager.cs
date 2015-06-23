using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO;
using Microsoft.Xna.Framework;
using Client.Objects;
using Microsoft.Xna.Framework.Input;
using Client.Controls;
using Client.UI.Menus;
using IO.Message.Client;

namespace Client.UI
{
    class UiManager : Control
    {
        readonly HeroUi heroUi;

        readonly TargetUi targetUi;

        readonly SpellButton mainAbilityButton;
        readonly SpellButton secondaryAbilityButton;

        readonly SpellBook spellBook;
        readonly MainMenu mainMenu;
        readonly CharacterInfo charInfo;
        readonly CastBar castBar;

        readonly ChatBox chatBox;


        readonly BuffBar HeroBuffs;

        public IHero TargetHero { get; set; }

        public event Action<ActionMessage> OnActionPerformed;

        public UnitControl Target
        {
            get { return targetUi.Target; }
            set { targetUi.Target = value; }
        }

        public UiManager()
        {
            // self checks
            this.Size = new Vector2(2f, 2f);
            this.AbsolutePosition = new Vector2(-1, -1);
            
            //hero panel
            this.heroUi = new HeroUi();
            this.Add(heroUi, false);

            //target panel
            this.targetUi = new TargetUi();
            this.Add(targetUi, false);

            const float btnSize = 0.2f;

            //main ability
            this.mainAbilityButton = new SpellButton()  
            {
                Size = new Vector2(btnSize),
                //Ability = hero.Abilities.First(),
                RelativePosition = new Vector2(heroUi.Left - btnSize, heroUi.Bottom - btnSize),
            };
            this.Add(mainAbilityButton, true);

            //secondary ability
            this.secondaryAbilityButton = new SpellButton()
            {
                Size = new Vector2(btnSize),
                //Ability = hero.Abilities.Skip(1).First(),
                RelativePosition = new Vector2(heroUi.Right, heroUi.Bottom - btnSize),
            };
            this.Add(secondaryAbilityButton, true);

            //main menu
            this.mainMenu = new MainMenu();
            this.Add(mainMenu, false);

            //spell-book
            this.spellBook = new SpellBook();
            this.Add(spellBook, false);

            //character info
            this.charInfo = new CharacterInfo();
            this.Add(charInfo, false);

            //chatbox
            this.chatBox = new ChatBox();
            this.Add(chatBox);

            //cast bar
            var castBarSize = new Vector2(0.5f, 0.08f);
            this.castBar = new CastBar()
            {
                Size = castBarSize,
                AbsolutePosition = new Vector2(
                    -castBarSize.X / 2, 
                    heroUi.AbsolutePosition.Y - 2 * castBarSize.Y),
            };
            this.Add(castBar, false);

            //buff bar
            this.HeroBuffs = new BuffBar()
            {
                AbsolutePosition = new Vector2(0, 0),
            };
            this.Add(HeroBuffs);

            //tooltip
            this.Add(new ToolTip());

            //spellbar?!
            //this.Add(new SpellBar()
            //{
            //    AbsolutePosition = new Vector2(-1, 0.5f),
            //}, false);

        }

        public override void Update(int msElapsed)
        {
            base.UpdateMain(msElapsed);     // this is always the root control. 


            castBar.Target = TargetHero;
            HeroBuffs.Target = TargetHero;
            heroUi.Target = TargetHero;
            spellBook.Target = TargetHero;
            charInfo.Target = TargetHero;

            var mouseOnScreen = this.MouseOver || (HoverControl != null && typeof(ObjectControl).IsAssignableFrom(HoverControl.GetType()));
            //check fire!
            if (mouseOnScreen)
            {
                IAbility a=null;
                if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    a = mainAbilityButton.Ability;
                if (mouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    a = secondaryAbilityButton.Ability;

                if (a != null && a.CurrentCooldown <= 0)
                {
                    var pos = Screen.ScreenToGame(mouseState.Position).ToVector();
                    var actionId = a.Name;
                    var message = new ActionMessage(actionId, pos);

                    OnActionPerformed?.Invoke(message);
                }
            }
        }
    }
}
