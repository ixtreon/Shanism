﻿using System;
using System.Linq;
using Engine.Objects;
using IO.Common;
using System.Collections.Generic;
using Engine._DefaultScenario.Units;
using Engine.Objects.Game;

public class DeathWardSpell : Ability
{
    public DeathWardSpell()
        : base(AbilityTargetType.PointTarget)
    {
        CastTime = 1000;
        Icon = "ice-sky-2";
        Name = "Death Ward";
        Description = "Places a death ward which shoots nearby enemies on the specified point. ";
        ManaCost = 1;
        Cooldown = 3;
        CastRange = 5;
    }

    public override void OnCast(Engine.Events.AbilityCastArgs e)
    {
        var ward = new DeathWard(Owner.Owner, e.TargetLocation);
        Map.Add(ward);
    }
   
    public override void OnUpdate(int msElapsed)
    {
        this.Cooldown = Owner.AttackCooldown;
    }
}