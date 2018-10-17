using Shanism.Client.IO;
using Shanism.Client.Systems;
using Shanism.Client.UI;
using Shanism.Common;
using Shanism.Common.Entities;
using Shanism.Common.Messages;
using Shanism.Common.Objects;
using System;
using System.Numerics;

namespace Shanism.Client.Game.Systems
{
    /// <summary>
    /// Casts abilities
    /// </summary>
    class ActionSystem : ISystem
    {
        const float OutOfRangePenalty = 0.95f;
        const float PreCastAllowanceInMs = 300;

        readonly MouseSystem mouse;
        readonly EntitySystem objects;
        readonly PlayerState playerState;

        IAbility currentAbility;
        Vector2 currentLocation;


        /// <summary>
        /// Run when the player attempts to activate an action. 
        /// Note that the <see cref="IAbility"/> param may be null.
        /// </summary>
        public event Action<AbilityCastArgs> CastAttempt;

        IHero Hero => objects.Hero;


        public ActionSystem(MouseSystem mouse, PlayerState playerState, EntitySystem objects)
        {
            this.playerState = playerState;
            this.objects = objects;
            this.mouse = mouse;
        }


        public void Update(int msElapsed)
        {
            if (Hero == null)
                return;

            //update current ability & loc
            currentAbility = SpellBarButton.CurrentSpellButton?.Ability;
            currentLocation = getCurrentCastLocation();

            if (currentAbility == null)
            {
                playerState.CancelAction();
                return;
            }

            //cast only if rightdown or if instacast
            if (!keepCastingCurrentSpell())
            {
                playerState.CancelAction();
                return;
            }

            //instacasts are spammed until server registers it
            if (currentAbility.TargetType == AbilityTargetType.NoTarget
                && currentAbility.CurrentCooldown > 0)
            {
                playerState.CancelAction();

                //set previous btn
                SpellBarButton.DeselectCurrent();
                return;
            }

            //update cast data as long as btn is held
            tryCastCurrentAbility();
        }

        public void CastAbility(IAbility ab)
        {
            currentAbility = ab;
            currentLocation = getCurrentCastLocation();

            tryCastCurrentAbility();
        }

        bool keepCastingCurrentSpell()
            => mouse.RightDown
            || currentAbility.TargetType == AbilityTargetType.NoTarget;

        Vector2 getCurrentCastLocation()
            => getCastTargetLocation(currentAbility,
                Hero.Position, mouse.InGamePosition,
                Settings.Current.ExtendCast);


        void tryCastCurrentAbility()
        {
            var outcome = canCastCurrentAbility2();
            if (outcome != ActionOutcome.Success)
            {
                playerState.CancelAction();
            }
            else
            {
                var targetID = objects.HoverSprite?.Entity.Id ?? 0;
                playerState.DoAction(currentAbility.Id, targetID, currentLocation);
            }

            CastAttempt?.Invoke(new AbilityCastArgs
            {
                Ability = currentAbility,
                Outcome = outcome,
                TargetLocation = currentLocation,
            });
        }

        ActionOutcome canCastCurrentAbility2()
            => currentAbility == null ? ActionOutcome.NoAbility
            : (currentAbility.TargetType == AbilityTargetType.Passive) ? ActionOutcome.PassiveAbility
            : !doCooldownCheck2() ? ActionOutcome.InCooldown
            : !doTargetCheck2() ? ActionOutcome.OutOfRange
            : !doManaCheck2() ? ActionOutcome.OutOfMana
            : ActionOutcome.Success;

        bool doCooldownCheck2()
            => currentAbility.CurrentCooldown <= 0;

        bool doTargetCheck2()
            => currentAbility.TargetType == AbilityTargetType.NoTarget
            || currentLocation.DistanceTo(Hero.Position) <= currentAbility.CastRange;

        bool doManaCheck2()
            => Hero.Mana >= currentAbility.ManaCost;

        static Vector2 getCastTargetLocation(IAbility ab,
            Vector2 heroPos, Vector2 mousePos, bool extendCast)
        {
            // no ability or direct cast -> mouse pos
            if (ab == null || !extendCast)
                return mousePos;

            // extend cast -> mouse pos within ability range
            var castRange = ab.CastRange;
            var distVector = mousePos - heroPos;
            var distScalar = distVector.Length();
            if (distScalar <= castRange)
                return mousePos;

            castRange *= OutOfRangePenalty;
            return heroPos + distVector * (castRange / distScalar);
        }
    }

    enum ActionOutcome
    {
        Success,

        NoAbility,
        PassiveAbility,

        OutOfRange,
        OutOfMana,
        InCooldown,
    }

    struct AbilityCastArgs
    {
        public ActionOutcome Outcome { get; set; }

        public Vector2 TargetLocation { get; set; }

        public IAbility Ability { get; set; }
    }

}
