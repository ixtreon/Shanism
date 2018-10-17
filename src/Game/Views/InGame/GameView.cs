using Shanism.Client.Game.Systems;
using Shanism.Client.Sprites;
using Shanism.Client.UI;
using Shanism.Client.Views;
using Shanism.Common;
using Shanism.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.Client.Game.Views
{
    /// <summary>
    /// Contains all the drawing and update logic of the client while in-game.    
    /// </summary>
    partial class GameView : GameViewBase
    {

        readonly ShanismGameState game;

        public GameView(ShanismGameState game)
            : base(game)
        {
            CanHover = true;
            CanFocus = true;
            Padding = 0;

            this.game = game;

            game.StartedPlayingEtc += () =>
            {
                game.InitializeGameSystems(this);
                game.Actions.CastAttempt += OnPlayerCastAttempt;
            };

            ActionActivated += OnGameAction;
        }


        protected override void OnInit()
        {
            GameHelper.QuitToTitle = quit;
        }

        protected override void OnReload()
        {
            // view
            ReloadUI();
        }

        void OnPlayerCastAttempt(AbilityCastArgs args)
        {
            switch (args.Outcome)
            {
                case ActionOutcome.OutOfRange:
                    game.FloatingText.AddLabel(args.TargetLocation, "Out of range", Color.Red, FloatingTextStyle.Up);
                    RangeIndicator.Show(args.Ability.CastRange, 1250);
                    break;

                case ActionOutcome.OutOfMana:
                    game.FloatingText.AddLabel(args.TargetLocation, "Not enough mana", Color.Red, FloatingTextStyle.Up);
                    break;

                case ActionOutcome.InCooldown:
                    game.FloatingText.AddLabel(args.TargetLocation, $"{args.Ability.CurrentCooldown / 1000.0:0.0} sec!", Color.Red, FloatingTextStyle.Up);
                    break;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            // start playing
            ReloadUI();
            game.StartPlaying();
        }

        void quitWithError(string errorMsg)
        {
            ViewStack.Pop();
            ViewStack.Current.ShowMessageBox("D'oh", errorMsg);
        }

        void quit()
        {
            game.Disconnect(null);
            ViewStack.ResetToMain();
        }

        void restart()
        {
            try
            {
                game.RestartScenario();
            }
            catch (Exception e)
            {
                ShowMessageBox("Scenario Restart Failed", e.Message);
                return;
            }

            OnReload();
            game.StartPlaying();
        }



        public override void Update(int msElapsed)
        {
            //update the server
            base.Update(msElapsed);

            if (game.State != ClientState.Playing)
                return;

            //pan camera to the hero
            game.PanCameraToMainHero();

            // update UI
            UpdateControls();
        }

        void UpdateControls()
        {
            if (Menus == null)
                return;

            //update hero pointer
            if (Menus.Target != game.MainHero)
            {
                Menus.Target = game.MainHero;
                CastBar.Target = game.MainHero;

                // adds all abilities of the hero to the default bar
                var abilities = game.MainHero?.Abilities ?? Enumerable.Empty<IAbility>();
                AbilityBar.SetAbilities(abilities);
                AbilityBar.SelectButton(0);
            }

            //update hero sprite pointer
            HeroFrame.TargetSprite = game.MainHeroSprite;

            //update target & hover controls
            if (IsHoverControl)
                HoverFrame.Target = (game.HoverSprite as UnitSprite)?.Unit;
        }


        protected override void OnDraw(CanvasStarter canvas)
        {
            base.OnDraw(canvas);
        }


        public override void WriteDebugStats(List<string> debugStats)
        {
            debugStats.Add("Game");
            debugStats.AddRange(game.GetDebugLines());
        }

        protected override void OnMouseClick(MouseButtonArgs e)
        {
            base.OnMouseClick(e);

            // update hover/target
            if (e.Button == MouseButton.Left)
                TargetFrame.TargetSprite = game.HoverSprite as UnitSprite;

        }

        void OnGameAction(Control sender, ClientActionArgs e)
        {
            switch (e.Action)
            {
                case ClientAction.ShowHealthBars:
                    Settings.Current.AlwaysShowHealthBars ^= true;
                    break;


                case ClientAction.Chat:
                    ChatBar.SetFocus();
                    break;

                case ClientAction.HideMenus when game.State != ClientState.Playing:
                    quit();
                    break;

                default:
                    AbilityBar.ActivateAction(e.Action);
                    Menus.ActivateAction(e.Action);
                    break;
            }
        }
    }
}
