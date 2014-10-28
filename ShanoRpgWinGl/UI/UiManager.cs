using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO;
using IO.Commands;
using Microsoft.Xna.Framework;
using ShanoRpgWinGl.Objects;
using Microsoft.Xna.Framework.Input;

namespace ShanoRpgWinGl.UI
{
    class UiManager : Control
    {
        readonly HeroUi heroUi;

        readonly TargetUi targetUi;

        readonly SpellButton mainAbilityButton;

        readonly IHero hero;

        readonly IServer input;

        public UnitControl Target
        {
            get { return targetUi.Target; }
            set { targetUi.Target = value; }
        }

        public UiManager(IHero hero, IO.IServer input)
        {
            this.Size = new Vector2(2f, 2f);
            this.AbsolutePosition = new Vector2(-1, -1);
            this.hero = hero;
            this.input = input;
            
            this.heroUi = new HeroUi(hero);
            this.Add(heroUi, false);

            this.mainAbilityButton = new SpellButton()
            {
                Ability = hero.Abilities.First(),
                AbsolutePosition = new Vector2(0.3f, 0.3f),
            };

            this.targetUi = new TargetUi();
            this.Add(targetUi, false);


            this.Add(mainAbilityButton, false);

            this.Add(new ToolTip());
            this.Add(new SpellBar()
            {
                AbsolutePosition = new Vector2(0, 0),
            }, false);

        }
        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);
            base.UpdateMain(msElapsed);     // this is the root control. 

            //check fire!
            if (this.MouseOver && mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                var a = mainAbilityButton.Ability;
                if (a.CurrentCooldown <= 0)
                {
                    var pos = Screen.ScreenToGame(mouseState.Position).ToVector();
                    var actionId = a.Name;
                    var args = new ActionArgs(actionId, pos);
                    input.RegisterAction(args);
                }
            }

        }
    }
}
