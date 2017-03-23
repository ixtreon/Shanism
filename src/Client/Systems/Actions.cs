using Shanism.Client.UI.Game;
using Shanism.Common;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Interfaces.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.UI;
using Shanism.Client.Input;
using Shanism.Common.Message.Client;

namespace Shanism.Client.Systems
{
    class ActionSystem : IClientSystem
    {

        FloatingTextProvider ErrorTextProvider => root.FloatingText;

        readonly GameRoot root;
        readonly SpriteSystem objects;
        readonly MouseInfo mouse;
        readonly PlayerState playerState;

        public IHero Hero => objects.MainHero;


        public ActionSystem(GameRoot root, MouseInfo mouse, 
            PlayerState playerState,
            SpriteSystem objects)
        {
            this.root = root;
            this.mouse = mouse;
            this.playerState = playerState;
            this.objects = objects;

            this.root.AbilityActivated += CastAbility;
        }

        public void Update(int msElapsed)
        {
            if (Hero == null)
                return;

            //cast only if there's an ability
            var ab = SpellBarButton.CurrentSpellButton?.Ability;
            if (ab == null)
            {
                playerState.ActionId = 0;
                return;
            }

            //cast only if rightdown or if instacast
            if (!mouse.RightDown && ab.TargetType != AbilityTargetType.NoTarget)
            {
                playerState.ActionId = 0;
                return;
            }

            //cast only if not passive
            if (ab.TargetType == AbilityTargetType.Passive)
            {
                playerState.ActionId = 0;
                return;
            }

            //instacasts are spammed until server registers it
            if (ab.TargetType == AbilityTargetType.NoTarget
                && ab.CurrentCooldown > 0)
            {
                playerState.ActionId = 0;

                if (SpellBarButton.PreviousSpellButton != null)
                    SpellBarButton.PreviousSpellButton.IsSelected = true;
                else
                    SpellBarButton.CurrentSpellButton.IsSelected = false;
                return;
            }

            CastAbility(ab);
        }

        public void CastAbility(IAbility ab)
        {
            var targetLoc = getCastTargetLocation(ab);

            //cooldown
            if (ab.CurrentCooldown > 0)
            {
                if (mouse.RightJustPressed)
                    root.FloatingText.AddLabel(targetLoc, $"{ab.CurrentCooldown / 1000.0:0.0} sec!", Color.Red, FloatingTextStyle.Top);
                return;
            }

            //target
            if (ab.TargetType != AbilityTargetType.NoTarget
                && targetLoc.DistanceTo(Hero.Position) > ab.CastRange)
            {
                if (mouse.RightJustPressed)
                {
                    root.FloatingText.AddLabel(targetLoc, "Out of range", Color.Red, FloatingTextStyle.Top);
                    root.RangeIndicator.Show(ab.CastRange, 1250);
                }
                return;
            }

            //mana
            if (Hero.Mana < ab.ManaCost)
            {
                if (mouse.RightJustPressed)
                {
                    root.FloatingText.AddLabel(targetLoc, "Not enough mana", Color.Red, FloatingTextStyle.Top);
                }
                return;
            }


            //cast abilities if button is held
            playerState.ActionId = ab.Id;
            playerState.ActionTargetId = objects.HoverSprite?.Entity.Id ?? 0;
            playerState.ActionTargetLocation = targetLoc;
        }

        Vector getCastTargetLocation(IAbility ab)
        {
            var mousePos = mouse.InGamePosition;
            if (!Settings.Current.ExtendCast)
                return mousePos;

            var castRange = ab.CastRange;
            var heroPos = objects.MainHero.Position;
            var distVector = mousePos - heroPos;
            var distScalar = distVector.Length();
            if (distScalar <= castRange)
                return mousePos;

            castRange *= 0.95;
            return heroPos + distVector * (castRange / distScalar);
        }

    }
}
